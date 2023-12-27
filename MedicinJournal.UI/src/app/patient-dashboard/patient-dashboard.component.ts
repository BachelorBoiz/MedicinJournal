import { Component, OnInit } from '@angular/core';
import { Journal } from '../shared/models/journal';
import { JournalService } from '../shared/journal.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-patient-dashboard',
  templateUrl: './patient-dashboard.component.html',
  styleUrls: ['./patient-dashboard.component.css']
})
export class PatientDashboardComponent implements OnInit {
  journals: Journal[] = [];

  constructor(private _journalService: JournalService) { }

  ngOnInit(): void {
    this._journalService.getJournals().subscribe(value => {
      value.forEach(j => {
        this.journals.push(j)
      })
    })
  }
}
