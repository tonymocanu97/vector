import type { StaticImageData } from 'next/image';

import backpack from '@/images/product-backpack.jpg';
import cap from '@/images/product-cap.jpg';
import hoodie from '@/images/product-hoodie.jpg';
import jersey from '@/images/product-jersey.jpg';
import joggers from '@/images/product-joggers.jpg';
import mousepad from '@/images/product-mousepad.jpg';
import tee from '@/images/product-tee.jpg';

export type Product = {
  id: string;
  name: string;
  category: string;
  price: number;
  image: StaticImageData;
  tag?: 'New' | 'Bestseller' | 'Limited' | 'Sold Out';
};

export const products: Product[] = [
  {
    id: 'pro-jersey-26',
    name: 'Pro Kit Jersey 2026',
    category: 'Pro Kit',
    price: 74.99,
    image: jersey,
    tag: 'New',
  },
  {
    id: 'core-hoodie',
    name: 'Core Blackout Hoodie',
    category: 'Apparel',
    price: 89.99,
    image: hoodie,
    tag: 'Bestseller',
  },
  {
    id: 'gaming-mousepad-xl',
    name: 'Arena Mousepad XL',
    category: 'Hardware',
    price: 34.99,
    image: mousepad,
  },
  { id: 'logo-cap', name: 'Emblem Snap-Back Cap', category: 'Apparel', price: 29.99, image: cap },
  {
    id: 'graphic-tee',
    name: 'Signal Graphic Tee',
    category: 'Apparel',
    price: 39.99,
    image: tee,
    tag: 'New',
  },
  {
    id: 'track-joggers',
    name: 'Pro Track Joggers',
    category: 'Pro Kit',
    price: 84.99,
    image: joggers,
  },
  {
    id: 'orange-backpack',
    name: 'Loadout Backpack 22L',
    category: 'Accessories',
    price: 119.99,
    image: backpack,
    tag: 'Limited',
  },
  { id: 'team-hoodie-2', name: 'Team Zip Hoodie', category: 'Apparel', price: 99.99, image: hoodie },
];
