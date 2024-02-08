import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Customer } from '../Customer';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'app-add-customer',
  templateUrl: './add-customer.component.html',
  styleUrls: ['./add-customer.component.css']
})
export class AddCustomerComponent implements OnInit, AfterViewInit {

  @ViewChild('customerForm') customerForm!: NgForm;

  customer: Customer = {
    id: 0,
    deletedIndicator: false,
    version: '',
    name: '',
    address1: '',
    address2: '',
    town: '',
    postalcode: '',
    country: '',
    project: []
  };

  constructor(private customerService: CustomerService, private router: Router) { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
  }


  onSubmit(): void {
    if (!this.customerForm.valid) {
      return;
    }
    this.customerService.addCustomer(this.customer).subscribe(() => {
      this.router.navigate(['main/customers']);
    });
  }

}
