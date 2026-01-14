// Update /Users/pranavmehra/Desktop/Web Development/karan_project/spark/spark-ui/src/app/pages/computer-list/computer-list.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComputerService } from '../../services/computer.service';
import { Computer } from '../../models/Computer';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-computer-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './computer-list.html',
  styleUrls: ['./computer-list.css']
})
export class ComputerList implements OnInit {
  computers: Computer[] = [];
  isLoading = true;
  error: string | null = null;

  constructor(private computerService: ComputerService) {}

  ngOnInit(): void {
    this.loadComputers();
  }

  public loadComputers(): void {
    this.isLoading = true;
    this.error = null;
    
    this.computerService.getComputers().subscribe({
      next: (data) => {
        this.computers = data;
        console.log('Complete computer data:', JSON.stringify(data, null, 2));
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading computers:', error);
        this.error = 'Failed to load computers. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  addToCart(computer: Computer): void {
    // TODO: Implement add to cart functionality
    console.log('Adding to cart:', computer);
    // This will be connected to CartService later
  }
}
