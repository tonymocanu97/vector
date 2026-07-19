'use client';

import { createContext, useCallback, useContext, useEffect, useState, type ReactNode } from 'react';

import { ApiError, apiFetch } from '@/lib/api-client';
import { useAuth } from '@/lib/auth/auth-context';

export type CartItem = {
  id: number;
  productId: number;
  productName: string;
  productImageUrl: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
};

export type Cart = {
  id: number;
  items: CartItem[];
  total: number;
};

type CartContextValue = {
  cart: Cart | null;
  isLoading: boolean;
  addItem: (productId: number, quantity: number) => Promise<void>;
  updateItem: (productId: number, quantity: number) => Promise<void>;
  removeItem: (productId: number) => Promise<void>;
  refresh: () => Promise<void>;
};

const CartContext = createContext<CartContextValue | null>(null);

export function CartProvider({ children }: { children: ReactNode }) {
  const { token, logout } = useAuth();
  const [cart, setCart] = useState<Cart | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const refresh = useCallback(async () => {
    if (!token) {
      setCart(null);
      return;
    }

    setIsLoading(true);
    try {
      setCart(await apiFetch<Cart>('/cart', { token }));
    } catch (error) {
      if (error instanceof ApiError && error.status === 401) {
        logout();
        return;
      }
      throw error;
    } finally {
      setIsLoading(false);
    }
  }, [token, logout]);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/set-state-in-effect
    refresh();
  }, [refresh]);

  const addItem = async (productId: number, quantity: number) => {
    setCart(await apiFetch<Cart>('/cart/items', { method: 'POST', body: { productId, quantity }, token }));
  };

  const updateItem = async (productId: number, quantity: number) => {
    setCart(await apiFetch<Cart>(`/cart/items/${productId}`, { method: 'PUT', body: { quantity }, token }));
  };

  const removeItem = async (productId: number) => {
    setCart(await apiFetch<Cart>(`/cart/items/${productId}`, { method: 'DELETE', token }));
  };

  return (
    <CartContext.Provider value={{ cart, isLoading, addItem, updateItem, removeItem, refresh }}>
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
}
