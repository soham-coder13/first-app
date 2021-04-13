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

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  getItems() {
    this.showHome = false;
    this.http.get<IItem[]>(this.baseUrl + 'api/Search?searchItem=' + this.itemToSearch).subscribe(result => {
      this.items = result;
    }, error => console.error(error));
  }
}
