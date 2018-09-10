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
  FriendName: string;
  FriendEmail: string;

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

  addFriend() {
    // console.log(this.FriendName + "  " + this.FriendEmail);
    this._appService.addFriend(this.UserId, this.FriendName, this.FriendEmail).subscribe((data: any) => {
      if (data.status == false) {
        alert("not valid user...");
      }
      else {
        alert("friend added..");
      }
    },
      err => {
        alert("Invalid User...please enter correct name or email...");
      }
    );
    this.FriendEmail = null;
    this.FriendName = null;
  }
  
}
