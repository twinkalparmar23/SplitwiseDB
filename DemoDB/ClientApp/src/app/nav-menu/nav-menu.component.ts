import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { User, UserEditModel } from '../Model/User';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  constructor(private _appService: AppService, private route: ActivatedRoute) { }

  @Input() UserId: number;
  friends: UserEditModel[];
  groups: any[];
  

  ngOnInit() {

    //this.UserId = this._appService.getUserId();
    //console.log(this.UserId);
    //this.UserId = this.route.snapshot.params['id'];
   // console.log(this.UserId);
    //this._appService.setUserId(this.UserId);
    this._appService.getAllFriends(this.UserId).subscribe((data: any) => {
      this.friends = data;
    });

    
    this._appService.getAllGroups(this.UserId).subscribe((data: any) => {
      this.groups = data;
    });
    
  }

  
}
