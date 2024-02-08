import { Component, OnInit } from '@angular/core';
import { Customer } from './Customer';
import { CustomerService } from './customer.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit {
  customers: Customer[] = [];

  constructor(private customerService: CustomerService, private router: Router) { }

  ngOnInit(): void {
    this.getCustomers();
  }

  getCustomers(): void {
    this.customerService.getCustomers().subscribe(customers => this.customers = customers);
  }

  addCustomer(): void {
    // Navigate to the AddCustomerComponent
    this.router.navigate(['main/customers/add']);
  }

  updateCustomer(customer: Customer): void {
    // Navigate to the UpdateCustomerComponent with the user data
    this.router.navigate(['main/customers/update', customer.id]);
  }

  deleteCustomer(customer: Customer): void {
    this.customerService.deleteCustomer(customer.id).subscribe(() => {
      this.customers = this.customers.filter(u => u !== customer);
    });
  }
}
