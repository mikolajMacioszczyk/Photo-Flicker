import {Component, OnDestroy, OnInit} from '@angular/core';
import {PhotoService} from "../../Services/photo.service";
import {IPhoto} from "../../Models/Photo";
import {Subscription} from "rxjs";
import {TagService} from "../../Services/tag.service";
import {ITag} from "../../Models/Tag";
import {flatMap} from "rxjs/operators";
import {MatDialog} from "@angular/material/dialog";
import {DetailsPhotoComponent} from "../details-photo/details-photo.component";

@Component({
  selector: 'app-all-photos',
  templateUrl: './all-photos.component.html',
  styleUrls: ['./all-photos.component.css']
})
export class AllPhotosComponent implements OnInit, OnDestroy {
  photos: IPhoto[];
  recommendedTags: string[];
  message: string;
  tag = "Wszystkie:";
  private pageSize = 100;
  private recommendedSize = 5;

  private isTagExistSubscription = new Subscription();
  private randomTagNamesSubscription = new Subscription();
  private takeFromServiceSubscription = new Subscription();
  private loadWithTagSubscription = new Subscription();

  constructor(private photoService: PhotoService,
              private tagService: TagService,
              public dialog: MatDialog) { }

  ngOnInit() {
    this.loadFullList();
    this.draw();
  }

  ngOnDestroy(): void {
    this.isTagExistSubscription.unsubscribe();
    this.randomTagNamesSubscription.unsubscribe();
    this.loadWithTagSubscription.unsubscribe();
    this.takeFromServiceSubscription.unsubscribe();
  }

  search(tag: string) {
    this.isTagExistSubscription.unsubscribe();

    this.isTagExistSubscription = this.tagService.isTagExists(tag).subscribe( isExist =>
    {
      if (isExist){
        this.loadWithTag(tag);
      }
      else {
        this.message = "Nie ma takiego tagu";
        this.tag = "Wszystkie";
      }
    }
    );
  }

  loadWithTag(tag: string){
    this.loadWithTagSubscription.unsubscribe();

    this.loadWithTagSubscription =
    this.tagService.getByName(tag)
      .pipe(
        flatMap((tagFromDb: ITag) => this.photoService.takeWhereTag(tagFromDb.id, this.pageSize))
      )
      .subscribe(photos =>{
        this.photos = photos;
        this.tag = tag;
    });
  }

  draw() {
    this.randomTagNamesSubscription.unsubscribe();

    this.randomTagNamesSubscription = this.tagService
      .getRandomTagNames(this.recommendedSize).subscribe(tags =>{
        this.recommendedTags = tags;
      });
  }

  loadFullList() {
    this.takeFromServiceSubscription.unsubscribe();

    this.takeFromServiceSubscription =
      this.photoService.take(this.pageSize).subscribe(photos => {
        this.photos = photos;
      });
  }

  openDialog(photo: IPhoto) {
    this.dialog.open(DetailsPhotoComponent, {data: photo})
  }
}
