import { Component, OnInit } from '@angular/core';
import { DlqService } from '../../services/dlq.service';
import { DlqMessage } from '../../models/dlq-message.model';

@Component({
  selector: 'app-dlq-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dlq-dashboard.component.html',
  styleUrl: './dlq-dashboard.component.css'
})
export class DlqDashboardComponent implements OnInit {

  messages: DlqMessage[] = [];

  loading = false;

  constructor(private dlqService: DlqService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(): void {
    this.loading = true;

    this.dlqService.getMessages().subscribe({
      next: (response: any) => {
        this.messages = response;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  replay(message: DlqMessage): void {
    this.dlqService.replayMessage(message).subscribe(() => {
      this.loadMessages();
    });
  }

  delete(message: DlqMessage): void {
    this.dlqService.deleteMessage(message.deliveryTag).subscribe(() => {
      this.loadMessages();
    });
  }

}
