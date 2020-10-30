import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { ReactiveFormsModule } from '@angular/forms';


import { AppComponent } from './app.component';
import {PhotoService} from "./Services/photo.service";
import {TagService} from "./Services/tag.service";

import {TextFieldModule} from '@angular/cdk/text-field';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatDialogModule} from '@angular/material/dialog';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {HomeComponent} from "./Components/home/home.component";
import {AllPhotosComponent} from "./Components/all-photos/all-photos.component";
import {NavMenuComponent} from "./Components/nav-menu/nav-menu.component";
import {NewPhotoComponent} from "./Components/new-photo/new-photo.component";
import {NewTagComponent} from "./Components/new-tag/new-tag.component";
import {NotFoundComponent} from "./Components/not-found/not-found.component";
import {PhotoSliderComponent} from "./Components/photo-slider/photo-slider.component";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import { DetailsPhotoComponent } from './Components/details-photo/details-photo.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PhotoSliderComponent,
    NotFoundComponent,
    AllPhotosComponent,
    NewPhotoComponent,
    NewTagComponent,
    DetailsPhotoComponent,
  ],
  imports: [
      BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      RouterModule.forRoot([
          {path: '', component: HomeComponent},
          {path: 'slider/:tag/:time/:withDescription', component: PhotoSliderComponent},
          {path: 'photos', component: AllPhotosComponent},
          {path: 'photos/new', component: NewPhotoComponent},
          {path: 'tags/new', component: NewTagComponent},
          {path: '**', component: NotFoundComponent},
      ]),
      BrowserAnimationsModule,
      TextFieldModule,
      MatFormFieldModule,
      MatInputModule,
      MatCheckboxModule,
      MatButtonModule,
      MatDialogModule
    ],
  providers: [PhotoService, TagService],
  entryComponents: [DetailsPhotoComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
