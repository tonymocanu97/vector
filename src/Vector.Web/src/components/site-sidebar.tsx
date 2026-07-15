'use client';

import { Home, Search, ShoppingBag, User } from 'lucide-react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

import { categories } from '@/lib/categories';
import { teamSections } from '@/lib/team';

const SiteSidebar = () => {
  const pathname = usePathname();

  return (
    <aside className="fixed inset-y-0 left-0 z-40 hidden w-64 flex-col border-r border-border bg-surface lg:flex">
      {/* Logo */}
      <div className="flex h-20 items-center gap-3 border-b border-border px-6">
        <div className="font-display grid h-10 w-10 place-items-center rounded-md bg-primary text-xl font-black text-primary-foreground">
          V
        </div>
        <div className="font-display text-xl font-black uppercase tracking-wider">
          Vector<span className="text-primary">GG</span>
        </div>
      </div>

      {/* Search */}
      <div className="px-4 pt-5">
        <label className="flex items-center gap-2 rounded-md border border-border bg-background px-3 py-2 text-sm text-muted-foreground focus-within:border-primary/60">
          <Search className="h-4 w-4" />
          <input
            type="search"
            placeholder="Search merch..."
            className="w-full bg-transparent text-foreground placeholder:text-muted-foreground focus:outline-none"
          />
        </label>
      </div>

      {/* Nav */}
      <nav className="flex-1 overflow-y-auto px-3 py-6">
        <div className="mb-2 px-3 text-[10px] font-semibold uppercase tracking-[0.2em] text-muted-foreground">
          Shop
        </div>
        <ul className="space-y-1">
          <li>
            <Link
              href="/"
              className={`group flex items-center gap-3 rounded-md px-3 py-2.5 text-sm font-medium transition-colors ${
                pathname === '/'
                  ? 'bg-primary/10 text-primary'
                  : 'text-foreground/80 hover:bg-surface-elevated hover:text-foreground'
              }`}
            >
              <Home className="h-4 w-4 shrink-0" />
              <span className="truncate">Shop</span>
              {pathname === '/' && (
                <span className="ml-auto h-1.5 w-1.5 rounded-full bg-primary" />
              )}
            </Link>
          </li>
          {categories.map(item => {
            const Icon = item.icon;
            const active = pathname === `/shop/${item.slug}`;
            return (
              <li key={item.slug}>
                <Link
                  href={`/shop/${item.slug}`}
                  className={`group flex items-center gap-3 rounded-md px-3 py-2.5 text-sm font-medium transition-colors ${
                    active
                      ? 'bg-primary/10 text-primary'
                      : 'text-foreground/80 hover:bg-surface-elevated hover:text-foreground'
                  }`}
                >
                  <Icon className="h-4 w-4 shrink-0" />
                  <span className="truncate">{item.label}</span>
                  {active && <span className="ml-auto h-1.5 w-1.5 rounded-full bg-primary" />}
                </Link>
              </li>
            );
          })}
        </ul>

        <div className="mb-2 mt-8 px-3 text-[10px] font-semibold uppercase tracking-[0.2em] text-muted-foreground">
          Team
        </div>
        <ul className="space-y-1">
          {teamSections.map(item => {
            const Icon = item.icon;
            const active = pathname === `/team/${item.slug}`;
            return (
              <li key={item.slug}>
                <Link
                  href={`/team/${item.slug}`}
                  className={`group flex items-center gap-3 rounded-md px-3 py-2.5 text-sm font-medium transition-colors ${
                    active
                      ? 'bg-primary/10 text-primary'
                      : 'text-foreground/80 hover:bg-surface-elevated hover:text-foreground'
                  }`}
                >
                  <Icon className="h-4 w-4 shrink-0" />
                  <span className="truncate">{item.label}</span>
                  {active && <span className="ml-auto h-1.5 w-1.5 rounded-full bg-primary" />}
                </Link>
              </li>
            );
          })}
        </ul>
      </nav>

      {/* Footer actions */}
      <div className="border-t border-border p-4">
        <div className="flex items-center gap-2">
          <button className="flex flex-1 items-center justify-center gap-2 rounded-md border border-border bg-background px-3 py-2 text-sm font-medium hover:border-primary/60 hover:text-primary">
            <User className="h-4 w-4" /> Log In
          </button>
          <button
            aria-label="Cart"
            className="relative grid h-9 w-9 place-items-center rounded-md border border-border bg-background hover:border-primary/60 hover:text-primary"
          >
            <ShoppingBag className="h-4 w-4" />
            <span className="absolute -top-1.5 -right-1.5 grid h-4 min-w-4 place-items-center rounded-full bg-primary px-1 text-[10px] font-bold text-primary-foreground">
              2
            </span>
          </button>
        </div>
      </div>
    </aside>
  );
}

export default SiteSidebar;
