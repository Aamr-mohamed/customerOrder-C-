import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  Order,
  OrderItem,
  Product,
  UpdateOrderItem,
  CreateOrder,
} from '../types/Order';
import { environment } from '../../environment';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  constructor(private http: HttpClient) {}
  apiUrl = environment.apiUrl;

  token = localStorage.getItem('token');

  getOrders() {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.get<Order[]>(`${this.apiUrl}/order`, { headers });
  }

  getOrder(Id: number) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.get<Order>(`${this.apiUrl}/order/${Id}`, { headers });
  }

  getCustomerOrders(customerId: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.get<Order[]>(`${this.apiUrl}/order/user/${customerId}`, {
      headers,
    });
  }

  getOrderItems(orderId: number) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.get<Product[]>(
      `${this.apiUrl}/order/${orderId}/products`,
      { headers },
    );
  }

  addOrderItem(orderId: number, orderItem: UpdateOrderItem) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.post<OrderItem>(
      `${this.apiUrl}/orderitems/${orderId}`,
      orderItem,
      { headers },
    );
  }

  editOrderItem(orderId: number, orderItem: UpdateOrderItem) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.put<OrderItem>(
      `${this.apiUrl}/orderitems/${orderId}`,
      orderItem,
      { headers },
    );
  }

  deleteOrderItem(orderItemId: number) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.delete<OrderItem>(
      `${this.apiUrl}/orderitems/${orderItemId}`,
      { headers },
    );
  }

  getProducts() {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.get<Product[]>(`${this.apiUrl}/orderitems/products`, {
      headers,
    });
  }

  createOrder(order: CreateOrder) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.post<Order>(`${this.apiUrl}/order`, order, { headers });
  }

  updateOrder(order: Partial<Order>) {
    return this.http.put<Order>(`${this.apiUrl}/order/${order.id}`, order);
  }

  deleteOrder(Id: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.token}`,
    });
    return this.http.delete<Order>(`${this.apiUrl}/order/${Id}`, { headers });
  }
}
