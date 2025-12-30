/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        "primary": "#4bf425",
        "accent-orange": "#ff6b00",
        "background-light": "#F0F0F0",
        "background-dark": "#132210",
      },
      fontFamily: {
        "display": ["Space Grotesk", "sans-serif"],
        "body": ["Noto Sans", "sans-serif"],
      },
      boxShadow: {
        "brutal": "4px 4px 0px 0px #000000",
        "brutal-hover": "2px 2px 0px 0px #000000",
        "brutal-lg": "8px 8px 0px 0px #000000",
      },
      backgroundImage: {
        'grid-pattern': "linear-gradient(#e5e5e5 1px, transparent 1px), linear-gradient(90deg, #e5e5e5 1px, transparent 1px)",
      }
    },
  },
  plugins: [],
}
