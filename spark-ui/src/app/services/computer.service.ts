import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Computer } from '../models/Computer';

@Injectable({
  providedIn: 'root'
})
export class ComputerService {
  // Use HTTP on port 5097 (backend launchSettings.json exposes http://localhost:5097)
  private apiUrl = 'http://localhost:5097/api/computers'; // adjust port if needed

  constructor(private http: HttpClient) {}

  getComputers(): Observable<Computer[]> {
    return this.http.get<Computer[]>(this.apiUrl);
  }
  getComputerById(id: number): Observable<Computer> {
    return this.http.get<Computer>(`${this.apiUrl}/${id}`);
  }
}
