import { Component } from '@angular/core';
import { AuthService } from '../shared/auth.service';
import { LoginDto } from '../shared/login.dto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  jwt: string | null | undefined;
  constructor(private _auth: AuthService,
    private router: Router) {
    _auth.isLoggedIn$.subscribe(jwt => {
      this.jwt = jwt;
    })
  }

  login(password: string, userName: string){
    const loginData: LoginDto = {
      userName: userName,
      password: password
    };
    this._auth.login(loginData)
      .subscribe(token => {
        if(token && token.jwt){
          console.log('Token: ', token);
          this._auth.getUserRole().subscribe(role => {
            if(role.name === "Doctor") {
              this.router.navigate(['/dashboard'])
            } else if(role.name === "Patient") {
              this.router.navigate(['/patient-dashboard'])
            }
          })          
        }
      });
  }
}
