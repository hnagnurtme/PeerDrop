import { create } from 'zustand';
import { persist } from 'zustand/middleware';

export interface User {
    id: string;
    email: string;
    userName: string;
    fullName: string;
    role: string;
    avatar?: string;
}

interface AuthState {
    user: User | null;
    token: string | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    error: string | null;

    // Actions
    setUser: ( user: User | null ) => void;
    setToken: ( token: string | null ) => void;
    setLoading: ( isLoading: boolean ) => void;
    setError: ( error: string | null ) => void;
    login: ( user: User, token: string ) => void;
    logout: () => void;
    clearError: () => void;
}

export const useAuthStore = create<AuthState>()(
    persist(
        ( set ) => ( {
            user: null,
            token: null,
            isAuthenticated: false,
            isLoading: false,
            error: null,

            setUser: ( user ) => set( { user, isAuthenticated: !!user } ),
            setToken: ( token ) => set( { token } ),
            setLoading: ( isLoading ) => set( { isLoading } ),
            setError: ( error ) => set( { error } ),

            login: ( user, token ) => set( {
                user,
                token,
                isAuthenticated: true,
                error: null,
            } ),

            logout: () => set( {
                user: null,
                token: null,
                isAuthenticated: false,
                error: null,
            } ),

            clearError: () => set( { error: null } ),
        } ),
        {
            name: 'auth-storage',
            partialize: ( state ) => ( {
                user: state.user,
                token: state.token,
                isAuthenticated: state.isAuthenticated,
            } ),
        }
    )
);
