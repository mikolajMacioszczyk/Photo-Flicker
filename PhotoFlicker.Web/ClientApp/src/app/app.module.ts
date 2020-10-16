import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import {PhotoService} from "./Services/photo.service";
import {TagService} from "./Services/tag.service";
import { PhotoSliderComponent } from './photo-slider/photo-slider.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AllPhotosComponent } from './all-photos/all-photos.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PhotoSliderComponent,
    NotFoundComponent,
    AllPhotosComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent },
      { path: 'slider/:tag/:time', component: PhotoSliderComponent },
      { path: 'photos', component: AllPhotosComponent },
      { path: '**', component: NotFoundComponent },
    ])
  ],
  providers: [PhotoService, TagService],
  bootstrap: [AppComponent]
})
export class AppModule { }
