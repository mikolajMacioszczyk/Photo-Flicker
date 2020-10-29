import {Component, OnDestroy, OnInit} from '@angular/core';
import {PhotoService} from "../../Services/photo.service";
import {ActivatedRoute, Router} from "@angular/router";
import {IPhoto} from "../../Models/Photo";
import {Subscription} from "rxjs";
import {ITag} from "../../Models/Tag";
import {TagService} from "../../Services/tag.service";

@Component({
  selector: 'app-photo-slider',
  templateUrl: './photo-slider.component.html',
  styleUrls: ['./photo-slider.component.css']
})
export class PhotoSliderComponent implements OnInit, OnDestroy {
  currentPhoto: IPhoto = new Photo();
  isEnd: boolean = false;
  tag: string;
  private time: number;
  private photos: IPhoto[] = [];
  private interval: number;
  private subscription = new Subscription();

  constructor(private photoService: PhotoService,
              private tagService: TagService,
              private route: ActivatedRoute,
              private router: Router) {}

  ngOnInit() {
    this.subscription.add(
      this.route.params.subscribe(params =>{
        this.tag = params['tag'];
        this.time = params['time'];
      this.readDataFromService()
    }));
  }

  private readDataFromService(){
    this.subscription.add(
    this.tagService.getByName(this.tag).subscribe((tag: ITag) => {
      this.subscription.add(this.photoService.takeWhereTag(tag.id, 10).subscribe(res =>{
        this.photos = res;
        this.display();
      }));

    }))
  }

  private display(): void{
    if (this.photos == null || this.photos[0] == null){return;}
    this.currentPhoto = this.photos[0];
    let idx = 1;
    this.interval = setInterval(() => {
      if (idx >= this.photos.length){
        this.isEnd = true;
        clearInterval(this.interval);
      }
      this.currentPhoto = this.photos[idx++];
    }, this.time);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    clearInterval(this.interval);
  }

  tryAgain() {
    this.router.navigate(['slider',this.tag, this.time])
  }

  backHome() {
    this.router.navigate(['']);
  }
}

class Photo implements IPhoto{
  id: number;
  path: string= "https://yetem.pl/media/uploads/c365/empty.png";
  tags: ITag[];
}