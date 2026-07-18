'use client';

import { useSearchParams } from 'next/navigation';
import { Suspense, useEffect, useState } from 'react';

import { apiFetch } from '@/lib/api-client';
import { useAuth } from '@/lib/auth/auth-context';

type OrderItem = {
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
};

type Order = {
  id: number;
  status: string;
  totalAmount: number;
  createdAt: string;
  items: OrderItem[];
};

const OrdersContent = () => {
  const { token, user, isLoading: authLoading } = useAuth();
  const searchParams = useSearchParams();
  const placedId = searchParams.get('placed');
  const [orders, setOrders] = useState<Order[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (!token) {
      setIsLoading(false);
      return;
    }
    apiFetch<Order[]>('/orders', { token })
      .then(setOrders)
      .finally(() => setIsLoading(false));
  }, [token]);

  if (!authLoading && !user) {
    return (
      <div className="mx-auto max-w-2xl px-6 py-24 text-center">
        <h1 className="text-display text-3xl">Your Orders</h1>
        <p className="mt-3 text-sm text-muted-foreground">Log in to see your order history.</p>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-3xl px-6 py-12 sm:px-10">
      <h1 className="text-display text-3xl">Your Orders</h1>

      {placedId && (
        <div className="mt-6 rounded-xl border border-primary/60 bg-primary/10 p-5 text-sm">
          Order #{placedId} placed successfully. Thank you!
        </div>
      )}

      {isLoading ? (
        <p className="mt-6 text-sm text-muted-foreground">Loading...</p>
      ) : orders.length === 0 ? (
        <p className="mt-6 text-sm text-muted-foreground">You haven&apos;t placed any orders yet.</p>
      ) : (
        <div className="mt-8 flex flex-col gap-4">
          {orders.map(order => (
            <div key={order.id} className="rounded-xl border border-border bg-surface p-5">
              <div className="flex items-center justify-between">
                <span className="text-sm font-semibold">Order #{order.id}</span>
                <span className="rounded-full bg-primary/10 px-3 py-1 text-xs font-bold uppercase tracking-widest text-primary">
                  {order.status}
                </span>
              </div>
              <p className="mt-1 text-xs text-muted-foreground">{new Date(order.createdAt).toLocaleDateString()}</p>
              <div className="mt-3 flex flex-col gap-1">
                {order.items.map(item => (
                  <div key={item.productId} className="flex items-center justify-between text-sm text-muted-foreground">
                    <span>
                      {item.productName} × {item.quantity}
                    </span>
                    <span>${item.lineTotal.toFixed(2)}</span>
                  </div>
                ))}
              </div>
              <div className="mt-3 flex items-center justify-between border-t border-border pt-3 text-sm font-semibold">
                <span>Total</span>
                <span>${order.totalAmount.toFixed(2)}</span>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

const OrdersPage = () => (
  <Suspense fallback={<div className="px-6 py-12 text-sm text-muted-foreground">Loading...</div>}>
    <OrdersContent />
  </Suspense>
);

export default OrdersPage;
