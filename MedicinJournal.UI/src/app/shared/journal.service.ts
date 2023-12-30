import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Journal } from './models/journal';

@Injectable({
  providedIn: 'root'
})
export class JournalService {

  constructor(private _http: HttpClient) { }

  getJournals(): Observable<Journal[]> {
    return this._http.get<Journal[]>('http://localhost:9000/api/Journals/patientJournals')
  }
}
