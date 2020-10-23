import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {uniqueTagName} from "../Common/Validators/uniqueTagValidator";
import {TagService} from "../Services/tag.service";

@Component({
  selector: 'app-new-tag',
  templateUrl: './new-tag.component.html',
  styleUrls: ['./new-tag.component.css']
})
export class NewTagComponent implements OnInit {
  form = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(50)], uniqueTagName(this.service))
  })

  get name(){
    if (this.form){
      return this.form.get('name');
    }
    else return "";
  }

  constructor(private service: TagService) { }

  ngOnInit() {
  }

  discard() {

  }

  submit() {

  }
}
