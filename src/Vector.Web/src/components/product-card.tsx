import Image from 'next/image';

import type { Product } from '@/lib/products';

const ProductCard = ({ product }: { product: Product }) => {
  return (
    <div className="group overflow-hidden rounded-lg border border-border bg-surface">
      <div className="relative aspect-square overflow-hidden bg-background">
        {product.tag && (
          <span className="absolute left-2 top-2 z-10 rounded-full bg-primary px-2 py-0.5 text-[10px] font-bold uppercase tracking-widest text-primary-foreground">
            {product.tag}
          </span>
        )}
        <Image
          src={product.image}
          alt={product.name}
          fill
          sizes="(min-width: 1024px) 25vw, (min-width: 768px) 33vw, 50vw"
          className="object-cover transition-transform group-hover:scale-105"
        />
      </div>
      <div className="p-3">
        <h3 className="text-sm font-semibold">{product.name}</h3>
        <p className="mt-1 text-sm text-muted-foreground">${product.price.toFixed(2)}</p>
      </div>
    </div>
  );
};

export default ProductCard;
