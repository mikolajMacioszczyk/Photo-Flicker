import {Component, OnDestroy} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {TagService} from "../../Services/tag.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnDestroy{
  tag: string;
  time: number = 30;
  message: string;
  withDescription: boolean = true;
  form = new FormGroup({
    searchTag: new FormControl('', [Validators.required, Validators.maxLength(50)])
  });

  private subscription = new Subscription();

  constructor(private router: Router, private service: TagService) {
  }

  onSubmit(){
    this.subscription.unsubscribe();

    this.subscription =
      this.service.isTagExists(this.tag).subscribe((isExist: boolean) => {
        if (isExist){
          this.router.navigate(['slider', this.tag, this.time*1000, this.withDescription ? 1 : 0]);
        }
        else {
          this.message = "Nie mogę znaleźć tagu o nazwie \""+ this.tag+ "\"";
          this.tag = "";
        }
      });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
