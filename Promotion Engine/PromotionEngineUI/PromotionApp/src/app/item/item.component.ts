import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss']
})
export class ItemComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  selectedCount : number = 0;
  totalShow : boolean = false;
  hideCoupons : boolean = false;
  couponSelected : string = "";
  results : any;
  linkUrl : string = "";
  totalAmount : string = "";
  search : string = "";
  itemListCount : number = 1;

  @Input() set searchItem(value : string){
    this.search = value;
    this.changeItemList();
  }

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

  checkedOut(){
    this.buildUrl();
    this.http
    .get(this.linkUrl)
    .subscribe(data => {
      this.results = data;
      this.totalAmount = "Total amount to be paid : " + this.results.totalAmount + " only.";
      this.totalShow=true;
      this.hideCoupons=true;
    },
    error => {
      this.totalAmount = "Error encountered! Please try again...";
      this.totalShow=true;
      console.log(error);
    });
  }

  buildUrl(){
    this.linkUrl="http://localhost:60772/marketAPI/Promotion?";
    for(var item of this.marketItems){
      this.linkUrl+=item.item_code+"="+item.item_count+"&";
    }
    this.linkUrl+="promotion="+this.couponSelected;
  }

  itemAdded(){
    this.selectedCount=0;
    for(var mktItem of this.marketItems){
      this.selectedCount+=mktItem.item_count
    }
  }

  changeItemList(){
    this.itemListCount = 0;

    for(var mktItem of this.marketItems){
      if(!mktItem.item_name.toLowerCase().includes(this.search.toLocaleLowerCase())){
        mktItem.item_show=false;
      }
      else{
        mktItem.item_show=true;
        this.itemListCount++;
      }
    }
  }
}
