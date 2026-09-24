import { Component, inject } from '@angular/core';
import { CustomerService } from '../../services/customer.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { IResult, Order, ResultState } from '../../models';
import { CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  imports: [CurrencyPipe, DatePipe],
  selector: 'app-customer-details-page',
  styleUrl: './customer-details-page.css',
  templateUrl: './customer-details-page.html',
})
export class CustomerDetailsPage {
  private readonly customerService = inject(CustomerService);

  protected readonly orderListState = toSignal(this.customerService.activeCustomerOrders$, {
    initialValue: {
      data: [],
      status: ResultState.pending,
    } satisfies IResult<Order[]>,
  });

  protected readonly activeCustomer = toSignal(this.customerService.activeCustomer$)
  readonly ResultState = ResultState;
}
