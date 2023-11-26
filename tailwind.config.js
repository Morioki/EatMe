/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./EatMe.Client/**/*.{razor,html}',
                './EatMe.Shared/**/*.{razor,html}'],
  darkMode: 'class',
  theme: {
    extend: {},
  },
    plugins: [
        require('@tailwindcss/typography'),
        require('@tailwindcss/forms')
    ],
}

