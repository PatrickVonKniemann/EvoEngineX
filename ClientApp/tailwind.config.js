/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './**/*.razor',
        './**/*.html',
        './**/*.cshtml',
        './**/*.js',
    ],
    theme: {
        extend: {
            colors: {
                primary: {
                    DEFAULT: '#A45DE8',
                    light: '#BC84F1',
                    dark: '#7F39FB',
                    darker: '#503FCD7F',
                },
                secondary: {
                    DEFAULT: '#03DAC5',
                    light: '#5AE3D4',
                    dark: '#00C2AE',
                },
                background: {
                    primary: '#131313',
                    secondaryBackgroundColor: '#1C1A1C',
                    darkBackgroundColor: '#131313',
                    infoBackgroundColor: '#BB86FD',
                }
            }
        }
    },
    plugins: [
        function ({addBase, theme}) {
            addBase({
                ':root': {
                    '--color-primary': theme('colors.primary.DEFAULT'),
                    '--color-primary-light': theme('colors.primary.light'),
                    '--color-primary-dark': theme('colors.primary.dark'),
                    '--color-primary-darker': theme('colors.primary.darker'),
                    '--color-secondary': theme('colors.secondary.DEFAULT'),
                    '--color-secondary-light': theme('colors.secondary.light'),
                    '--color-secondary-dark': theme('colors.secondary.dark'),
                    '--color-background-primary': theme('colors.background.primary'),
                    '--color-background-secondary': theme('colors.background.secondaryBackgroundColor'),
                    '--color-background-dark': theme('colors.background.darkBackgroundColor'),
                    '--color-background-info': theme('colors.background.infoBackgroundColor'),
                },
            });
        },
    ],
    
}