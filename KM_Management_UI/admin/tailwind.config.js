/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    fontFamily: {
      'comic-sans': ['Comic Sans MS', 'cursive'],
      roboto: ['Roboto', 'ui-sans-serif', 'system-ui']
    },
    extend: {
      backgroundColor: {
        'side-bar': 'rgba(113, 180, 131, 0.4)',
        'head-bar': 'rgba(113, 180, 131, 0.1)'
      },
      colors: {
        primary: 'rgba(94, 109, 92, 1)'
      }
    }
  },
  plugins: []
}
