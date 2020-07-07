import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

loggedIn()
{
  const token = localStorage.getItem('token');
  return !!token;
}
logout()
{
  const token = localStorage.removeItem('token');
}
  login(model: any) {
    return this.http.post(this.baseUrl + 'auth', model)
    .pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
        }
      })
    );
  }

}
