import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {IPhoto} from "../Models/Photo";
import {IUrlAndString} from "../Models/UrlAndString";

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl + "api/photo/";
  }

  take(amount: number): Observable<any>{
    return this.http.get<IPhoto>(this.baseUrl+amount);
  }

  takeWhereTag(tagId: number, amount: number): Observable<any>{
    return this.http.get<IPhoto>(this.baseUrl+tagId+"/"+amount);
  }

  createPhoto(created: IUrlAndString): Observable<any>{
    return this.http.post<IPhoto>(this.baseUrl+"create", created);
  }
}
