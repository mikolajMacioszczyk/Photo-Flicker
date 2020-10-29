import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {uniqueTagName} from "../../Common/Validators/uniqueTagValidator";
import {TagService} from "../../Services/tag.service";
import {Subscription} from "rxjs";
import {ITag} from "../../Models/Tag";
import {IPhoto} from "../../Models/Photo";
import {Router} from "@angular/router";

@Component({
  selector: 'app-new-tag',
  templateUrl: './new-tag.component.html',
  styleUrls: ['./new-tag.component.css']
})
export class NewTagComponent implements OnInit {
  tagName: string;
  form = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(50)], uniqueTagName(this.service))
  })
  private subscription = new Subscription();

  get name(){
    if (this.form){
      return this.form.get('name');
    }
    else return "";
  }

  constructor(private service: TagService, private router: Router) { }

  ngOnInit() {
  }

  discard() {
    this.tagName = "";
  }

  submit() {
    this.subscription.add(
      this.service.createTag(new Tag(this.tagName))
        .subscribe(res => {
          if (res){
            this.tagName = "";
            this.router.navigate(['photos']);
          }
        }, error => {console.log(error)})
    )
  }
}

class Tag implements ITag
{
  id: number;
  name: string;
  markedPhotos: IPhoto[]

  constructor(name: string) {
    this.name = name;
  }
}
