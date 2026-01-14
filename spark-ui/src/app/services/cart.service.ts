// src/app/services/cart.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, switchMap, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';

export interface CartItem {
  id?: number;
  computerId: number;
  computerName: string;
  computerPrice: number;
  computerImageUrl: string;
  quantity: number;
  components: {
    id: number;
    name: string;
    price: number;
    type: string;
  }[];
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly API_URL = 'http://localhost:5097/api/orders';
  private cartItems = new BehaviorSubject<CartItem[]>([]);
  private isInitialized = false;

  cartItems$ = this.cartItems.asObservable();

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
    this.initializeCart();
  }

  private initializeCart(): void {
    if (this.isInitialized) return;
    
    this.authService.user$.subscribe(user => {
      if (user) {
        // User is logged in, fetch cart from server
        this.fetchCartFromServer();
      } else {
        // User is not logged in, clear cart
        this.cartItems.next([]);
      }
    });
    
    this.isInitialized = true;
  }

  private fetchCartFromServer(): void {
    this.http.get<CartItem[]>(`${this.API_URL}/cart`).subscribe({
      next: (items) => {
        this.cartItems.next(items || []);
      },
      error: (err) => {
        console.error('Failed to fetch cart:', err);
        this.cartItems.next([]);
      }
    });
  }

  private syncWithServer(item: CartItem): Observable<CartItem> {
    return this.http.post<CartItem>(`${this.API_URL}/cart`, item);
  }

  addToCart(item: Omit<CartItem, 'quantity'>): void {
  const currentItems = [...this.cartItems.value];

  const existingItem = currentItems.find(
    i => i.computerId === item.computerId &&
         this.areComponentsEqual(i.components, item.components)
  );

  if (existingItem) {
    existingItem.quantity += 1;
    this.cartItems.next(currentItems); // ✅ update UI immediately

    if (this.authService.isAuthenticated) {
      this.syncWithServer(existingItem).subscribe();
    }
  } else {
    const newItem: CartItem = { ...item, quantity: 1 };
    this.cartItems.next([...currentItems, newItem]); // ✅ show instantly

    if (this.authService.isAuthenticated) {
      this.syncWithServer(newItem).subscribe(created => {
        const updated = this.cartItems.value.map(i =>
          i === newItem ? created : i
        );
        this.cartItems.next(updated);
      });
    }
  }
}


  updateQuantity(itemId: number, quantity: number): void {
    if (quantity < 1) {
      this.removeFromCart(itemId);
      return;
    }

    const currentItems = this.cartItems.value;
    const item = currentItems.find(i => i.id === itemId);
    
    if (item) {
      item.quantity = quantity;
      this.syncWithServer(item).subscribe(updatedItem => {
        const index = currentItems.findIndex(i => i.id === updatedItem.id);
        if (index !== -1) {
          currentItems[index] = updatedItem;
          this.cartItems.next([...currentItems]);
        }
      });
    }
  }

  removeFromCart(itemId: number): void {
    const currentItems = this.cartItems.value;
    const item = currentItems.find(i => i.id === itemId);
    
    if (item) {
      this.http.delete(`${this.API_URL}/cart/${itemId}`).subscribe(() => {
        this.cartItems.next(currentItems.filter(i => i.id !== itemId));
      });
    }
  }

  clearCart(): void {
    this.http.delete(`${this.API_URL}/cart`).subscribe(() => {
      this.cartItems.next([]);
    });
  }

  getCartCount(): number {
    return this.cartItems.value.reduce((total, item) => total + item.quantity, 0);
  }

  getTotalPrice(): number {
    return this.cartItems.value.reduce(
      (total, item) => total + (item.computerPrice * item.quantity) + 
        item.components.reduce((sum, comp) => sum + comp.price, 0) * item.quantity,
      0
    );
  }
private getAuthHeaders() {
  return { headers: this.authService.authHeader };
}


checkout(): Observable<any> {
  const items = this.cartItems.value.map(item => ({
    computerId: item.computerId,
    quantity: item.quantity,
    components: item.components.map(c => ({ id: c.id }))
  }));

  return this.http.post<any>(`${this.API_URL}`, { items }, this.getAuthHeaders()).pipe(
    tap(() => {
      this.cartItems.next([]);
    })
  );
}


  private areComponentsEqual(a: any[], b: any[]): boolean {
    if (a.length !== b.length) return false;
    const aSorted = [...a].sort((x, y) => x.id - y.id);
    const bSorted = [...b].sort((x, y) => x.id - y.id);
    return JSON.stringify(aSorted) === JSON.stringify(bSorted);
  }
}