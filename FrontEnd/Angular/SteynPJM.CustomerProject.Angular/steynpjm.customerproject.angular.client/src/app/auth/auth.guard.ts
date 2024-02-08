import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, public jwtHelper: JwtHelperService) { }

  canActivate(): boolean {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // If the token exists in local storage and it's not expired, navigate to the main page
      this.router.navigate(['/main']);
      return false;
    }
    // If the token doesn't exist or it's expired, allow navigation to the login page
    return true;
  }
}
