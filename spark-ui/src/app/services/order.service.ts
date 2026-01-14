import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

export interface Order {
  id: number;
  totalPrice: number;
  orderDate: string;
  computer: {
    id: number;
    name: string;
    price: number;
    imageUrl: string;
  };
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
export class OrderService {
  private readonly API_URL = 'http://localhost:5097/api/orders';

  constructor(private http: HttpClient, private authService: AuthService) {}

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.API_URL, {
      headers: this.authService.authHeader
    });
  }

  getOrder(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.API_URL}/${id}`, {
      headers: this.authService.authHeader
    });
  }

  createOrder(computerId: number, componentIds: number[]): Observable<Order> {
    return this.http.post<Order>(
      this.API_URL,
      { computerId, componentIds },
      { headers: this.authService.authHeader }
    );
  }
}