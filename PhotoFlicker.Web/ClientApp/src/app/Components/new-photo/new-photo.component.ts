import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {photoUrlExistValidator} from "../../Common/Validators/photUrlExistValidator";
import {PhotoService} from "../../Services/photo.service";
import {Router} from "@angular/router";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-new-photo',
  templateUrl: './new-photo.component.html',
  styleUrls: ['./new-photo.component.css']
})
export class NewPhotoComponent implements OnDestroy {
  url: string;
  tags: string;
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
    this.service.createPhoto({url: this.url, content: this.tags})
      .subscribe(res => {
        this.router.navigate(['photos'])
      }, error => {console.log(error)})
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}


