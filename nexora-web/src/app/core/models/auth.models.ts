export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface AuthUser {
  firstName: string;
  lastName: string;
  email: string;
  token: string;
}
