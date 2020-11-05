import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  search : string = "";

  constructor() { }

  ngOnInit(): void {
  }

  @Output() messageEvent = new EventEmitter<string>();

  searchItem($event){
    this.messageEvent.emit($event);
  }

}
