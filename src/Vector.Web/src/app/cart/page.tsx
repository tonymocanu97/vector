'use client';

import Image from 'next/image';
import Link from 'next/link';
import { useState } from 'react';

import { useAuth } from '@/lib/auth/auth-context';
import { useCart } from '@/lib/cart/cart-context';

const CartPage = () => {
  const { user, isLoading: authLoading } = useAuth();
  const { cart, isLoading, updateItem, removeItem } = useCart();
  const [error, setError] = useState<string | null>(null);
  const [pendingId, setPendingId] = useState<number | null>(null);

  const handleUpdate = async (productId: number, quantity: number) => {
    setError(null);
    setPendingId(productId);
    try {
      if (quantity < 1) {
        await removeItem(productId);
      } else {
        await updateItem(productId, quantity);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Could not update cart.');
    } finally {
      setPendingId(null);
    }
  };

  if (!authLoading && !user) {
    return (
      <div className="mx-auto max-w-2xl px-6 py-24 text-center">
        <h1 className="text-display text-3xl">Your Cart</h1>
        <p className="mt-3 text-sm text-muted-foreground">Log in to see what&apos;s in your cart.</p>
        <Link
          href="/login"
          className="mt-6 inline-flex items-center gap-2 rounded-md bg-primary px-5 py-2.5 text-sm font-bold uppercase tracking-widest text-primary-foreground"
        >
          Log In
        </Link>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-4xl px-6 py-12 sm:px-10">
        <h1 className="text-display text-3xl">Your Cart</h1>

        {error && <p className="mt-4 text-sm text-red-500">{error}</p>}

        {isLoading && !cart ? (
          <p className="mt-6 text-sm text-muted-foreground">Loading...</p>
        ) : !cart || cart.items.length === 0 ? (
          <div className="mt-10 rounded-xl border border-dashed border-border bg-surface p-10 text-center">
            <p className="text-sm text-muted-foreground">Your cart is empty.</p>
            <Link
              href="/"
              className="mt-6 inline-flex items-center gap-2 rounded-md bg-primary px-5 py-2.5 text-sm font-bold uppercase tracking-widest text-primary-foreground"
            >
              Continue Shopping
            </Link>
          </div>
        ) : (
          <>
            <div className="mt-8 flex flex-col gap-4">
              {cart.items.map(item => (
                <div
                  key={item.id}
                  className="flex flex-wrap items-center gap-4 rounded-lg border border-border bg-surface p-4"
                >
                  <div className="relative h-20 w-20 shrink-0 overflow-hidden rounded-md bg-background">
                    <Image src={item.productImageUrl} alt={item.productName} fill sizes="80px" className="object-cover" />
                  </div>
                  <div className="min-w-0 flex-1">
                    <h3 className="text-sm font-semibold">{item.productName}</h3>
                    <p className="mt-1 text-sm text-muted-foreground">${item.unitPrice.toFixed(2)}</p>
                  </div>
                  <div className="flex items-center gap-2">
                    <button
                      onClick={() => handleUpdate(item.productId, item.quantity - 1)}
                      disabled={pendingId === item.productId}
                      className="grid h-8 w-8 cursor-pointer place-items-center rounded-md border border-border hover:border-primary/60 disabled:cursor-not-allowed disabled:opacity-50"
                    >
                      −
                    </button>
                    <span className="w-6 text-center text-sm">{item.quantity}</span>
                    <button
                      onClick={() => handleUpdate(item.productId, item.quantity + 1)}
                      disabled={pendingId === item.productId}
                      className="grid h-8 w-8 cursor-pointer place-items-center rounded-md border border-border hover:border-primary/60 disabled:cursor-not-allowed disabled:opacity-50"
                    >
                      +
                    </button>
                  </div>
                  <div className="w-20 shrink-0 text-right text-sm font-semibold">${item.lineTotal.toFixed(2)}</div>
                  <button
                    onClick={() => handleUpdate(item.productId, 0)}
                    disabled={pendingId === item.productId}
                    className="cursor-pointer text-xs font-bold uppercase tracking-widest text-muted-foreground hover:text-red-500 disabled:cursor-not-allowed disabled:opacity-50"
                  >
                    Remove
                  </button>
                </div>
              ))}
            </div>

            <div className="mt-8 flex items-center justify-between border-t border-border pt-6">
              <span className="text-lg font-semibold">Total</span>
              <span className="text-display text-2xl">${cart.total.toFixed(2)}</span>
            </div>

            <Link
              href="/checkout"
              className="mt-6 inline-flex w-full items-center justify-center gap-2 rounded-md bg-primary px-6 py-3 text-sm font-bold uppercase tracking-widest text-primary-foreground transition-colors hover:bg-primary/90"
            >
              Proceed to Checkout
            </Link>
          </>
        )}
      </div>
    );
};

export default CartPage;
