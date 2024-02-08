import { Component, OnInit } from '@angular/core';
import { User } from './User';
import { UserService } from './user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[] = [];

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.userService.getUsers().subscribe(users => this.users = users);
  }

  addUser(): void {
    // Navigate to the AddUserComponent
    this.router.navigate(['main/users/add']);
  }

  updateUser(user: User): void {
    // Navigate to the UpdateUserComponent with the user data
    this.router.navigate(['main/users/update', user.id]);
  }

  deleteUser(user: User): void {
    this.userService.deleteUser(user.id).subscribe(() => {
      this.users = this.users.filter(u => u !== user);
    });
  }
}
