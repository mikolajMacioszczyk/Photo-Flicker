import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {photoUrlExistValidator} from "../../Common/Validators/photUrlExistValidator";
import {PhotoService} from "../../Services/photo.service";
import {Router} from "@angular/router";
import {Observable, Subscription} from "rxjs";
import {ValidationTagOutput} from "../../Models/validationTagOutput";
import {ITag} from "../../Models/Tag";

@Component({
  selector: 'app-new-photo',
  templateUrl: './new-photo.component.html',
  styleUrls: ['./new-photo.component.css']
})
export class NewPhotoComponent implements OnDestroy {
  url: string;
  tags: string;
  invalidTags: string[];
  form = new FormGroup({
    photoUrl: new FormControl('', [Validators.required], [photoUrlExistValidator()]),
  })
  private subscription = new Subscription();

  constructor(private service: PhotoService, private router: Router) { }

  get photoUrl(){
    return this.form.get('photoUrl');
  }

  submit() {
    this.subscription.add(
      this.validate().subscribe((res: ValidationTagOutput) => {
        if (res.isValid){
          this.addPhotoToDatabase();
        }
        else {
          this.invalidTags = res.noValid;
        }
      })
    );
  }

  private addPhotoToDatabase(){
    this.subscription.add(
    this.service.createPhoto({url: this.url, content: this.tags})
      .subscribe(res => {
        this.router.navigate(['photos'])
      }, error => {console.log(error)})
    );
  }

  private validate(): Observable<any>{
    return this.service.validateTagsAsPlainText(this.tags);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}


