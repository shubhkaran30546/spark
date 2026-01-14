// src/app/pages/computer-details/computer-details.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ComputerService } from '../../services/computer.service';
import { CartService, CartItem } from '../../services/cart.service';
import { Computer, Component as ComputerComponent } from '../../models/Computer';
import { Location } from '@angular/common';

@Component({
  selector: 'app-computer-details',
  standalone: true,
  imports: [CommonModule, RouterModule, CurrencyPipe],
  templateUrl: './computer-details.html',
  styleUrls: ['./computer-details.css']
})
export class ComputerDetails implements OnInit {
  computer: Computer | null = null;
  isLoading = true;
  error: string | null = null;
  selectedComponents: ComputerComponent[] = [];

  constructor(
    private route: ActivatedRoute,
    private computerService: ComputerService,
    private cartService: CartService,
    private location: Location
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadComputer(+id);
    }
  }

  loadComputer(id: number): void {
    this.isLoading = true;
    this.computerService.getComputerById(id).subscribe({
      next: (computer) => {
        this.computer = computer;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load computer details.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  toggleComponent(component: ComputerComponent): void {
    const index = this.selectedComponents.findIndex(c => c.id === component.id);
    if (index === -1) {
      this.selectedComponents.push(component);
    } else {
      this.selectedComponents.splice(index, 1);
    }
  }

  isComponentSelected(component: ComputerComponent): boolean {
    return this.selectedComponents.some(c => c.id === component.id);
  }

  addToCart(computer: Computer): void {
    if (!computer) return;

    const cartItem: Omit<CartItem, 'quantity'> = {
      computerId: computer.id,
      computerName: computer.name,
      computerPrice: computer.price,
      computerImageUrl: computer.imageUrl || 'assets/placeholder-computer.jpg',
      components: this.selectedComponents.map(c => ({
        id: c.id,
        name: c.name,
        price: c.price,
        type: c.type
      }))
    };

    this.cartService.addToCart(cartItem);
  }

  goBack(): void {
    this.location.back();
  }
}