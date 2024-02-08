import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../User';
import { UserService } from '../user.service';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css']
})
export class UpdateUserComponent implements OnInit {
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

  constructor(
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = +idParam;
      this.userService.getUser(id).subscribe(user => this.user = user);
    }
  }

  updateUser(): void {
    this.userService.updateUser(this.user.id, this.user).subscribe(() => {
      this.router.navigate(['/main/users']);
    });
  }
}
