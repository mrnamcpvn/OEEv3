import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { isConstructorDeclaration } from 'typescript';
import { AuthService } from '../_core/_services/auth.service';
import { AlertifyService } from '../_core/_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  public constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}
  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }
    this.router.navigate(['login']);
    return false;
  }
}