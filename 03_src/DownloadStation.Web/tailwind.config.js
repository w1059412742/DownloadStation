/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        background: '#FAFAFA',
        foreground: '#0A0A0A',
        card: '#FFFFFF',
        'card-foreground': '#0A0A0A',
        primary: '#3B82F6',
        'primary-foreground': '#FFFFFF',
        secondary: '#F4F4F5',
        'secondary-foreground': '#18181B',
        muted: '#F4F4F5',
        'muted-foreground': '#71717A',
        accent: '#F4F4F5',
        'accent-foreground': '#18181B',
        border: '#E4E4E7',
        input: '#E4E4E7',
        ring: '#3B82F6',
        
        // Keep some old semantic names mapped if necessary or just replace completely.
        // Based on analysis, Home.vue uses textPrimary, textSecondary, textHint.
        // We will replace them in Home.vue, so we can just use the demo colors.
        surface: '#FFFFFF', // keep for compatibility if missed
        textPrimary: '#0A0A0A', // map to foreground
        textSecondary: '#71717A', // map to muted-foreground
        textHint: '#9CA3AF',
        borderHover: '#D1D5DB',
        success: '#10B981',
        danger: '#EF4444'
      },
      fontFamily: {
        sans: ['Inter', 'system-ui', '-apple-system', 'BlinkMacSystemFont', 'Segoe UI', 'Roboto', 'sans-serif'],
      },
      borderRadius: {
        DEFAULT: '0.5rem',
        'xl': '12px',
        '2xl': '16px',
        '3xl': '24px'
      },
      boxShadow: {
        'card': '0 1px 3px 0 rgb(0 0 0 / 0.04), 0 1px 2px -1px rgb(0 0 0 / 0.04)',
        'card-hover': '0 10px 40px -10px rgb(0 0 0 / 0.08), 0 4px 12px -2px rgb(0 0 0 / 0.04)',
        'modal': '0 25px 50px -12px rgb(0 0 0 / 0.15)',
        'soft': '0 4px 20px rgba(0, 0, 0, 0.05)',
        'hover': '0 10px 30px rgba(0, 0, 0, 0.08)'
      },
    },
  },
  plugins: [],
}
