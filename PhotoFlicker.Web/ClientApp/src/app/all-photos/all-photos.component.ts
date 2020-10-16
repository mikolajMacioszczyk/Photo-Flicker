import {Component, OnDestroy, OnInit} from '@angular/core';
import {PhotoService} from "../Services/photo.service";
import {IPhoto} from "../Models/Photo";
import {Subscription} from "rxjs";
import {TagService} from "../Services/tag.service";

@Component({
  selector: 'app-all-photos',
  templateUrl: './all-photos.component.html',
  styleUrls: ['./all-photos.component.css']
})
export class AllPhotosComponent implements OnInit, OnDestroy {
  photos: IPhoto[];
  message: string;
  private subscription = new Subscription();
  private pageSize = 100;

  constructor(private photoService: PhotoService, private tagService: TagService) { }

  ngOnInit() {
    this.subscription.add(
    this.photoService.take(this.pageSize).subscribe(photos => {
      this.photos = photos;
    }));
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  search(tag: string) {
    this.subscription.unsubscribe();
    this.subscription.add(
    this.tagService.isTagExists(tag).subscribe( isExist =>
    {
      if (isExist){
        this.loadWithTag(tag);
      } else {
        this.message = "Nie ma takiego tagu";
      }
    }
    ));
  }

  private loadWithTag(tag: string){

  }
}
