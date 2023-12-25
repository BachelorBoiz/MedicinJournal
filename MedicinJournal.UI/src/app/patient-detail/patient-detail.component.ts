import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-patient-detail',
  templateUrl: './patient-detail.component.html',
  styleUrls: ['./patient-detail.component.css'],
})
export class PatientDetailComponent implements OnInit {
  patientName!: string;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.patientName = this.route.snapshot.paramMap.get('name')!;
  }
}