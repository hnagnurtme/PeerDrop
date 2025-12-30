import { z } from 'zod';

// Login Schema
export const loginSchema = z.object( {
    email: z
        .string()
        .min( 1, 'Email is required' )
        .email( 'Invalid email format' ),
    password: z
        .string()
        .min( 1, 'Password is required' )
        .min( 6, 'Password must be at least 6 characters' ),
} );

export type LoginFormData = z.infer<typeof loginSchema>;

// Signup Schema
export const signupSchema = z.object( {
    username: z
        .string()
        .min( 1, 'Username is required' )
        .min( 3, 'Username must be at least 3 characters' )
        .max( 20, 'Username must be at most 20 characters' )
        .regex( /^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores' ),
    email: z
        .string()
        .min( 1, 'Email is required' )
        .email( 'Invalid email format' ),
    password: z
        .string()
        .min( 1, 'Password is required' )
        .min( 6, 'Password must be at least 6 characters' )
        .regex( /[A-Z]/, 'Password must contain at least one uppercase letter' )
        .regex( /[0-9]/, 'Password must contain at least one number' ),
    confirmPassword: z
        .string()
        .min( 1, 'Please confirm your password' ),
} ).refine( ( data ) => data.password === data.confirmPassword, {
    message: 'Passwords do not match',
    path: [ 'confirmPassword' ],
} );

export type SignupFormData = z.infer<typeof signupSchema>;
