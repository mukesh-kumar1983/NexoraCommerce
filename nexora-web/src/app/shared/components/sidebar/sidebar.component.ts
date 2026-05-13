import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Home, Package, ShoppingCart, Users, Settings } from 'lucide-angular';
import { LucideAngularModule } from 'lucide-angular';


@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    LucideAngularModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {

  isCollapsed = false;

  Home = Home;
  Package = Package;
  ShoppingCart = ShoppingCart;
  Users = Users;
  Settings = Settings;

  menuItems = [
    { label: 'Dashboard', icon: Home, route: '/app/dashboard' },
    { label: 'Products', icon: Package, route: '/app/products' },
    { label: 'Orders', icon: ShoppingCart, route: '/app/orders' },
    { label: 'Customers', icon: Users, route: '/app/customers' },
    { label: 'Settings', icon: Settings, route: '/app/settings' },
  ];

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }
}
