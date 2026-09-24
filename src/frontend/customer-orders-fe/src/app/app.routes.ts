import { Routes } from '@angular/router';
import { CustomerPage } from './pages/customer-page/customer-page';
import { Order } from './models';
import { CustomerDetailsPage } from './pages/customer-details-page/customer-details-page';

export const routes: Routes = [
  {
    path: '',
    redirectTo: "/customers",
    pathMatch: 'full'
  },
  {
    path: 'customers',
    component: CustomerPage,
  },
  {
    path: 'customers/details/:id',
    component: CustomerDetailsPage,
  },
];
