'use client';

import Image from 'next/image';
import { useRouter } from 'next/navigation';
import { useState } from 'react';

import { useAuth } from '@/lib/auth/auth-context';
import { useCart } from '@/lib/cart/cart-context';
import type { Product } from '@/lib/products';

const ProductCard = ({ product }: { product: Product }) => {
  const { token } = useAuth();
  const { addItem } = useCart();
  const router = useRouter();
  const [isAdding, setIsAdding] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleAddToCart = async () => {
    if (!token) {
      router.push('/login');
      return;
    }

    setIsAdding(true);
    setError(null);

    try {
      await addItem(product.id, 1);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Could not add to cart.');
    } finally {
      setIsAdding(false);
    }
  };

  return (
    <div className="group overflow-hidden rounded-lg border border-border bg-surface">
      <div className="relative aspect-square overflow-hidden bg-background">
        {product.tag && (
          <span className="absolute left-2 top-2 z-10 rounded-full bg-primary px-2 py-0.5 text-[10px] font-bold uppercase tracking-widest text-primary-foreground">
            {product.tag}
          </span>
        )}
        <Image
          src={product.imageUrl}
          alt={product.name}
          fill
          sizes="(min-width: 1024px) 25vw, (min-width: 768px) 33vw, 50vw"
          className="object-cover transition-transform group-hover:scale-105"
        />
      </div>
      <div className="p-3">
        <h3 className="text-sm font-semibold">{product.name}</h3>
        <p className="mt-1 text-sm text-muted-foreground">${product.price.toFixed(2)}</p>
        <button
          onClick={handleAddToCart}
          disabled={isAdding || product.stockQuantity === 0}
          className="mt-2 w-full cursor-pointer rounded-md bg-primary py-1.5 text-xs font-bold uppercase tracking-widest text-primary-foreground transition-colors hover:bg-primary/90 disabled:cursor-not-allowed disabled:opacity-50"
        >
          {product.stockQuantity === 0 ? 'Sold Out' : isAdding ? 'Adding...' : 'Add to Cart'}
        </button>
        {error && <p className="mt-1 text-xs text-red-500">{error}</p>}
      </div>
    </div>
  );
};

export default ProductCard;
