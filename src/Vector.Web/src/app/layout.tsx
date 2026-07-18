import type { Metadata } from 'next';
import { Barlow_Condensed, Inter } from 'next/font/google';

import './globals.css';
import SiteLayout from '@/components/site-layout';
import { AuthProvider } from '@/lib/auth/auth-context';
import { CartProvider } from '@/lib/cart/cart-context';

const barlowCondensed = Barlow_Condensed({
  variable: '--font-barlow-condensed',
  subsets: ['latin'],
  weight: ['400', '500', '600', '700', '800'],
});

const inter = Inter({
  variable: '--font-inter',
  subsets: ['latin'],
});

export const metadata: Metadata = {
  title: 'VectorGG',
  description: 'VectorGG',
};

const RootLayout = ({ children }: { children: React.ReactNode }) => {
  return (
    <html lang="en">
      <body className={`${barlowCondensed.variable} ${inter.variable} font-sans antialiased`}>
        <AuthProvider>
          <CartProvider>
            <SiteLayout>{children}</SiteLayout>
          </CartProvider>
        </AuthProvider>
      </body>
    </html>
  );
};

export default RootLayout;
