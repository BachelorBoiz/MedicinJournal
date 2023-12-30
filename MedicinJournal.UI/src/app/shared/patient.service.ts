import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Patient } from './models/patient';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  constructor(private _http: HttpClient) { }

  getPatientById(id: number): Observable<Patient> {
    return this._http.get<Patient>('http://localhost:9000/api/Patients/' + id)
  }
}
