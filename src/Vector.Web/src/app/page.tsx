import { ArrowRight, RotateCcw, ShieldCheck, Truck } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

import ProductCard from '@/components/product-card';
import SiteLayout from '@/components/site-layout';
import heroBase from '@/images/hero-base.jpg';
import heroProkit from '@/images/hero-prokit.jpg';
import { categories } from '@/lib/categories';
import { products } from '@/lib/products';

const Home = () => {
  return (
    <SiteLayout>
      {/* Category strip */}
      <section className="border-b border-border">
        <div className="mx-auto max-w-7xl px-6 py-8">
          <div className="grid grid-cols-3 gap-3 sm:grid-cols-6">
            {categories.map(({ label, slug, icon: Icon }) => (
              <Link
                key={slug}
                href={`/shop/${slug}`}
                className="group flex flex-col items-center gap-2 rounded-lg border border-transparent p-3 text-center transition-colors hover:border-border hover:bg-surface"
              >
                <div className="grid h-14 w-14 place-items-center rounded-full border border-border bg-surface transition-colors group-hover:border-primary group-hover:text-primary">
                  <Icon className="h-6 w-6" />
                </div>
                <span className="text-xs font-semibold uppercase tracking-wider">{label}</span>
              </Link>
            ))}
          </div>
        </div>
      </section>

      {/* Hero grid */}
      <section className="mx-auto max-w-7xl px-6 py-8">
        <div className="grid gap-5 lg:grid-cols-3">
          {/* Main hero */}
          <div className="group relative overflow-hidden rounded-xl border border-border bg-surface lg:col-span-2">
            <div className="relative aspect-16/10 w-full">
              <Image
                src={heroProkit}
                alt="Pro Kit 2026 Jacket and Joggers"
                fill
                priority
                sizes="(min-width: 1024px) 66vw, 100vw"
                className="object-cover"
              />
              <div className="absolute inset-0 bg-linear-to-r from-background via-background/70 to-transparent" />
            </div>
            <div className="absolute inset-0 flex flex-col justify-between p-6 sm:p-10">
              <div className="flex items-center gap-2 text-xs font-semibold uppercase tracking-[0.25em] text-primary">
                <span className="h-1.5 w-1.5 rounded-full bg-primary" /> New Drop
              </div>
              <div className="max-w-md">
                <h1 className="text-display text-4xl sm:text-6xl">
                  Pro Kit <span className="text-primary">2026</span>
                  <br />
                  Jacket & Joggers
                </h1>
                <p className="mt-3 max-w-sm text-sm text-muted-foreground">
                  Match-worn engineering, built for the arena and the streets.
                </p>
                <Link
                  href="/shop/pro-kit"
                  className="mt-6 inline-flex items-center gap-2 rounded-md bg-primary px-6 py-3 text-sm font-bold uppercase tracking-widest text-primary-foreground transition-transform hover:translate-x-0.5"
                >
                  Shop Now <ArrowRight className="h-4 w-4" />
                </Link>
              </div>
            </div>
          </div>

          {/* Secondary hero */}
          <div className="group relative overflow-hidden rounded-xl border border-border bg-surface">
            <div className="relative aspect-4/5 w-full lg:aspect-auto lg:h-full">
              <Image
                src={heroBase}
                alt="Base collection team"
                fill
                sizes="(min-width: 1024px) 33vw, 100vw"
                className="object-cover"
              />
              <div className="absolute inset-0 bg-linear-to-t from-background via-background/40 to-transparent" />
            </div>
            <div className="absolute inset-0 flex flex-col justify-end p-6">
              <div className="text-xs font-semibold uppercase tracking-[0.25em] text-primary">
                Collection
              </div>
              <h2 className="text-display mt-2 text-2xl sm:text-3xl">
                Base Collection.
                <br />
                <span className="text-primary">Pro Mindset.</span>
              </h2>
              <Link
                href="/shop/apparel"
                className="mt-4 inline-flex w-fit items-center gap-2 rounded-md border border-border bg-background/70 px-4 py-2.5 text-xs font-bold uppercase tracking-widest backdrop-blur transition-colors hover:border-primary hover:text-primary"
              >
                Available Now <ArrowRight className="h-3.5 w-3.5" />
              </Link>
            </div>
          </div>
        </div>
      </section>

      {/* Featured products */}
      <section className="mx-auto max-w-7xl px-6 py-12">
        <div className="mb-8 flex items-end justify-between gap-4">
          <div>
            <div className="text-xs font-semibold uppercase tracking-[0.25em] text-primary">
              Featured
            </div>
            <h2 className="text-display mt-1 text-3xl sm:text-4xl">Trending Gear</h2>
          </div>
          <Link
            href="/shop/pro-kit"
            className="hidden items-center gap-2 text-xs font-bold uppercase tracking-widest text-muted-foreground hover:text-primary sm:inline-flex"
          >
            View All <ArrowRight className="h-3.5 w-3.5" />
          </Link>
        </div>
        <div className="grid grid-cols-2 gap-5 md:grid-cols-3 lg:grid-cols-4">
          {products.slice(0, 8).map(p => (
            <ProductCard key={p.id} product={p} />
          ))}
        </div>
      </section>

      {/* Split banner CTA */}
      <section className="mx-auto max-w-7xl px-6 py-8">
        <div className="grid gap-5 md:grid-cols-2">
          <Link
            href="/shop/apparel"
            className="group flex items-center justify-between rounded-xl border border-border bg-surface px-6 py-8 transition-colors hover:border-primary/60"
          >
            <span className="text-display text-xl sm:text-2xl">Shop All Apparel</span>
            <ArrowRight className="h-5 w-5 text-muted-foreground transition-transform group-hover:translate-x-1 group-hover:text-primary" />
          </Link>
          <Link
            href="/shop/hardware"
            className="group flex items-center justify-between rounded-xl border border-border bg-surface px-6 py-8 transition-colors hover:border-primary/60"
          >
            <span className="text-display text-xl sm:text-2xl">Shop All Hardware</span>
            <ArrowRight className="h-5 w-5 text-muted-foreground transition-transform group-hover:translate-x-1 group-hover:text-primary" />
          </Link>
        </div>
      </section>

      {/* Value props */}
      <section className="border-t border-border">
        <div className="mx-auto grid max-w-7xl gap-8 px-6 py-12 md:grid-cols-3">
          {[
            {
              icon: Truck,
              title: 'Free Shipping',
              body: 'Free worldwide delivery on orders over €150. Fast dispatch within 24h.',
            },
            {
              icon: ShieldCheck,
              title: 'Secure Checkout',
              body: 'Encrypted payments and buyer protection on every order, guaranteed.',
            },
            {
              icon: RotateCcw,
              title: '14-Day Returns',
              body: 'Not happy with the fit? Return unworn items within 14 days for a refund.',
            },
          ].map(({ icon: Icon, title, body }) => (
            <div key={title} className="flex gap-4">
              <div className="grid h-12 w-12 shrink-0 place-items-center rounded-full bg-primary/10 text-primary">
                <Icon className="h-5 w-5" />
              </div>
              <div className="min-w-0">
                <h3 className="text-display text-lg">{title}</h3>
                <p className="mt-1 text-sm text-muted-foreground">{body}</p>
              </div>
            </div>
          ))}
        </div>
      </section>
    </SiteLayout>
  );
};

export default Home;
