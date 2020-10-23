import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ITag} from "../Models/Tag";
import {IPhoto} from "../Models/Photo";

@Component({
  selector: 'app-new-photo',
  templateUrl: './new-photo.component.html',
  styleUrls: ['./new-photo.component.css']
})
export class NewPhotoComponent implements OnInit {
  form = new FormGroup({
    path: new FormControl('', [Validators.required]),
  })

  constructor() { }

  ngOnInit() {
  }

}

class Photo implements IPhoto{
  id: number;
  path: string;
  tags: ITag[];

  constructor() {
    this.tags = [];
  }
}
