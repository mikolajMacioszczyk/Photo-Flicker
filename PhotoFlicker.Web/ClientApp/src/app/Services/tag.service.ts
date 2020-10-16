import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Tag} from "@angular/compiler/src/i18n/serializers/xml_helper";

@Injectable({
  providedIn: 'root'
})
export class TagService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  Take(amount: number): Observable<any>{
    return this.http.get<Tag>(this.baseUrl+"api/tag/"+amount);
  }

  TakeWherePhoto(photoId: number, amount: number): Observable<any>{
    return this.http.get<Tag>(this.baseUrl+"api/tag/"+photoId+"/"+amount);
  }

  getByName(name: string): Observable<any>{
    return this.http.get<Tag>(this.baseUrl+"api/tag/single/"+name);
  }

  isTagExists(name: string): Observable<boolean>{
    return this.http.get<boolean>(this.baseUrl+"api/tag/canFind/"+name);
  }
}
