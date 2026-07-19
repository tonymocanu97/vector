import Link from 'next/link';
import { notFound } from 'next/navigation';

import ProductCard from '@/components/product-card';
import { getCategories, getCategoryBySlug } from '@/lib/categories';
import { getProducts } from '@/lib/products';

type Props = { params: Promise<{ category: string }> };

const CategoryPage = async ({ params }: Props) => {
  const { category: slug } = await params;
  const [category, categories] = await Promise.all([getCategoryBySlug(slug), getCategories()]);

  if (!category) notFound();

  const items = await getProducts(slug);

  return (
    <>
      {/* Category chips */}
      <section>
        <div className="mx-auto flex max-w-7xl flex-wrap gap-2 px-6 py-8 sm:px-10 sm:py-10">
          {categories.map(c => {
            const active = c.slug === category.slug;
            return (
              <Link
                key={c.slug}
                href={`/shop/${c.slug}`}
                className={`rounded-full border px-4 py-1.5 text-xs font-bold uppercase tracking-widest transition-colors ${
                  active
                    ? 'border-primary bg-primary text-primary-foreground'
                    : 'border-border text-muted-foreground hover:border-primary hover:text-primary'
                }`}
              >
                {c.name}
              </Link>
            );
          })}
        </div>
      </section>

      {/* Products */}
      <section className="mx-auto max-w-7xl px-6 pb-24 sm:px-10">
        {items.length === 0 && (
          <div className="mx-auto mb-10 max-w-xl rounded-xl border border-dashed border-border bg-surface p-10 text-center">
            <div className="text-xs font-semibold uppercase tracking-[0.25em] text-primary">
              Dropping soon
            </div>
            <h2 className="text-display mt-2 text-2xl">The {category.name} vault is loading.</h2>
            <p className="mt-2 text-sm text-muted-foreground">
              We&apos;re finalising this collection. Check back shortly - or browse what&apos;s
              live now.
            </p>
            <Link
              href="/"
              className="mt-6 inline-flex items-center gap-2 rounded-md bg-primary px-5 py-2.5 text-sm font-bold uppercase tracking-widest text-primary-foreground"
            >
              Back to Shop
            </Link>
          </div>
        )}
        <div className="grid grid-cols-2 gap-5 md:grid-cols-3 lg:grid-cols-4">
          {items.map(p => (
            <ProductCard key={p.id} product={p} />
          ))}
          {Array.from({
            length: items.length === 0 ? 8 : ((4 - (items.length % 4)) % 4) + 4,
          }).map((_, i) => (
            <div key={`ph-${i}`} aria-hidden className="aspect-4/5" />
          ))}
        </div>
      </section>
    </>
  );
};

export default CategoryPage;
