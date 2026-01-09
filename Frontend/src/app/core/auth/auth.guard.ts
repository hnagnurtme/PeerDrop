import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthFacade } from './auth.facade';

export const authGuard: CanActivateFn = () => {
    const auth = inject( AuthFacade );
    const router = inject( Router );

    if ( auth.isAuthenticated() ) {
        return true;
    }

    router.navigate( [ '/auth/login' ] );
    return false;
};
