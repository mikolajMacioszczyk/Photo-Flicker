import {Component, OnDestroy, OnInit} from '@angular/core';
import {PhotoService} from "../../Services/photo.service";
import {IPhoto} from "../../Models/Photo";
import {Subscription} from "rxjs";
import {TagService} from "../../Services/tag.service";
import {ITag} from "../../Models/Tag";

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
  private subscription = new Subscription();
  private pageSize = 100;
  private recommendedSize = 5;

  constructor(private photoService: PhotoService, private tagService: TagService) { }

  ngOnInit() {
    this.loadFullList();
    this.draw();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  search(tag: string) {
    this.subscription.add(
    this.tagService.isTagExists(tag).subscribe( isExist =>
    {
      if (isExist){
        this.loadWithTag(tag);
      }
      else {
        this.message = "Nie ma takiego tagu";
        this.tag = "Wszystkie";
      }
    }
    ));
  }

  loadWithTag(tag: string){
    this.subscription.add(
      this.tagService.getByName(tag).subscribe((tagFromDb: ITag) =>{
        let idx = tagFromDb.id;
        this.subscription.add(
          this.photoService.takeWhereTag(idx, this.pageSize).subscribe(photos => {
            this.photos = photos;
            this.tag = tag;
          })
        )
      })
    )
  }

  draw() {
    this.subscription.add(
      this.tagService.getRandomTagNames(this.recommendedSize).subscribe(tags =>{
        this.recommendedTags = tags;
      })
    )
  }

  loadFullList() {
    this.subscription.add(
      this.photoService.take(this.pageSize).subscribe(photos => {
        this.photos = photos;
      }));
  }
}
