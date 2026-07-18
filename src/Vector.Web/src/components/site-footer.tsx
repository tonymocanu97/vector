'use client';

import { useState, type FormEvent } from 'react';
import { FaGithub, FaInstagram, FaTwitch, FaXTwitter, FaYoutube } from 'react-icons/fa6';

import { apiFetch } from '@/lib/api-client';

const SiteFooter = () => {
  const [email, setEmail] = useState('');
  const [status, setStatus] = useState<'idle' | 'submitting' | 'success' | 'error'>('idle');
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setStatus('submitting');
    setError(null);

    try {
      await apiFetch('/newsletter/subscribe', { method: 'POST', body: { email } });
      setStatus('success');
      setEmail('');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Something went wrong.');
      setStatus('error');
    }
  };

  return (
    <footer className="border-t border-border bg-surface">
      {/* Newsletter */}
      <section className="border-b border-border">
        <div className="mx-auto max-w-7xl px-6 py-14">
          <div className="grid gap-8 md:grid-cols-2 md:items-center">
            <div>
              <h3 className="text-display text-3xl md:text-4xl">
                Join the <span className="text-primary">Squad</span>
              </h3>
              <p className="mt-2 max-w-md text-sm text-muted-foreground">
                Get first access to drops, pro-kit releases and exclusive member-only gear.
              </p>
            </div>
            <form onSubmit={handleSubmit} className="flex flex-col gap-3 sm:flex-row">
              <input
                type="email"
                required
                placeholder="Your email address"
                value={email}
                onChange={e => setEmail(e.target.value)}
                disabled={status === 'submitting' || status === 'success'}
                className="h-12 flex-1 rounded-md border border-border bg-background px-4 text-sm outline-none focus:border-primary disabled:opacity-50"
              />
              <button
                type="submit"
                disabled={status === 'submitting' || status === 'success'}
                className="h-12 cursor-pointer rounded-md bg-primary px-6 text-sm font-bold uppercase tracking-wider text-primary-foreground transition-colors hover:bg-primary/90 disabled:cursor-not-allowed disabled:opacity-50"
              >
                {status === 'submitting' ? 'Signing Up...' : status === 'success' ? 'Subscribed!' : 'Sign Up'}
              </button>
            </form>
            {status === 'error' && <p className="mt-2 text-xs text-red-500">{error}</p>}
          </div>
        </div>
      </section>

      {/* Links */}
      <div className="mx-auto max-w-7xl px-6 py-12">
        <div className="grid gap-10 md:grid-cols-4">
          <div>
            <div className="font-display flex items-center gap-2 text-xl font-black uppercase">
              <div className="grid h-8 w-8 place-items-center rounded-md bg-primary text-primary-foreground">
                V
              </div>
              Vector<span className="text-primary">GG</span>
            </div>
            <p className="mt-3 text-sm text-muted-foreground">
              Professional esports merch, forged for competitors and fans.
            </p>
            <div className="mt-5 flex gap-2">
              {[FaXTwitter, FaInstagram, FaYoutube, FaTwitch, FaGithub].map((Icon, i) => (
                <a
                  key={i}
                  href="#"
                  className="grid h-9 w-9 place-items-center rounded-md border border-border text-muted-foreground transition-colors hover:border-primary hover:text-primary"
                >
                  <Icon className="h-4 w-4" />
                </a>
              ))}
            </div>
          </div>

          {[
            { title: 'Shop', items: ['Pro Kit', 'Apparel', 'Hardware', 'Accessories', 'Bundles'] },
            { title: 'About', items: ['Our Story', 'Teams', 'Careers', 'Press'] },
            { title: 'Help', items: ['Contact', 'Shipping', 'Returns', 'Size Guide', 'FAQ'] },
          ].map(col => (
            <div key={col.title}>
              <h4 className="mb-4 text-xs font-semibold uppercase tracking-[0.2em] text-muted-foreground">
                {col.title}
              </h4>
              <ul className="space-y-2 text-sm">
                {col.items.map(i => (
                  <li key={i}>
                    <a href="#" className="text-foreground/80 hover:text-primary">
                      {i}
                    </a>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>

        <div className="mt-10 flex flex-col justify-between gap-4 border-t border-border pt-6 text-xs text-muted-foreground sm:flex-row">
          <div>© 2026 VectorGG. All rights reserved.</div>
          <div className="flex gap-4">
            <a href="#" className="hover:text-primary">
              Terms
            </a>
            <a href="#" className="hover:text-primary">
              Privacy
            </a>
            <a href="#" className="hover:text-primary">
              Cookies
            </a>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default SiteFooter;
