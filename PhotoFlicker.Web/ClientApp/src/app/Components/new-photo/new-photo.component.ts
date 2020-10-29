import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {photoUrlExistValidator} from "../../Common/Validators/photUrlExistValidator";

@Component({
  selector: 'app-new-photo',
  templateUrl: './new-photo.component.html',
  styleUrls: ['./new-photo.component.css']
})
export class NewPhotoComponent implements OnInit {
  url: string;
  form = new FormGroup({
    photoUrl: new FormControl('', [Validators.required], [photoUrlExistValidator()]),
  })

  constructor() { }

  get photoUrl(){
    return this.form.get('photoUrl');
  }

  ngOnInit() {
  }

  submit() {

  }
}


