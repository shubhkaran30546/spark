import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { shareReplay } from 'rxjs/operators';
import { Computer } from '../models/Computer';

@Injectable({
  providedIn: 'root'
})
export class ComputerService {
  // Use HTTP on port 5097 (backend launchSettings.json exposes http://localhost:5097)
  private apiUrl = 'http://localhost:5097/api/computers'; // adjust port if needed

  constructor(private http: HttpClient) {}

  // cached observable so the list persists across navigations
  private computers$?: Observable<Computer[]>;

  getComputers(): Observable<Computer[]> {
    if (!this.computers$) {
      this.computers$ = this.http.get<Computer[]>(this.apiUrl).pipe(
        // cache the latest successful response for subscribers
        shareReplay({ bufferSize: 1, refCount: true })
      );
    }
    return this.computers$;
  }
  getComputerById(id: number): Observable<Computer> {
    return this.http.get<Computer>(`${this.apiUrl}/${id}`);
  }
}
