/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        'background-main': '#D9D9D9',
      }
    },
  },
  plugins: [
    require('tailwindcss-animated')
  ],
}
