import { Component } from '@angular/core';
import {
  Order,
  Product,
  OrderItem,
  UpdateOrderItem,
  CreateOrder,
} from '../../types/Order';
import { OrderService } from '../../services/OrderService';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/AuthService';

@Component({
  selector: 'app-add-order',
  standalone: true,
  imports: [RouterModule, HttpClientModule, FormsModule, CommonModule],
  templateUrl: './add-order.component.html',
  styleUrl: './add-order.component.css',
})
export class AddOrderComponent {
  constructor(
    private orderService: OrderService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
  ) {}
  products: Product[] = [];
  order: CreateOrder = {
    userId: '0',
    customerName: '',
    orderDate: '',
    orderItems: [],
  };
  items: { productId: number; quantity: number } = {
    productId: 0,
    quantity: 0,
  };
  productSections: { productId: number; quantity: number }[] = [];
  currentUserId: string | null = localStorage.getItem('currentUserId');

  ngOnInit() {
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

  addProductSection() {
    this.productSections.push({ productId: 0, quantity: 0 });
  }

  removeProductSection(index: number) {
    this.productSections.splice(index, 1);
  }

  addOrderItem() {
    if (this.productSections.length === 0 && !this.order.customerName) {
      console.error('Please add at least one product.');
      return;
    }
    if (!this.currentUserId) {
      this.router.navigate(['/login']);
      return;
    }

    const combinedOrderItems = [...this.productSections];

    console.log(this.items);
    if (this.items.productId !== 0 && this.items.quantity !== 0) {
      combinedOrderItems.unshift({
        productId: this.items.productId,
        quantity: this.items.quantity,
      });
    }

    this.order = {
      userId: this.currentUserId,
      customerName: this.order.customerName,
      orderDate: new Date().toISOString(),
      orderItems: combinedOrderItems,
    };
    this.orderService.createOrder(this.order).subscribe((item) => {
      alert('Order Created');
    });
  }
}
