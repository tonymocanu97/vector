import { Backpack, Cpu, Gift, Package, Shirt, Trophy, type LucideIcon } from 'lucide-react';

export type CategorySlug = 'pro-kit' | 'apparel' | 'hardware' | 'accessories' | 'bundles' | 'legacy';

export type CategoryDef = {
  slug: CategorySlug;
  label: string;
  matches: string;
  icon: LucideIcon;
  tagline: string;
  description: string;
};

export const categories: CategoryDef[] = [
  {
    slug: 'pro-kit',
    label: 'Pro Kit',
    matches: 'Pro Kit',
    icon: Trophy,
    tagline: 'Match-Worn Engineering',
    description:
      'Official 2026 player jerseys, jackets and training gear — engineered for the arena, cut for the streets.',
  },
  {
    slug: 'apparel',
    label: 'Apparel',
    matches: 'Apparel',
    icon: Shirt,
    tagline: 'Off-Duty Uniform',
    description:
      'Blackout hoodies, graphic tees and caps built from premium fabrics with a competitive edge.',
  },
  {
    slug: 'hardware',
    label: 'Hardware',
    matches: 'Hardware',
    icon: Cpu,
    tagline: 'Precision Gear',
    description:
      'Mousepads, peripherals and pro-tuned accessories — the exact loadout our players use on LAN.',
  },
  {
    slug: 'accessories',
    label: 'Accessories',
    matches: 'Accessories',
    icon: Backpack,
    tagline: 'Everyday Loadout',
    description:
      'Backpacks, headwear and small goods to complete the kit — engineered to move with you.',
  },
  {
    slug: 'bundles',
    label: 'Bundles',
    matches: 'Bundles',
    icon: Package,
    tagline: 'Curated Drops',
    description:
      'Hand-picked bundles that save you more when you kit up head to toe. New drops rotating in soon.',
  },
  {
    slug: 'legacy',
    label: 'Legacy',
    matches: 'Legacy',
    icon: Gift,
    tagline: 'Vault Archive',
    description:
      'Retro jerseys, championship editions and pieces from our archive — while supplies last.',
  },
];

export const getCategory = (slug: string): CategoryDef | undefined =>
  categories.find(c => c.slug === slug);
