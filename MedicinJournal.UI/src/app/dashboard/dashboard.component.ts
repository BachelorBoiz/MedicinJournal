import { Component, OnInit } from '@angular/core';
import { Patient } from '../shared/models/patient';
import { Employee } from '../shared/models/employee';
import { Observable } from 'rxjs';
import { EmployeeService } from '../shared/employee.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  patients: Patient[] = [];
  employee: Employee | undefined

  constructor(private _employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.getEmployeeInfo()
  }

  getEmployeeInfo() {
    this._employeeService.getEmployeeInfo().subscribe(value => {
      this.employee = value
      value.patients.forEach(p => {
        this.patients.push(p)
      })
    })
  }
}
