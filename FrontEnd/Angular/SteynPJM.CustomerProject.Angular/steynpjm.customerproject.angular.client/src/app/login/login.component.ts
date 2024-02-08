import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';


interface LoginResponse {
  token: string;
  firstName: string;
  lastName: string;
  title: string;
  designation: string;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})


export class LoginComponent {

  constructor(private http: HttpClient, private router: Router) { }

  login(userName: string, password: string) {
    const payload = {
      userName: userName,
      password: password
    };
    this.http.post<LoginResponse>('https://localhost:7107/system/login', payload).subscribe(
      data => {
        // Handle response here
        localStorage.setItem('token', data.token);
        this.router.navigate(['/main']);
      },
      error => {
        // Handle error here
      }
    );
  }
}

