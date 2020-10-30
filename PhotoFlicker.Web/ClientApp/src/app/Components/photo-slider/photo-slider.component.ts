import {Component, OnDestroy, OnInit} from '@angular/core';
import {PhotoService} from "../../Services/photo.service";
import {ActivatedRoute, Router} from "@angular/router";
import {IPhoto} from "../../Models/Photo";
import {Observable, Subscription} from "rxjs";
import {ITag} from "../../Models/Tag";
import {TagService} from "../../Services/tag.service";
import {flatMap} from "rxjs/operators";

@Component({
  selector: 'app-photo-slider',
  templateUrl: './photo-slider.component.html',
  styleUrls: ['./photo-slider.component.css']
})
export class PhotoSliderComponent implements OnInit, OnDestroy {
  currentPhoto: IPhoto = {id: -1, path: "https://www.nycgirlinpearls.com/wp-content/uploads/2017/01/15fe8848c0da0cd2274cce9a0db04b34.jpg", tags: [], description: ""};
  isEnd: boolean = false;
  tag: string;
  private time: number;
  private photos: IPhoto[] = [];
  private interval: number;
  private subscription = new Subscription();
  private withDescription: boolean = true;

  constructor(private photoService: PhotoService,
              private tagService: TagService,
              private route: ActivatedRoute,
              private router: Router) {}

  ngOnInit() {
    this.subscription.add(
      this.loadParamsToTagService().pipe(
        flatMap((tag: ITag) => this.photoService.takeWhereTag(tag.id, 10))
      )
      .subscribe(res => {
        this.photos = res;
        this.display();
      })
    );
  }

  private loadParamsToTagService(): Observable<any>
  {
    return this.route.params.pipe(
      flatMap(params => {
        this.tag = params['tag'];
        this.time = params['time'];
        this.withDescription = params['withDescription'] != 0;
        return this.tagService.getByName(this.tag)
      })
    )
  }

  private display(): void{
    if (this.photos == null || this.photos[0] == null){
      return;
    }

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
    this.isEnd = false;
    this.display();
  }

  backHome() {
    this.router.navigate(['']);
  }
}
