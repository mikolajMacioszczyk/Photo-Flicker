import { Component } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  tag: string;
  form = new FormGroup({
    searchTag: new FormControl('', [Validators.required, Validators.maxLength(50)])
  });

  constructor(private router: Router) {
  }

  onSubmit(){
    this.router.navigate(['slider', this.tag, 5000]);
  }
}
