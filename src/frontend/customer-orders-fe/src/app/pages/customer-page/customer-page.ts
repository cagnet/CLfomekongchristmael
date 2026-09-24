import { Component, inject, OnInit } from '@angular/core';
import { CustomerService } from '../../services/customer.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { Customer, IResult, ResultState } from '../../models';
import { RouterLink } from '@angular/router';

@Component({
  imports: [RouterLink],
  selector: 'app-customer-page',
  styleUrl: './customer-page.css',
  templateUrl: './customer-page.html',
})
export class CustomerPage implements OnInit {
  ngOnInit(): void {
    this.customerService.loadCustomer();
  }

  private readonly customerService: CustomerService = inject(CustomerService);

  customerListState = toSignal(this.customerService.customerList$, {
    initialValue: {
      data: [],
      status: ResultState.pending,
    } satisfies IResult<Customer[]>,
  });
  protected readonly ResultState = ResultState;

  protected onActiveCustomerSelected(c: Customer) {
    this.customerService.setActiveCustomer(c)
  }
}
