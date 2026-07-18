'use client';

import { useRouter } from 'next/navigation';
import { useState, type FormEvent } from 'react';

import { apiFetch } from '@/lib/api-client';
import { useAuth } from '@/lib/auth/auth-context';
import { useCart } from '@/lib/cart/cart-context';

type CheckoutOrder = { id: number };

const CheckoutPage = () => {
  const { token } = useAuth();
  const { cart, refresh } = useCart();
  const router = useRouter();

  const [shippingFullName, setShippingFullName] = useState('');
  const [shippingAddressLine, setShippingAddressLine] = useState('');
  const [shippingCity, setShippingCity] = useState('');
  const [shippingPostalCode, setShippingPostalCode] = useState('');
  const [shippingCountry, setShippingCountry] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      const order = await apiFetch<CheckoutOrder>('/orders/checkout', {
        method: 'POST',
        token,
        body: {
          shippingFullName,
          shippingAddressLine,
          shippingCity,
          shippingPostalCode,
          shippingCountry,
        },
      });
      await refresh();
      router.push(`/orders?placed=${order.id}`);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Something went wrong.');
    } finally {
      setIsSubmitting(false);
    }
  };

  if (!cart || cart.items.length === 0) {
    return (
      <div className="mx-auto max-w-2xl px-6 py-24 text-center">
        <h1 className="text-display text-3xl">Checkout</h1>
        <p className="mt-3 text-sm text-muted-foreground">Your cart is empty.</p>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-2xl px-6 py-12 sm:px-10">
        <h1 className="text-display text-3xl">Checkout</h1>

        <div className="mt-6 rounded-xl border border-border bg-surface p-5">
          {cart.items.map(item => (
            <div key={item.id} className="flex items-center justify-between py-1 text-sm">
              <span className="text-muted-foreground">
                {item.productName} × {item.quantity}
              </span>
              <span>${item.lineTotal.toFixed(2)}</span>
            </div>
          ))}
          <div className="mt-3 flex items-center justify-between border-t border-border pt-3 text-base font-semibold">
            <span>Total</span>
            <span>${cart.total.toFixed(2)}</span>
          </div>
        </div>

        <form onSubmit={handleSubmit} className="mt-8 flex flex-col gap-4">
          <input
            type="text"
            required
            placeholder="Full name"
            value={shippingFullName}
            onChange={e => setShippingFullName(e.target.value)}
            className="h-11 rounded-md border border-border bg-background px-4 text-sm outline-none focus:border-primary"
          />
          <input
            type="text"
            required
            placeholder="Address"
            value={shippingAddressLine}
            onChange={e => setShippingAddressLine(e.target.value)}
            className="h-11 rounded-md border border-border bg-background px-4 text-sm outline-none focus:border-primary"
          />
          <div className="flex gap-3">
            <input
              type="text"
              required
              placeholder="City"
              value={shippingCity}
              onChange={e => setShippingCity(e.target.value)}
              className="h-11 flex-1 rounded-md border border-border bg-background px-4 text-sm outline-none focus:border-primary"
            />
            <input
              type="text"
              required
              placeholder="Postal code"
              value={shippingPostalCode}
              onChange={e => setShippingPostalCode(e.target.value)}
              className="h-11 flex-1 rounded-md border border-border bg-background px-4 text-sm outline-none focus:border-primary"
            />
          </div>
          <input
            type="text"
            required
            placeholder="Country"
            value={shippingCountry}
            onChange={e => setShippingCountry(e.target.value)}
            className="h-11 rounded-md border border-border bg-background px-4 text-sm outline-none focus:border-primary"
          />
          {error && <p className="text-sm text-red-500">{error}</p>}
          <button
            type="submit"
            disabled={isSubmitting}
            className="h-11 cursor-pointer rounded-md bg-primary text-sm font-bold uppercase tracking-widest text-primary-foreground transition-colors hover:bg-primary/90 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {isSubmitting ? 'Placing order...' : 'Place Order'}
          </button>
        </form>
      </div>
  );
};

export default CheckoutPage;
