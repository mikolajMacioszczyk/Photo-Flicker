import {Component, OnDestroy, OnInit} from '@angular/core';
import {PhotoService} from "../Services/photo.service";
import {ActivatedRoute} from "@angular/router";
import {IPhoto} from "../Models/Photo";
import {Subscription} from "rxjs";
import {ITag} from "../Models/Tag";

@Component({
  selector: 'app-photo-slider',
  templateUrl: './photo-slider.component.html',
  styleUrls: ['./photo-slider.component.css']
})
export class PhotoSliderComponent implements OnInit, OnDestroy {
  currentPhoto: IPhoto = new Photo();
  private photos: IPhoto[] = [];
  private interval: number;
  private subscription = new Subscription();

  constructor(private service: PhotoService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.subscription.add(
      this.route.params.subscribe(params =>{
      this.readDataFromService(params['tag'], params['time'])
    }));
  }

  private readDataFromService(tag: string, time: number){
    this.subscription.add(this.service.takeWhereTag(3, 10).subscribe(res =>{
      this.photos = res;
      this.display(time);
    }));
  }

  private display(time: number): void{
    if (this.photos == null || this.photos[0] == null){return;}
    this.currentPhoto = this.photos[0];
    let idx = 1;
    this.interval = setInterval(() => {
      this.currentPhoto = this.photos[idx++];
      idx = idx % this.photos.length;
    }, time);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    clearInterval(this.interval);
  }

}

class Photo implements IPhoto{
  id: number;
  path: string= "https://yetem.pl/media/uploads/c365/empty.png";
  tags: ITag[];
}
