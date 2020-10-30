import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Tag} from "@angular/compiler/src/i18n/serializers/xml_helper";
import {ITag} from "../Models/Tag";

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private readonly baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl+"api/tag/";
  }

  take(amount: number): Observable<any>{
    return this.http.get<Tag>(this.baseUrl+amount);
  }

  getRandomTagNames(amount: number): Observable<any>{
    return this.http.get<Tag>(this.baseUrl+"randomNames/"+amount);
  }

  takeWherePhoto(photoId: number, amount: number): Observable<any>{
    return this.http.get<Tag>(this.baseUrl+photoId+"/"+amount);
  }

  getByName(name: string): Observable<any>{
    return this.http.get<Tag>(this.baseUrl+"single/"+name);
  }

  isTagExists(name: string): Observable<boolean>{
    return this.http.get<boolean>(this.baseUrl+"canFind/"+name);
  }

  isTagUnique(name: string): Observable<boolean>{
    return this.http.get<boolean>(this.baseUrl+"isUnique/"+name);
  }

  createTag(tag: ITag): Observable<any>{
    return this.http.post(this.baseUrl+"create", tag);
  }

  deleteTag(id: number): Observable<any>{
    return this.http.delete(this.baseUrl+"delete/"+id);
  }
}
