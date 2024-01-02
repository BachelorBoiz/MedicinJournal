import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Patient } from '../shared/models/patient';
import { PatientService } from '../shared/patient.service';

@Component({
  selector: 'app-patient-detail',
  templateUrl: './patient-detail.component.html',
  styleUrls: ['./patient-detail.component.css'],
})
export class PatientDetailComponent implements OnInit {
  patientId!: number;
  patient: Patient | undefined

  constructor(private route: ActivatedRoute, private _patientService: PatientService, private router: Router) {}

  ngOnInit(): void {
    const patientIdString = this.route.snapshot.paramMap.get('id');
    this.patientId = patientIdString ? +patientIdString : 0; // Convert to number or set default value
    this.getPatientInfo();
  }

  getPatientInfo() {
    this._patientService.getPatientById(this.patientId).subscribe(value => {
      this.patient = value
    })
  }

  navigateToCreateJournal(): void {
    this.router.navigate(['/create-journal' + this.patientId])
  }
}