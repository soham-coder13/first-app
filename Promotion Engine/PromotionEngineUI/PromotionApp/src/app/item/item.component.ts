import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../Services/mock-data.service';
import { ApiService } from '../Services/api-service.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss']
})

export class ItemComponent implements OnInit {

  selectedCount : number = 0;
  totalShow : boolean = false;
  hideCoupons : boolean = false;
  couponSelected : string = "";
  results : any;
  linkUrl : string = "";
  totalAmount : string = "";
  search : string = "";
  itemListCount : number = 1;
  marketItems : Array<any>;
  coupons : Array<any>;
  loading : boolean = false;

  constructor(private itemData: DataService, private apiData: ApiService) {
    this.marketItems = this.itemData.getMarketItems();
    this.coupons = this.itemData.getCoupons();
    this.itemListCount = this.marketItems.length;
   }

  ngOnInit(): void { }

  @Input() set searchItem(value : string){
    this.search = value;
    this.changeItemList();
  }

  async checkedOut(){
    this.totalShow = false;
    this.loading = true;
    this.buildUrl();
    this.results = await this.apiData.callApi(this.linkUrl);

    if(this.results === undefined){
      this.loading = false;
      this.totalAmount = "Error encountered!! Please try later...";
      this.totalShow = true;
    }
    else{
      this.loading = false;
      this.totalAmount = "Total amount to be paid : " + this.results.totalAmount + " only";
      this.totalShow=true;
      this.hideCoupons=true;
    }
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
    if(this.search.length>0){
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
}
