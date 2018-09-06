import { Component, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { UserEditModel } from '../Model/User';
import { GroupResponse, Grpmember } from '../Model/Group';
import { Balance, Settle } from '../Model/Settle';
import { Transaction } from '../Model/Transaction';
import { AddBill, member } from '../Model/AddBill';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  UserId: number;
  UserName: string;
  user=new UserEditModel();
  friends: UserEditModel[];
  groups: GroupResponse[] = [];
  friendId: number;
  groupId: number;
  amount: number;
  settleRecords: Balance[] = [];
  payment=new Transaction();

  AddBillModel = new AddBill();
  billPayer = new member();
  billPayers: member[] = [];
  sharedMembers: member[] = [];
  totalAmount: number;
  Members: any[] = [];
  SharedArray: any[] = [];
  PayersArray: any[] = [];
  memberCount: number;
  sharedAmount: number;
  settlements: Settle[] = [];
  payer: member[] = [];
  payee: member[] = [];
  showMultiplePayer: boolean = false;

  constructor(private _appService: AppService, private route: ActivatedRoute, private renderer: Renderer2) {
    this.UserId = this.route.snapshot.params['id'];
   // console.log(this.UserId);
  }

  ngOnInit() {

    this.route.parent.params.subscribe(params => {
      this.UserId = +params['id'];
    });
   // console.log(this.UserId);

    this._appService.getSingleFriendDetail(this.UserId).subscribe((data: any) => {
      this.user = data;
      this.UserName = data.userName;
      let x = { "id": data.userId, "name": data.userName };
      this.SharedArray.push(x);
      this.PayersArray.push(x);
      this.Members.push(x);
    });

    this._appService.getAllFriends(this.UserId).subscribe((data: any) => {
      this.friends = data;
      for (var i = 0; i < this.friends.length; i++) {
        let x = { "id": this.friends[i].userId, "name": this.friends[i].userName };
        this.Members.push(x);
      }
    });
    console.log(this.Members);
  }

  selectFriend(id: number) {
    var x = this.friends.find(item => item.userId === +id);
    this.friendId = x.userId;
    
    this.amount = 0;
    document.getElementById("receiver").innerText = x.userName;
    document.getElementById("groups").innerText = "Select Group";
    this._appService.getCommenGroups(this.UserId, x.userId).subscribe((data: any) => {
      this.groups = data;
      let x = new GroupResponse();
      x.groupId = 0;
      x.groupName = "Non-Group";
      this.groups.push(x);
    });
  }

  selectGroup(id: number) {
    var gdata = this.groups.find(item => item.groupId === +id);
    document.getElementById("groups").innerText = gdata.groupName;
    this.groupId = gdata.groupId;
   this.getBalance();
  }

  

  getBalance() {
    this._appService.getIndividualbalance(this.UserId, this.friendId).subscribe((data: any) => {
      this.settleRecords = data;
      console.log(this.settleRecords);
      
      var exist = this.settleRecords.find(item => item.group_id === +this.groupId);
      if (exist == null) {
        alert("You does not owe anything...");
        this.amount = 0;
      }
      else {
        for (var i = 0; i < this.settleRecords.length; i++) {
          if (this.groupId == this.settleRecords[i].group_id) {
            if (this.UserId == this.settleRecords[i].payer_id && this.settleRecords[i].amount > 0) {
              this.amount = this.settleRecords[i].amount;
            }
            else if (this.UserId == this.settleRecords[i].receiver_id && this.settleRecords[i].amount < 0) {
              this.amount = Math.abs(this.settleRecords[i].amount);
           }
            else {
              alert("You does not owe anything...");
             this.amount = 0;
            }
          }
        }
      }
    });
  }

  savePayment() {
    if (this.amount == 0) {
      alert("can not record payment");
      this.payment = null;
    }
    else {
      this.payment = new Transaction();
      this.payment.transPayersId = this.UserId;
      this.payment.transReceiversId = this.friendId;
      this.payment.paidAmount = this.amount;
      if (this.groupId != 0) {
        this.payment.groupId = this.groupId;
      }
      this.payment.createdDate = new Date().toLocaleString();
    }
    console.log(this.payment);
    this._appService.recordPayment(this.payment).subscribe();
    this.amount = 0;
  }

  selectSharedMember(id: number, name: string) {

    var exist = this.SharedArray.find(item => item.id === +id);
    if (exist == null) {
      let member = { "id": id, "name": name };
      this.SharedArray.push(member);
    }
    //var exist = this.PayersArray.find(item => item.id === +id);
    //if (exist == null) {
    //  this.PayersArray.push({ "id": id, "name": name });
    //}
  }

  removeSharedMember(id: number) {
    if (id != this.UserId) {
      var exist = this.SharedArray.find(item => item.id === +id);
      if (exist != null) {
        let index = this.SharedArray.indexOf(exist);
        this.SharedArray.splice(index, 1);
      }
    }
  }

  showPayer() {
    //document.getElementById("payers").innerText = "Multiple";
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

    //let x = this.groupMember.find(y => y.id === this.billPayer.id);
    //document.getElementById("payers").innerText = x.name;
  }

  addMultiplePayer(id: number, amount: number) {
    let x = new member();
    x.id = id;
    x.amount = amount;
    this.billPayers.push(x);
    this.billPayer = null;
  }

  saveBill() {
    this.AddBillModel.creatorId = this.UserId;
    this.AddBillModel.createdDate = new Date().toLocaleString();
    this.sharedAmount = this.totalAmount / this.SharedArray.length;

    for (var i = 0; i < this.SharedArray.length; i++) {
      let x = new member();
      x.id = this.SharedArray[i].id;
      x.amount = this.sharedAmount;
      this.sharedMembers.push(x);
    }

    this.AddBillModel.sharedMember = this.sharedMembers;

    if (this.billPayer != null) {
      this.AddBillModel.payer = [];
      this.AddBillModel.payer.push(this.billPayer);

      for (var i = 0; i < this.sharedMembers.length; i++) {
        if (this.sharedMembers[i].id != this.billPayer.id) {
          let settle = new Settle();
          settle.PayerId = this.sharedMembers[i].id;
          settle.SharedMemberId = this.billPayer.id;
          settle.TotalAmount = this.sharedAmount;
          this.settlements.push(settle);
        }
      }
      this.AddBillModel.SettleModels = this.settlements;
    }
    else {
      this.AddBillModel.payer = this.billPayers;

      for (var i = 0; i < this.sharedMembers.length; i++) {
        var payerExist = this.billPayers.find(item => item.id === this.sharedMembers[i].id);
        if (payerExist != null) {
          if (payerExist.amount > this.sharedAmount) {
            let x = new member();
            x.id = this.sharedMembers[i].id;
            x.amount = payerExist.amount - this.sharedAmount
            this.payee.push(x);
          }
          else if (payerExist.amount < this.sharedAmount) {
            let x = new member();
            x.id = this.sharedMembers[i].id;
            x.amount = this.sharedAmount - payerExist.amount;
            this.payer.push(x);
          }
        }
        else {
          let x = new member();
          x.id = this.sharedMembers[i].id;
          x.amount = this.sharedMembers[i].amount;
          this.payer.push(x);
        }
      }

      //console.log(this.payee);
      // console.log(this.payer);


      if (this.payee.length == 1) {
        for (var i = 0; i < this.payer.length; i++) {
          for (var j = 0; j < this.payee.length; j++) {
            var set = new Settle();
            set.PayerId = this.payer[i].id;
            set.SharedMemberId = this.payee[j].id;
            
            set.TotalAmount = this.payer[i].amount;
            this.settlements.push(set);
          }
        }
      }
      else {
        if (this.payer.length == 1) {
          for (var i = 0; i < this.payee.length; i++) {
            for (var j = 0; j < this.payer.length; j++) {
              var set = new Settle();
              set.PayerId = this.payer[j].id;
              set.SharedMemberId = this.payee[i].id;
              
              set.TotalAmount = this.payee[i].amount;
              this.settlements.push(set);
            }
          }
        }
        else {
          for (var i = 0; i < this.payer.length; i++) {
            for (var j = 0; j < this.payee.length; j++) {

              var set = new Settle();
              set.PayerId = this.payer[i].id;
              set.SharedMemberId = this.payee[j].id;
             

              if (this.payee[j].amount < this.payer[i].amount) {

                set.TotalAmount = this.payee[j].amount;
                if (set.TotalAmount != 0) {
                  this.settlements.push(set);
                }
                this.payee[j].amount = this.payee[j].amount - set.TotalAmount;
                this.payer[i].amount = this.payer[i].amount - set.TotalAmount;
              }
              else if (this.payee[j].amount > this.payer[i].amount) {
                set.TotalAmount = this.payer[i].amount;
                if (set.TotalAmount != 0) {
                  this.settlements.push(set);
                }
                this.payer[i].amount = this.payer[i].amount - set.TotalAmount;
                this.payee[j].amount = this.payee[j].amount - set.TotalAmount;
              }
              else {  //equal
                set.TotalAmount = this.payer[i].amount;
                if (set.TotalAmount != 0) {
                  this.settlements.push(set);
                }
                this.payer[i].amount = this.payer[i].amount - set.TotalAmount;
                this.payee[j].amount = this.payee[i].amount - set.TotalAmount;
              }
            }
          }
        }
      }

      
      
      this.AddBillModel.SettleModels = this.settlements;
    }
    console.log(this.AddBillModel);

    if (this.sharedMembers.length == 1) {
      alert("can not add bill please select more users");
    }


    this.PayersArray = [];
    this.SharedArray = [];
    let x = { "id": this.UserId, "name": this.UserName };
    this.SharedArray.push(x);
    this.settlements = [];
    this.billPayers = [];
    this.sharedMembers = [];
    this.payer = [];
    this.payee = [];
    this.AddBillModel.billName = '';
    this.totalAmount = null;

  }
}
