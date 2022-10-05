import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from 'src/_models/member';

//Replaced by jwt interceptor
//import { HttpClient, HttpHeaders } from '@angular/common/http';

//const httpOptions = {
//  headers : new HttpHeaders({
//    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
//  })
//};

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseURL= environment.apiURL;

  constructor(private http: HttpClient) { }

  //we can remove Observable<Member[]> from getMembers() : Observable<Member[]>
  //it will be the same :)
  getMembers() {
    //return this.http.get<Member[]>(this.baseURL + 'users', httpOptions);
    return this.http.get<Member[]>(this.baseURL + 'users');

  }

  //getMember(username: string) : Observable<Member> { => the next is the same :)
  getMember(username: string) : Observable<Member> {
    //return this.http.get<Member>(this.baseURL + 'users/' + username , httpOptions);
    return this.http.get<Member>(this.baseURL + 'users/' + username);

  }
}
