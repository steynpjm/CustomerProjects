import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AuthGuard } from './auth/auth.guard'; 
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { MainPageComponent } from './mainpage/mainpage.component';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { CustomersComponent } from './customers/customers.component';
import { AddCustomerComponent } from './customers/add-customer/add-customer.component';
import { UpdateCustomerComponent } from './customers/update-customer/update-customer.component';
import { UsersComponent } from './users/users.component';
import { AddUserComponent } from './users/add-user/add-user.component';
import { UpdateUserComponent } from './users/update-user/update-user.component';
import { ProjectsComponent } from './projects/projects.component';
import { UpdateProjectComponent } from './projects/update-project/update-project.component'
import { AddProjectComponent } from './projects/add-project/add-project.component'

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    MainPageComponent,
    ProjectsComponent,
    UpdateProjectComponent,
    AddProjectComponent,
    CustomersComponent,
    UpdateCustomerComponent,
    AddCustomerComponent,
    UsersComponent,
    AddUserComponent,
    UpdateUserComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    RouterModule,
    FormsModule
  ],
  providers: [
    AuthGuard,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

