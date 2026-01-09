import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { StorageService } from '@/app/core/services/storage.service';

export const authInterceptor: HttpInterceptorFn = ( req, next ) => {
    const storage = inject( StorageService );
    const token = storage.getAccessToken();

    if ( !token ) return next( req );

    const authReq = req.clone( {
        setHeaders: {
            Authorization: `Bearer ${ token }`
        }
    } );

    return next( authReq );
};
