import { Route } from "@angular/router";

export const AUTH_ROUTES: Route[] = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        loadComponent: () =>
            import('./pages/login/login.page').then(m => m.LoginComponent)
    },
]