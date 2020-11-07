import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor() { }

  public marketItems : Array<any> = [
    {
      item_image : "../../assets/images/A.PNG",
      item_name : "Item-A",
      item_code : "item_a",
      item_price : 50.00,
      item_count : 0,
      item_show : true
    },
    {
      item_image : "../../assets/images/B.PNG",
      item_name : "Item-B",
      item_code : "item_b",
      item_price : 30.00,
      item_count : 0,
      item_show : true
    },
    {
      item_image : "../../assets/images/C.PNG",
      item_name : "Item-C",
      item_code : "item_c",
      item_price : 20.00,
      item_count : 0,
      item_show : true
    },
    {
      item_image : "../../assets/images/D.PNG",
      item_name : "Item-D",
      item_code : "item_d",
      item_price : 15.00,
      item_count : 0,
      item_show : true
    }
  ];

  public coupons : Array<any> = [
    {
      selected : false,
      code : "Apply-A",
      desc : "Buy 3 Item-A for 130"
    },
    {
      selected : false,
      code : "Apply-B",
      desc : "Buy 2 Item-B for 45"
    },
    {
      selected : false,
      code : "Apply-C_D",
      desc : "Buy Item-C and Item-D for 30"
    }
  ]

  getMarketItems(){
    return this.marketItems;
  }

  getCoupons(){
    return this.coupons;
  }
}
