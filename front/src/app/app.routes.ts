import { Routes } from '@angular/router';
import { provideRouter } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { SalesOrderComponent } from './sales-order/sales-order.component';
import { CustomerOrderComponent } from './customer-order/customer-order.component';
import { EditOrderComponent } from './edit-order/edit-order.component';
import { AddOrderComponent } from './add-order/add-order.component';
import { AuthGuard } from '../services/Auth.guard';
import { AdduserComponent } from './adduser/adduser.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'sales',
    canActivate: [AuthGuard],
    component: SalesOrderComponent,
    data: { roles: ['Sales'] },
  },
  {
    path: 'customer',
    canActivate: [AuthGuard],
    component: CustomerOrderComponent,
    data: { roles: ['Customer'] },
  },
  {
    path: 'edit/:id',
    component: EditOrderComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Customer', 'Sales'] },
  },
  {
    path: 'addOrder',
    component: AddOrderComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Customer', 'Sales'] },
  },
  {
    path: 'adduser',
    component: AdduserComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Sales'] },
  },
];

export const appRoutingProviders = [provideRouter(routes)];
