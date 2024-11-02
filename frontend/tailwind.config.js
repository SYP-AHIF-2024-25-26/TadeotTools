/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        'informatic-blue': '#0059A7',
        'media-technology-blue': '#70B4D9',
        'elecronics-red': '#CE1223',
        'medicin-technology-yellow': '#f1a102',
        'neutral-green': '#80c076',
      }
    },
  },
  plugins: [
    require('tailwindcss-animated')
  ],
}
