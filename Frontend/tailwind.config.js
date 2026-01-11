/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [ './src/**/*.{html,ts}' ],
    darkMode: 'class',
    theme: {
        extend: {
            colors: {
                primary: '#10b981',
                'primary-hover': '#059669',
                'background-dark': '#020406',
                'cyber-emerald': '#00ff9d',
                'neon-cyan': '#00f2ff',
                'toast-success': '#10b981',
                'toast-error': '#ef4444',
                'toast-warning': '#f59e0b',
                'toast-info': '#3b82f6',
            },
            fontFamily: {
                sans: [ 'Plus Jakarta Sans', 'sans-serif' ],
                display: [ 'Plus Jakarta Sans', 'sans-serif' ],
            },
            borderRadius: {
                DEFAULT: '0.5rem',
                lg: '1rem',
                xl: '1.5rem',
                full: '9999px',
            },
            keyframes: {
                shine: {
                    '100%': { left: '125%' },
                },
                'data-flow': {
                    '0%': { left: '0%', opacity: '0' },
                    '10%': { opacity: '0.6' },
                    '90%': { opacity: '0.6' },
                    '100%': { left: '100%', opacity: '0' },
                },
            },
            animation: {
                shine: 'shine 3s infinite',
                'data-flow': 'data-flow 3s linear infinite',
                'data-flow-delayed': 'data-flow 3s linear infinite 1.5s',
            },
        },
    },
    plugins: [],
};
