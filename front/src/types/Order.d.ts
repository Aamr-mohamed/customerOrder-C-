export interface Order {
  id: int;
  customerName: string;
  orderDate: string;
  userId: string;
  orderItems: OrderItem[];
  products?: Product[];
}

export interface OrderItem {
  id?: int;
  orderId?: int;
  productId?: int;
  productName?: string;
  productDescription?: string;
  quantity?: number;
  price?: number;
}

export interface CreateOrder {
  userId: string;
  customerName: string;
  orderDate: string;
  orderItems: OrderItem[];
}

export interface UpdateOrderItem {
  orderId?: int;
  productId?: int;
  productName?: string;
  productDescription?: string;
  quantity?: number;
  price?: number;
}

export interface Product {
  id: int;
  productName: string;
  description: string;
  price: number;
  quantity?: number;
}
