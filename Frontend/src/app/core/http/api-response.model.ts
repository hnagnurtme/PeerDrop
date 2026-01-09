export interface ApiResponse<T> {
    isSuccess: boolean;
    message: string;
    data: T;
    errors? : string ;
    errorCode? : string ;
    timestamp: string;
}