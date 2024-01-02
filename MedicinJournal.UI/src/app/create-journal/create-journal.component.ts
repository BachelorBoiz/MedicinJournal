import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PatientService } from '../shared/patient.service';
import { JournalService } from '../shared/journal.service';

@Component({
  selector: 'app-create-journal',
  templateUrl: './create-journal.component.html',
  styleUrls: ['./create-journal.component.css'],
})
export class CreateJournalComponent {
  patientId!: number;
  title: string | undefined;
  description: string | undefined;

  constructor(
    private route: ActivatedRoute,
    private patientService: PatientService,
    private journalService: JournalService) {
      const patientIdString = this.route.snapshot.paramMap.get('id');
      this.patientId = patientIdString ? +patientIdString : 0;
  }

  createJournal(): void {
    
  }
}
