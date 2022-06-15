import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-complete',
  templateUrl: './login-complete.component.html',
  styleUrls: ['./login-complete.component.scss']
})
export class LoginCompleteComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
    setTimeout(() => {
      this.router.navigate(['']);
    }, 1000);
  }

}
