import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComputerService } from '../../services/computer.service';
import { Computer } from '../../models/Computer';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule], // ✅ THIS FIXES ngFor
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  computers: Computer[] = [];

  constructor(private computerService: ComputerService) {}

  ngOnInit(): void {
    this.computerService.getComputers().subscribe(data => {
      this.computers = data.slice(0, 3); // 👈 first 3 items only
    });
  }
}
