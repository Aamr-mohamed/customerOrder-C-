import { Component } from '@angular/core';
import { Order, Product, OrderItem, UpdateOrderItem } from '../../types/Order';
import { OrderService } from '../../services/OrderService';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/AuthService';

@Component({
  selector: 'app-edit-order',
  standalone: true,
  imports: [RouterModule, HttpClientModule, FormsModule, CommonModule],
  templateUrl: './edit-order.component.html',
  styleUrl: './edit-order.component.css',
})
export class EditOrderComponent {
  orderItems: OrderItem[] = [];
  orderId: number = 0;
  productId: number = 0;
  quantity: number = 0;
  products: Product[] = [];
  orderItem: UpdateOrderItem = {};

  currentUserId: string | null = localStorage.getItem('currentUserId');

  constructor(
    private orderService: OrderService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.orderId = Number(params['id']);
    });
    this.orderService.getOrder(this.orderId).subscribe((items) => {
      console.log(items);
      items.orderItems.forEach((item) => {
        this.orderItems = this.orderItems.concat(item);
      });
    });

    this.orderService.getProducts().subscribe((products) => {
      products.forEach((product) => {
        this.products = this.products.concat(product);
        console.log(product);
      });
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  addOrderItem() {
    if (this.productId === 0) {
      console.error('Please select a product before adding.');
      return;
    }
    this.route.params.subscribe((params) => {
      this.orderId = Number(params['id']);
    });
    this.orderItem = {
      productId: this.productId,
      quantity: this.quantity,
    };
    this.orderService
      .addOrderItem(this.orderId, this.orderItem)
      .subscribe((item) => {
        alert('Item Added');
      });
  }

  editOrderItem(orderId: number, orderItem: UpdateOrderItem) {
    this.orderService.editOrderItem(orderId, orderItem).subscribe((item) => {
      alert('Item Edited');
    });
  }

  deleteOrderItem(orderItemId: number) {
    this.orderService.deleteOrderItem(orderItemId).subscribe((item) => {
      this.orderItems = this.orderItems.filter((o) => o.id !== orderItemId);
      alert('Item Deleted');
    });
  }
}
