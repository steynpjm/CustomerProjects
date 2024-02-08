import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Customer } from '../Customer';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'app-update-customer',
  templateUrl: './update-customer.component.html',
  styleUrls: ['./update-customer.component.css']
})
export class UpdateCustomerComponent implements OnInit {
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

  constructor(
    private customerService: CustomerService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = +idParam;
      this.customerService.getCustomer(id).subscribe(customer => this.customer = customer);
    }
  }

  updateCustomer(): void {
    this.customerService.updateCustomer(this.customer.id, this.customer).subscribe(() => {
      this.router.navigate(['/main/customers']);
    });
  }
}
