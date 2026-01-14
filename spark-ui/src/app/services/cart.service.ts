import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface CartItem {
  productId: number;
  quantity: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  private _cartItems = new BehaviorSubject<CartItem[]>([]);
  cartItems$ = this._cartItems.asObservable();

  getCartItems(): Observable<CartItem[]> {
    return this.cartItems$;
  }

  addItem(item: CartItem) {
    const items = [...this._cartItems.value];
    items.push(item);
    this._cartItems.next(items);
  }

  clear() {
    this._cartItems.next([]);
  }
}
