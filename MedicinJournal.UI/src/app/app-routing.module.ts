import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { CreateUserComponent } from './auth/create-user/create-user.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PatientDetailComponent } from './patient-detail/patient-detail.component';
import { PatientDashboardComponent } from './patient-dashboard/patient-dashboard.component';
import { CreateJournalComponent } from './create-journal/create-journal.component';

const routes: Routes = [
  {path: '', redirectTo: '/login', pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {path: 'create-user', component: CreateUserComponent},
  {path: 'dashboard', component: DashboardComponent},
  {path: 'patient-dashboard', component: PatientDashboardComponent},
  {path: 'patient/:id', component: PatientDetailComponent},
  {path: 'create-journal/:id', component: CreateJournalComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
