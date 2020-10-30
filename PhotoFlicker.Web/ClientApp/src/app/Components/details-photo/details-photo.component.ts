import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {IPhoto} from "../../Models/Photo";

@Component({
  selector: 'app-details-photo',
  templateUrl: './details-photo.component.html',
  styleUrls: ['./details-photo.component.css']
})
export class DetailsPhotoComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: IPhoto) { }

  ngOnInit() {
  }

}
