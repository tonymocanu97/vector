import type { Config } from 'tailwindcss';

const config: Config = {
  content: [
    './src/pages/**/*.{ts,tsx}',
    './src/components/**/*.{ts,tsx}',
    './src/app/**/*.{ts,tsx}',
  ],
  theme: {
    extend: {
      colors: {
        background: 'hsl(var(--background))',
        foreground: 'hsl(var(--foreground))',
        border: 'hsl(var(--border))',
        card: 'hsl(var(--card))',

        surface: 'hsl(var(--surface))',
        muted: 'hsl(var(--muted))',
        subtle: 'hsl(var(--subtle))',

        accent: 'hsl(var(--accent))',
        'accent-light': 'hsl(var(--accent-light))',

        success: 'hsl(var(--success))',
        danger: 'hsl(var(--danger))',
      },
    },
    fontFamily: {
      heading: ['var(--font-space-grotesk)', 'sans-serif'],
      body: ['var(--font-inter)', 'sans-serif'],
    },
    container: {
      center: true,
      padding: '2rem',
      screens: {
        '2xl': '1400px',
      },
    },
  },
  plugins: [],
};

export default config;
