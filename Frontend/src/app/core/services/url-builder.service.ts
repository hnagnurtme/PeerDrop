import { environment } from '@/environments/environment';

export function buildApiUrl ( path: string ): string {
    return `${ environment.apiUrl }/${ path }`;
}
