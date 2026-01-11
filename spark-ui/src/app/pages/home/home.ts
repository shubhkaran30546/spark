import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],   // ✅ THIS FIXES ngFor
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent {
  featuredComputers = [
    {
      id: 1,
      name: 'HP Pavilion',
      price: 999,
      image: '/public/hp_pav.webp'
    },
    {
      id: 2,
      name: 'Apple iMac',
      price: 1499,
      image: 'public/imac.jpeg'
    },
    {
      id: 3,
      name: 'Apple MacBook Air',
      price: 1999,
      image: 'public/mc_air.jpeg'
    }
  ];
}

