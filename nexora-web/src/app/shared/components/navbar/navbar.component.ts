import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserMenuComponent } from '../user-menu/user-menu.component';
import { AuthService } from '../../../features/auth/services/auth.service.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    UserMenuComponent,
    CommonModule

  ],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  constructor(public authService: AuthService) {
    
  }

  ngOnInit() {
   
  }

}
