import {Component, OnDestroy} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {photoUrlExistValidator} from "../../Common/Validators/photUrlExistValidator";
import {PhotoService} from "../../Services/photo.service";
import {Router} from "@angular/router";
import {Observable, Subscription} from "rxjs";
import {ValidationTagOutput} from "../../Models/validationTagOutput";
import {TagService} from "../../Services/tag.service";
import {ITag} from "../../Models/Tag";

@Component({
  selector: 'app-new-photo',
  templateUrl: './new-photo.component.html',
  styleUrls: ['./new-photo.component.css']
})
export class NewPhotoComponent implements OnDestroy {
  url: string;
  tagsText: string;
  invalidTags: string[] = [];
  form = new FormGroup({
    photoUrl: new FormControl('', [Validators.required], [photoUrlExistValidator()]),
    tags: new FormControl()
  })
  private subscription = new Subscription();

  constructor(private photoService: PhotoService, private tagService: TagService ,private router: Router) { }

  get photoUrl(){
    return this.form.get('photoUrl');
  }

  get tags(){
    return this.form.get('tags');
  }

  submit() {
    this.invalidTags = [];
    this.subscription.add(
      this.validate().subscribe((res: ValidationTagOutput) => {
        if (res.isValid){
          this.addPhotoToDatabase();
        }
        else {
          this.invalidTags = res.invalidValues;
        }
      })
    );
  }

  private addPhotoToDatabase(){
    this.subscription.add(
    this.photoService.createPhoto({url: this.url, text: this.tagsText})
      .subscribe(res => {
        if (res) {
          this.router.navigate(['photos'])
        }
      }, error => {console.log(error)})
    );
  }

  private validate(): Observable<any>{
    return this.photoService.validateTagsAsPlainText(this.tagsText);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  removeFromTagText(tagName: string) {
    this.tagsText = this.tagsText.replace('#'+tagName, "");
  }

  addTag(tagName: string) {
    let created: ITag = {id: -1, name: tagName, markedPhotos: []};
    this.subscription.add(
      this.tagService.createTag(created).subscribe(isCreated => {
        if (isCreated){
          let idx = this.invalidTags.indexOf(tagName)
          this.invalidTags.splice(idx, 1);
        }
      }, error => console.log(error))
    )
  }
}
