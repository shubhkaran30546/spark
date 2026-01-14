import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ComputerService } from '../../services/computer.service';
import { Computer, Component as ComputerComponent } from '../../models/Computer';
import { Location } from '@angular/common';

@Component({
  selector: 'app-computer-details',
  standalone: true,
  imports: [CommonModule, RouterModule, CurrencyPipe],  // Add CurrencyPipe here
  templateUrl: './computer-details.html',
  styleUrls: ['./computer-details.css']
})
export class ComputerDetails implements OnInit {
  computer: Computer | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private computerService: ComputerService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.loadComputer();
  }

  loadComputer(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error = 'No computer ID provided';
      this.isLoading = false;
      return;
    }

    this.computerService.getComputerById(+id).subscribe({
      next: (data) => {
        this.computer = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading computer:', error);
        this.error = 'Failed to load computer details. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }

  getComponentType(components: ComputerComponent[] | undefined, type: string): ComputerComponent[] {
    return components?.filter(c => c.type.toLowerCase() === type.toLowerCase()) || [];
  }

  addToCart(computer: Computer): void {
    console.log('Adding to cart:', computer);
    // TODO: Implement add to cart functionality
  }
}