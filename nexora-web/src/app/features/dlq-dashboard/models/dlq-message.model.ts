export interface DlqMessage {
  deliveryTag: number;
  payload: string;
  retrievedAt: string;
}
