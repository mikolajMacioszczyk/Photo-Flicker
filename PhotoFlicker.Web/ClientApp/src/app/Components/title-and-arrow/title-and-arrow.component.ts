import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ITag} from "../../Models/Tag";

@Component({
  selector: 'app-title-and-arrow',
  templateUrl: './title-and-arrow.component.html',
  styleUrls: ['./title-and-arrow.component.css']
})
export class TitleAndArrowComponent implements OnInit {
  @Input('tag') tag: ITag;
  @Output('delete') delete = new EventEmitter();
  @Output('edit') edit = new EventEmitter();
  @Output('showImages') showImages = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  emitEmit() {
    this.edit.emit(this.tag);
  }

  deleteEmit() {
    this.delete.emit(this.tag);
  }

  showImagesEmit() {
    this.showImages.emit(this.tag);
  }
}
