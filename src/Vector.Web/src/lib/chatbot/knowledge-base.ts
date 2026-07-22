export type FaqEntry = {
  id: string;
  topic: string;
  keywords: string[];
  answer: string;
};

export const knowledgeBase: FaqEntry[] = [
  {
    id: 'shipping',
    topic: 'Shipping & delivery',
    keywords: ['shipping', 'ship', 'delivery', 'deliver', 'arrive', 'when will', 'how long'],
    answer:
      'Standard shipping takes 3-5 business days. You\'ll get a tracking link by email as soon as your order ships.',
  },
  {
    id: 'returns',
    topic: 'Returns & exchanges',
    keywords: ['return', 'refund', 'exchange', 'send back', 'wrong size', 'money back'],
    answer:
      'You can return unworn items within 30 days of delivery for a full refund. Go to Your Orders and select "Start a return" on the order.',
  },
  {
    id: 'sizing',
    topic: 'Sizing guide',
    keywords: ['size', 'sizing', 'fit', 'fits', 'small', 'medium', 'large', 'measurements'],
    answer:
      'Apparel runs true to size. If you\'re between sizes, we\'d recommend sizing up for a more relaxed fit - most of our jerseys and hoodies are cut athletic/slim.',
  },
  {
    id: 'payment',
    topic: 'Payment methods',
    keywords: ['payment', 'pay', 'card', 'paypal', 'credit', 'debit', 'checkout'],
    answer:
      'We accept all major credit/debit cards at checkout. Your total is charged once your order is confirmed.',
  },
  {
    id: 'order-status',
    topic: 'Order status',
    keywords: ['order status', 'track', 'tracking', 'where is my order', 'my order'],
    answer:
      'You can see the status of every order, including items and totals, on the Orders page once you\'re logged in.',
  },
  {
    id: 'account',
    topic: 'Account help',
    keywords: ['account', 'login', 'log in', 'register', 'sign up', 'password', 'forgot'],
    answer:
      'You can create an account from the Register page with just an email and password - no account is required to browse, only to check out.',
  },
  {
    id: 'catalog',
    topic: 'What do you sell?',
    keywords: ['catalog', 'products', 'what do you sell', 'categories', 'jersey', 'hoodie', 'hardware'],
    answer:
      'We carry Pro Kit (match-worn jerseys), Apparel, Hardware (peripherals), Accessories, Bundles, and Legacy drops from past seasons.',
  },
  {
    id: 'discount',
    topic: 'Discount codes',
    keywords: ['discount', 'coupon', 'promo', 'code', 'sale', 'deal'],
    answer:
      "Keep an eye on the homepage - we run drops and mini-games there that unlock discount codes. There's usually one active if you look around.",
  },
  {
    id: 'contact',
    topic: 'Talk to a human',
    keywords: ['human', 'agent', 'person', 'contact', 'email', 'support', 'help me'],
    answer: "I'm just a scripted bot with a fixed set of answers - for anything else, reach out to support@vectorgg.com and a real person will help.",
  },
];

export const fallbackAnswer =
  "I don't have an answer for that one. Try one of the topics below, or email support@vectorgg.com for anything else.";

export const welcomeMessage = "Hey! I'm the VectorGG bot. Ask me about shipping, returns, sizing, or pick a topic below.";
