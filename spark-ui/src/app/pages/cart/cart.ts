// src/app/pages/cart/cart.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { CartService, CartItem } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule, CurrencyPipe],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css']
})
export class Cart implements OnInit {
  cartItems: CartItem[] = [];
  isLoading = true;
  error: string | null = null;

  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.cartService.cartItems$.subscribe({
      next: (items) => {
        this.cartItems = items;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load cart. Please try again.';
        this.isLoading = false;
        console.error('Error loading cart:', err);
      }
    });
  }

  updateQuantity(item: CartItem, quantity: number): void {
    if (item.id !== undefined) {
      this.cartService.updateQuantity(item.id, quantity);
    }
  }

  removeItem(itemId: number): void {
    this.cartService.removeFromCart(itemId);
  }

  clearCart(): void {
    this.cartService.clearCart();
  }

  getTotalPrice(): number {
    return this.cartService.getTotalPrice();
  }

  checkout(): void {
    if (!this.authService.isAuthenticated) {
      this.router.navigate(['/login'], { 
        queryParams: { returnUrl: '/checkout' } 
      });
      return;
    }
    
    // Here you would typically navigate to a checkout page
    // For now, we'll just clear the cart
    this.cartService.clearCart();
    this.router.navigate(['/orders']);
  }
}