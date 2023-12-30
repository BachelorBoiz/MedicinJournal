import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee } from './models/employee';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private _http: HttpClient) { }

  getEmployeeInfo(): Observable<Employee> {
    return this._http.get<Employee>('http://localhost:9000/api/Employees/LoggedInEmployeeInfo')
  }
}
