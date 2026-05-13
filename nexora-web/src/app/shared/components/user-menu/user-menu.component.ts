import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-menu',
  standalone: true,
  imports: [],
  templateUrl: './user-menu.component.html',
  styleUrl: './user-menu.component.css'
})
export class UserMenuComponent {

  constructor(private router: Router) { }

  goToRegister(event: Event) {
    event.preventDefault();
    event.stopPropagation();

    this.isDropdownOpen = false;

    this.router.navigate(['/app/register']);
  }

  isDropdownOpen = false;

  avatarUrl: string =
    'https://ui-avatars.com/api/?name=John+Doe&background=2563eb&color=fff';

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

}
