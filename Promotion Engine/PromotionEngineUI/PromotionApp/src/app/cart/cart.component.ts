import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {

  message : string = "";

  constructor() { }

  ngOnInit(): void {
  }

  recieveMessage($event){
    this.message=$event;
  }

}
