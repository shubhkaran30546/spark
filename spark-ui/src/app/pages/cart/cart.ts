// src/app/services/cart.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';


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
  cartItems$ = this.cartItems.asObservable();

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
    this.loadLocalCart();
  }

  private loadLocalCart() {
    const stored = localStorage.getItem('cart');
    if (stored) {
      this.cartItems.next(JSON.parse(stored));
    }
  }

  private saveLocalCart() {
    localStorage.setItem('cart', JSON.stringify(this.cartItems.value));
  }

  private authHeaders() {
    return { headers: this.authService.authHeader };
  }

  addToCart(item: Omit<CartItem, 'quantity'>) {
    const currentItems = this.cartItems.value;
    const existing = currentItems.find(
      i => i.computerId === item.computerId &&
           JSON.stringify(i.components) === JSON.stringify(item.components)
    );

    if (existing) {
      existing.quantity += 1;
    } else {
      currentItems.push({ ...item, quantity: 1 });
    }

    this.cartItems.next([...currentItems]);
    this.saveLocalCart();

    // Sync with backend if logged in
    if (this.authService.isAuthenticated) {
      currentItems.forEach(ci => {
        this.http.post<CartItem>(`${this.API_URL}`, ci, this.authHeaders())
          .subscribe();
      });
    }
  }

  updateQuantity(itemId: number, quantity: number) {
    const currentItems = this.cartItems.value;
    const item = currentItems.find(i => i.id === itemId);
    if (!item) return;

    if (quantity < 1) {
      this.removeFromCart(itemId);
      return;
    }

    item.quantity = quantity;
    this.cartItems.next([...currentItems]);
    this.saveLocalCart();

    if (this.authService.isAuthenticated) {
      this.http.post<CartItem>(`${this.API_URL}`, item, this.authHeaders()).subscribe();
    }
  }

  removeFromCart(itemId: number) {
    const currentItems = this.cartItems.value.filter(i => i.id !== itemId);
    this.cartItems.next(currentItems);
    this.saveLocalCart();

    if (this.authService.isAuthenticated) {
      this.http.delete(`${this.API_URL}/${itemId}`, this.authHeaders()).subscribe();
    }
  }

  clearCart() {
    this.cartItems.next([]);
    localStorage.removeItem('cart');

    if (this.authService.isAuthenticated) {
      this.http.delete(`${this.API_URL}`, this.authHeaders()).subscribe();
    }
  }

  getTotalPrice(): number {
    return this.cartItems.value.reduce((total, item) => {
      const compTotal = item.components.reduce((sum, c) => sum + c.price, 0);
      return total + (item.computerPrice + compTotal) * item.quantity;
    }, 0);
  }

  checkout(): Observable<any> {
    if (!this.authService.isAuthenticated) {
      throw new Error('User not authenticated');
    }

    const payload = this.cartItems.value.map(item => ({
      computerId: item.computerId,
      quantity: item.quantity,
      components: item.components.map(c => ({ id: c.id }))
    }));

    return this.http.post(`${this.API_URL}`, payload, this.authHeaders()).pipe(
      tap(() => this.clearCart())
    );
  }
}
