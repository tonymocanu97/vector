import { type FaqEntry } from '@/lib/chatbot/knowledge-base';

export function findBestMatch(input: string, entries: FaqEntry[]): FaqEntry | null {
  const normalized = input.toLowerCase();

  let best: FaqEntry | null = null;
  let bestScore = 0;

  for (const entry of entries) {
    const score = entry.keywords.reduce((count, keyword) => count + (normalized.includes(keyword) ? 1 : 0), 0);
    if (score > bestScore) {
      bestScore = score;
      best = entry;
    }
  }

  return best;
}
