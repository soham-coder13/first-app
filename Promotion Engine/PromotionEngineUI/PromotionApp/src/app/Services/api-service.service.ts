import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AmountModel } from '../Models/amount-model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  async callApi(link) {
    try{
      let resp = await this.http
        .get(link)
        .toPromise();

      return resp;
    }
    catch(err){
      console.log(err);
    }
  }
}
