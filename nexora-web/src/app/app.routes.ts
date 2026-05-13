import { Routes } from '@angular/router';


import { PublicLayoutComponent }
from './layouts/public-layout/public-layout.component';

import { AppLayoutComponent }
  from './layouts/app-layout/app-layout.component';


export const routes: Routes = [

  // PUBLIC AREA
  {
    path: '',
    component: PublicLayoutComponent,
    children: [

      {
        path: '',
        loadComponent: () =>
          import('./features/landing/pages/landing/landing.component')
            .then(c => c.LandingComponent)
      },

      {
        path: 'login',
        loadComponent: () =>
          import('./features/auth/pages/login/login.component')
            .then(c => c.LoginComponent)
      }

      
    ]
  },

  // PROTECTED AREA
  {
    path: 'app',
    component: AppLayoutComponent,
    children: [

      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/pages/dashboard/dashboard.component')
            .then(c => c.DashboardComponent)
      }
      ,
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'register',
        loadComponent: () =>
          import('./features/auth/pages/register/register.component')
            .then(c => c.RegisterComponent)
      }
    ]
  },

  // FALLBACK
  {
    path: '**',
    redirectTo: ''
  }
];
