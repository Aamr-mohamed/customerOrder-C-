import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/AuthService';
import { OrderService } from '../../services/OrderService';
import { Order } from '../../types/Order';

@Component({
  selector: 'app-sales-order',
  standalone: true,
  imports: [RouterModule, HttpClientModule, FormsModule, CommonModule],
  templateUrl: './sales-order.component.html',
  styleUrl: './sales-order.component.css',
})
export class SalesOrderComponent {
  constructor(
    private authService: AuthService,
    private router: Router,
    private orderService: OrderService,
  ) {}
  orders: Order[] = [];

  ngOnInit() {
    this.orderService.getOrders().subscribe((orders) => {
      this.orders = orders;
    });
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
