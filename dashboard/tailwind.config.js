/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}" // Ensure it includes your Angular files
  ],
  theme: {
    extend: {
      colors: {
        'informatic-blue': '#0059A7',
        'media-technology-blue': '#70B4D9',
        'electronics-red': '#CE1223',
        'medicin-technology-yellow': '#f1a102',
        'neutral-green': '#80c076',
        'htl-orange': '#e8704f',
        'htl-orange-dark': '#b4573d',
      }
    },
  },
  plugins: [],
}
