/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./**/*.{razor,html}'],
  darkMode: 'class',
  theme: {
    extend: {},
  },
    plugins: [
        require('@tailwindcss/typography'),
        require('@tailwindcss/forms')
    ],
}

