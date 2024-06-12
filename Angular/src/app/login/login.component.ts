import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  userName: string = '';
  password: string = '';
  apiUrl = 'http://localhost:5193/api/Users';
  constructor(private http: HttpClient, private router: Router) {}

  onSubmit() {
    const credentials = {
      userName: this.userName,
      password: this.password,
    };
    console.log(credentials);
    this.http.post<any>(`${this.apiUrl}/Login`, credentials).subscribe(
      (response) => {
        const token = response.token;
        localStorage.setItem('token', token);
        console.log('Login successful. Token:', token);
        this.router.navigate(['/todo']);
      },
      (error) => {
        console.error('Login error:', error);
      }
    );
  }
}
