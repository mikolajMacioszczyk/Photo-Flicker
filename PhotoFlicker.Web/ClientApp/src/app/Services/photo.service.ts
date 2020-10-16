import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {IPhoto} from "../Models/Photo";

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  take(amount: number): Observable<any>{
    return this.http.get<IPhoto>(this.baseUrl+"api/photo/"+amount);
  }

  takeWhereTag(tagId: number, amount: number): Observable<any>{
    return this.http.get<IPhoto>(this.baseUrl+"api/photo/"+tagId+"/"+amount);
  }
}
