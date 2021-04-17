import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent {
  showHome = true;
  itemToSearch = "";
  public items: IItem[];
  http: HttpClient;
  baseUrl = "";
  count: number;
  showMoreItems: Boolean;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.count = 20;
    this.showMoreItems = true;
  }

  getItems() {
    this.showHome = false;
    this.http.get<IItem[]>(this.baseUrl + 'api/Search?searchItem=' + this.itemToSearch + '&itemCount=' + this.count).subscribe(result => {
      this.items = result;
      if (this.items.length < this.count) {
        this.showMoreItems = false;
      }
    }, error => console.error(error));
  }

  loadMore() {
    this.count += 20;
    this.getItems();

  }
}
