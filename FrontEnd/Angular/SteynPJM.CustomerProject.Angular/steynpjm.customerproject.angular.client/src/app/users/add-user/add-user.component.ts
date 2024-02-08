import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../User';
import { UserService } from '../user.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit, AfterViewInit {

  @ViewChild('userForm') userForm!: NgForm;

  titles: string[] = ['Mr', 'Mrs', 'Miss', 'Dr'];

  user: User = {
    id: 0,
    deletedIndicator: false,
    version: '',
    companyHid: 0,
    username: '',
    password: '',
    title: '',
    firstname: '',
    lastname: '',
    email: '',
    designation: '',
    companyH: null,
    project: []
  };

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.userForm.form.controls['email'].setValidators([Validators.required, Validators.email]);
    this.userForm.form.controls['email'].updateValueAndValidity();
  }


  onSubmit(): void {
    if (!this.userForm.valid) {
      return;
    }
    this.userService.addUser(this.user).subscribe(() => {
      this.router.navigate(['main/users']);
    });
  }

}
