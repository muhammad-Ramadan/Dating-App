import { Component, OnInit } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { User } from 'src/_models/User';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  //loggedIn: boolean;
  //currentUser$: Observable<User>;
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    //this.getCurrentUser();
    //this.currentUser$ = this.accountService.currentUser$;
  }
  
  login(){
    this.accountService.login(this.model).subscribe(Response =>{
      console.log(Response);
      //this.loggedIn = true;
    }, error =>{
      console.log(error);
    })
  }

  logout(){
    this.accountService.logout();
    //this.loggedIn == false;
  }

  //replace loggedIn by currentUser$
  /*getCurrentUser(){
    
    return this.accountService.currentUser$.subscribe(user => {
      this.loggedIn = !!user;
    }, error => {
      console.log(error);
    }
    );
  }*/

}