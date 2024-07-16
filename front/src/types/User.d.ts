export interface LoginResponse {
  id: string;
  username: string;
  email: string;
  token: string;
  role: string;
  expiration: string;
}
export interface RegisterResponse {
  email: string;
  username: string;
  password: string;
  role: string;
}
