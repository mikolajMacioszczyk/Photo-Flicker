import {Component, OnDestroy, OnInit} from '@angular/core';
import {TagService} from "../../Services/tag.service";
import {ITag} from "../../Models/Tag";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-tag-list',
  templateUrl: './tag-list.component.html',
  styleUrls: ['./tag-list.component.css']
})
export class TagListComponent implements OnInit, OnDestroy {
  firstHalf: ITag[];
  secondHalf: ITag[];
  private static defaultPageSize = 100;

  private takeTagsSubscription;
  private deleteTagSubscription = new Subscription();

  constructor(private service: TagService) { }

  ngOnInit() {
    this.takeTagsSubscription =
      this.service.take(TagListComponent.defaultPageSize)
        .subscribe((tags: ITag[]) =>
        {
          this.firstHalf = tags.slice(0, tags.length/2)
          this.secondHalf = tags.slice(tags.length/2, tags.length)
        });
  }

  ngOnDestroy(): void {
    this.takeTagsSubscription.unsubscribe();
  }

  deleteTag(tag: ITag) {
    this.deleteTagSubscription.unsubscribe();

    this.deleteTagSubscription =
      this.service.deleteTag(tag.id).subscribe(res =>{
        if (res){
          this.removeFromLists(tag);
        }
      }, error => console.log(error));
  }

  private removeFromLists(tag: ITag){
    let idx = this.firstHalf.indexOf(tag);
    if (idx >= 0){
      this.firstHalf.splice(idx, 1);
    }
    else {
      idx = this.secondHalf.indexOf(tag);
      this.secondHalf.splice(idx, 1);
    }
  }
}
