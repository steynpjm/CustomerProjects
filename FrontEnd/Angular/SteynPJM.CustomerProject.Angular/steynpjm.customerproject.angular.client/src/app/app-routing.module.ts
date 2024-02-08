import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { MainPageComponent } from './mainpage/mainpage.component';
import { UsersComponent } from './users/users.component';
import { AddUserComponent } from './users/add-user/add-user.component';
import { UpdateUserComponent } from './users/update-user/update-user.component';
import { ProjectsComponent } from './projects/projects.component';
import { UpdateProjectComponent } from './projects/update-project/update-project.component';
import { AddProjectComponent } from './projects/add-project/add-project.component';
import { CustomersComponent } from './customers/customers.component';
import { UpdateCustomerComponent } from './customers/update-customer/update-customer.component'
import { AddCustomerComponent } from './customers/add-customer/add-customer.component'
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] },
  {
    path: 'main',
    component: MainPageComponent,
    children: [
      { path: 'projects', component: ProjectsComponent },
      { path: 'projects/update/:id', component: UpdateProjectComponent },
      { path: 'projects/add', component: AddProjectComponent },
      { path: 'customers', component: CustomersComponent },
      { path: 'customers/update/:id', component: UpdateCustomerComponent },
      { path: 'customers/add', component: AddCustomerComponent },
      { path: 'users/add', component: AddUserComponent },
      { path: 'users/update/:id', component: UpdateUserComponent },
      { path: 'users', component: UsersComponent },
      // other routes...
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
