import { Component } from '@angular/core';
import { Order, Product, OrderItem } from '../../types/Order';
import { OrderService } from '../../services/OrderService';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/AuthService';

@Component({
  selector: 'app-customer-order',
  standalone: true,
  imports: [RouterModule, HttpClientModule, FormsModule, CommonModule],
  templateUrl: './customer-order.component.html',
  styleUrl: './customer-order.component.css',
})
export class CustomerOrderComponent {
  orders: Order[] = [];
  orderItems: OrderItem[] = [];
  currentUserId: string | null = localStorage.getItem('currentUserId');

  constructor(
    private orderService: OrderService,
    private router: Router,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    if (!this.currentUserId) {
      this.router.navigate(['/login']);
    } else {
      this.orderService
        .getCustomerOrders(this.currentUserId)
        .subscribe((orders) => {
          this.orders = orders;
          orders.forEach((order) => {
            console.log(order.orderItems);
            this.orderItems = this.orderItems.concat(order.orderItems);
          });
        });
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  deleteOrder(orderId: string) {
    this.orderService.deleteOrder(orderId).subscribe((order) => {
      console.log(order);
      this.orders = this.orders.filter((o) => o.id !== orderId);
      alert('Order Deleted');
    });
  }
}
