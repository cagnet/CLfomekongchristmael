import { inject, Service } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, filter, map, Observable, of, startWith, switchMap } from 'rxjs';
import { Customer, IResult, Order, ResultState } from '../models';

@Service()
export class CustomerService {
  private readonly httpClient = inject(HttpClient);

  private readonly customersLoadSub = new BehaviorSubject<void>(undefined);
  private readonly activeCustomerSub = new BehaviorSubject<Customer | undefined>(undefined);

  readonly activeCustomer$ = this.activeCustomerSub.asObservable();
  readonly customerList$: Observable<IResult<Customer[]>> = this.customersLoadSub.pipe(
    switchMap((): Observable<IResult<Customer[]>> =>
      this.httpClient.get<Customer[]>('http://localhost:5156/customers').pipe(
        map((data): IResult<Customer[]> => ({ status: ResultState.success, data })),
        startWith<IResult<Customer[]>>({
          status: ResultState.pending,
          data: [],
        }),
        catchError((err, caught): Observable<IResult<Customer[]>> => {
          console.log('Error');
          return of<IResult<Customer[]>>({ data: [], status: ResultState.failed });
        }),
      ),
    ),
  );

  readonly activeCustomerOrders$: Observable<IResult<Order[]>> = this.activeCustomerSub.pipe(
    filter((c) => !!c),
    switchMap((c): Observable<IResult<Order[]>> =>
      this.httpClient.get<Order[]>(`http://localhost:5156/customers/${c.id}/orders`).pipe(
        map((data): IResult<Order[]> => ({ status: ResultState.success, data })),
        startWith<IResult<Order[]>>({
          status: ResultState.pending,
          data: [],
        }),
        catchError((err, caught): Observable<IResult<Order[]>> => {
          console.log('Error');
          return of<IResult<Order[]>>({ data: [], status: ResultState.failed });
        }),
      ),
    ),
  );

  loadCustomer() {
    this.customersLoadSub.next();
  }

  setActiveCustomer(customer: Customer) {
    this.activeCustomerSub.next(customer);
  }
}
