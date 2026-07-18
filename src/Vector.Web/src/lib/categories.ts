import { Backpack, Cpu, Gift, Package, Shirt, Trophy, type LucideIcon } from 'lucide-react';

import { apiFetch } from '@/lib/api-client';

export type Category = {
  id: number;
  name: string;
  slug: string;
  description: string;
};

const categoryIcons: Record<string, LucideIcon> = {
  'pro-kit': Trophy,
  apparel: Shirt,
  hardware: Cpu,
  accessories: Backpack,
  bundles: Package,
  legacy: Gift,
};

export const getCategoryIcon = (slug: string): LucideIcon => categoryIcons[slug] ?? Package;

export async function getCategories(): Promise<Category[]> {
  return apiFetch<Category[]>('/categories', { cache: 'no-store' });
}

export async function getCategoryBySlug(slug: string): Promise<Category | undefined> {
  const categories = await getCategories();
  return categories.find(c => c.slug === slug);
}
