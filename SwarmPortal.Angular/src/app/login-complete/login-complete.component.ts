import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-login-complete',
  templateUrl: './login-complete.component.html',
  styleUrls: ['./login-complete.component.scss']
})
export class LoginCompleteComponent implements OnInit {

  constructor(private router: Router, private http: HttpService) { }

  async ngOnInit(): Promise<void> {
    setTimeout(() => {
      this.router.navigate(['']);
    }, 1000);
    await this.http.processLogin();
  }

}
