import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { User, UserEditModel } from '../Model/User';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private _appService: AppService,private router: Router) { }

  model = new UserEditModel;
  

  ngOnInit() {
  }


  onSubmit(form: any) {
   // console.log(this.model.email + ":" + this.model.password);

    this._appService.userLogin(this.model.email, this.model.password).subscribe((data: any) => {

      if (data.status == true) {
        console.log("correct");
        
        this.router.navigate(['/'+data.id]);
      }
    },
      err => {
        //console.log(err),
        alert("Invalid data...please register first...");
      });

   
  }
}
