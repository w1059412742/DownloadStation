/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        background: '#FAFAFB',
        surface: '#FFFFFF',
        primary: '#3B82F6',
        primaryHover: '#2563EB',
        textPrimary: '#111827',
        textSecondary: '#6B7280',
        textHint: '#9CA3AF',
        border: '#E5E7EB',
        borderHover: '#D1D5DB',
        success: '#10B981',
        danger: '#EF4444'
      },
      fontFamily: {
        sans: ['Inter', 'system-ui', 'Avenir', 'Helvetica', 'Arial', 'sans-serif'],
      },
      boxShadow: {
        'soft': '0 4px 20px rgba(0, 0, 0, 0.05)',
        'hover': '0 10px 30px rgba(0, 0, 0, 0.08)'
      },
      borderRadius: {
        'xl': '12px',
        '2xl': '16px',
        '3xl': '24px'
      }
    },
  },
  plugins: [],
}
