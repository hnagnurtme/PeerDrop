export interface User {
    id: string;
    email: string;
    userName: string;
    fullName: string;
    role: string;
    avatar?: string;
}

export interface LoginRequest{
    email: string;
    password: string;
}

export interface LoginResponse{
    accessToken: string;
    refreshToken: string;
    expiredAt: string;
    user : User
}