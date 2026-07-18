import { apiFetch } from '@/lib/api-client';

export type Product = {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  stockQuantity: number;
  tag: string | null;
  categoryId: number;
  categoryName: string;
  categorySlug: string;
};

export async function getProducts(categorySlug?: string): Promise<Product[]> {
  const query = categorySlug ? `?category=${encodeURIComponent(categorySlug)}` : '';
  return apiFetch<Product[]>(`/products${query}`, { cache: 'no-store' });
}

export async function getProductById(id: number): Promise<Product> {
  return apiFetch<Product>(`/products/${id}`, { cache: 'no-store' });
}
