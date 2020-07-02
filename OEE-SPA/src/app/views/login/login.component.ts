import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../_core/_services/auth.service';
import { AlertifyService } from '../../_core/_services/alertify.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-dashboard',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.checkLogged();
  }
  checkLogged()
  {
    const token = localStorage.getItem('token');
    if (token != null)
    {
      this.router.navigate(['downtime-reasons']);
    }
  }
  login() {
    this.authService.login(this.model ).subscribe(next => {
      this.alertify.success('Login successfully!');

    }, error => {
      this.alertify.error('Wrong Username or Password');
    }, () => {
      this.router.navigate(['downtime-reasons']);
    });
   }
  //  loggedIn(){
  //    return this.authService.loggedIn();
  //  }
}
