import {Component } from '@angular/core';
import { navItems } from '../../_nav';
import { AlertifyService } from '../../_core/_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})

export class DefaultLayoutComponent {
  public sidebarMinimized = false;
  public navItems = navItems;
  public username = '';
  constructor( private alertify : AlertifyService, private router: Router) { }

  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }
  logout() {
    localStorage.removeItem('token');
    this.alertify.success('logged out');
    this.router.navigate(['login']);
  }
}

