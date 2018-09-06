import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { User, UserEditModel } from '../Model/User';
import { AddBill, member } from '../Model/AddBill';
import { Settle, Balance } from '../Model/Settle';
import { Bill } from '../Model/Bill';

@Component({
  selector: 'app-friend',
  templateUrl: './friend.component.html',
  styleUrls: ['./friend.component.css']
})
export class FriendComponent implements OnInit {

  UserId: number;
  friendId: number;
  friend: UserEditModel;

  AddBillModel = new AddBill();
  members: any[] = [];
  billPayer = new member();
  billPayers: member[] = [];
  showMultiplePayer: boolean = false;
  totalAmount: number;
  sharedAmount: number;
  sharedMembers: any[] = [];
  settlements: Settle[] = [];

  IndividualBills: Bill;
  balance: Balance[] = [];
  TotalBalance= new member();

  constructor(private _appService: AppService, private route: ActivatedRoute) {

    this.friendId = this.route.snapshot.params['id'];
    this.route.parent.params.subscribe(params => {
      this.UserId = +params['id'];
    });

  }

  ngOnInit() {

    this._appService.getSingleFriendDetail(this.friendId).subscribe((data: any) => {
      this.friend = data;
      let x = { "id": this.friend.userId, "name": this.friend.userName };
      this.members.push(x);
      
    });

    this._appService.getSingleFriendDetail(this.UserId).subscribe((data: any) => {
      let x = { "id": data.userId, "name": data.userName };
      this.members.push(x);
      //console.log(this.members);
    });

    this._appService.getIndividualBills(this.UserId, this.friendId).subscribe((data: any) => {
      this.IndividualBills = data;
      
    });

    this._appService.getIndividualbalance(this.UserId, this.friendId).subscribe((data: any) => {
      this.balance = data;
      console.log(this.balance);
      this.TotalBalance.id = this.UserId;
      this.TotalBalance.amount = 0;
      
      for (var i = 0; i < this.balance.length; i++) {
       
        if (this.balance[i].payer_id === this.UserId) {
          if (this.balance[i].amount > 0) {
            this.TotalBalance.amount = this.TotalBalance.amount + this.balance[i].amount;
          }
          else {
            this.TotalBalance.amount = this.TotalBalance.amount - Math.abs(this.balance[i].amount);
          }
        }
        else {
          if (this.balance[i].amount > 0) {
            this.TotalBalance.amount = this.TotalBalance.amount - this.balance[i].amount;
          }
          else {
            this.TotalBalance.amount = this.TotalBalance.amount + Math.abs(this.balance[i].amount);
          }
        }
      }
      console.log(this.TotalBalance);
    });
  }

  showPayer() {
    document.getElementById("payers").innerText = "Multiple";
    if (this.showMultiplePayer == false) {
      this.showMultiplePayer = true;
    }
    else {
      this.showMultiplePayer = false;
    }
  }

  addSinglePayer(id: number) {
    this.billPayer = new member();
    this.billPayer.id = id;
    this.billPayer.amount = this.totalAmount;
    this.billPayers = [];

    let x = this.members.find(y => y.id === this.billPayer.id);
    document.getElementById("payers").innerText = x.name;
  }

  addMultiplePayer(id: number, amount: number) {
    let x = new member();
    x.id = id;
    x.amount = Number(amount);
    this.billPayers.push(x);
    this.billPayer = null;  
  }

  saveBill() {
    this.AddBillModel.creatorId = this.UserId;
    this.AddBillModel.createdDate = new Date().toLocaleString();
    this.sharedAmount = this.totalAmount / 2;

    if (this.billPayer != null) {
      this.AddBillModel.payer = [];
      this.AddBillModel.payer.push(this.billPayer);

      if (this.billPayer.id == this.UserId) {
        let x = new Settle();
        x.SharedMemberId = this.UserId;
        x.PayerId = this.friendId;
        x.TotalAmount = this.totalAmount - this.sharedAmount;
        this.settlements.push(x);
        
      }
      else {
        let x = new Settle();
        x.SharedMemberId = this.friendId;
        x.PayerId = this.UserId;
        x.TotalAmount = this.totalAmount - this.sharedAmount;
        this.settlements.push(x);
        //this.AddBillModel.creatorId = this.friendId;
      }

      this.AddBillModel.SettleModels = this.settlements;

    }
    else {
      this.AddBillModel.payer = [];
      this.AddBillModel.payer = this.billPayers;

      var x = this.billPayers.find(item => item.amount > this.sharedAmount);
      if (x != null) {
        var y = this.billPayers.find(item => item.id != x.id);
        let set = new Settle();
        set.SharedMemberId = x.id;
        set.PayerId = y.id;
        set.TotalAmount = x.amount - this.sharedAmount;
        this.settlements.push(set);
      }

      var y = this.billPayers.find(item => item.amount === this.sharedAmount);
      if (y != null) {
        this.settlements = [];
      }

      this.AddBillModel.SettleModels = this.settlements;
    }

    for (var i = 0; i < 2; i++) {
      let x = new member();
      x.id = this.members[i].id;
      x.amount = this.sharedAmount;
      this.sharedMembers.push(x);
    }

    this.AddBillModel.sharedMember = this.sharedMembers;
    console.log(this.AddBillModel);

    this._appService.addBill(this.AddBillModel).subscribe();
    this.billPayers = [];
    this.settlements = [];
    this.sharedMembers = [];
    this.totalAmount = 0;
    this.AddBillModel.billName = '';
  }
}
