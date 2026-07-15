import { Handshake, Newspaper, Trophy, Users, type LucideIcon } from 'lucide-react';

export type TeamSlug = 'esports' | 'players' | 'partners' | 'news';

export type TeamSection = {
  slug: TeamSlug;
  label: string;
  icon: LucideIcon;
  tagline: string;
  description: string;
  highlights: { title: string; body: string; meta?: string }[];
};

export const teamSections: TeamSection[] = [
  {
    slug: 'esports',
    label: 'Esports',
    icon: Trophy,
    tagline: 'Competitive Divisions',
    description:
      'Six active rosters, one banner. Meet the divisions carrying VectorGG across every major circuit.',
    highlights: [
      {
        title: 'Counter-Strike 2',
        meta: 'Tier 1 · EU',
        body: 'Back-to-back BLAST Spring finalists chasing a first Major lift.',
      },
      {
        title: 'Valorant',
        meta: 'VCT EMEA',
        body: 'A young core built around aggressive mid-round reads and hard flanks.',
      },
      {
        title: 'League of Legends',
        meta: 'LEC',
        body: 'Playoff regulars with a bot lane duo drafted first ten weeks running.',
      },
      {
        title: 'Rocket League',
        meta: 'RLCS EU',
        body: 'The most decorated 3-stack in our history — three World finals appearances.',
      },
      {
        title: 'Apex Legends',
        meta: 'ALGS EMEA',
        body: 'Rookie roster with the highest average placement in the region this split.',
      },
      {
        title: 'Fighting Games',
        meta: 'Global',
        body: 'Solo pros repping Street Fighter 6 and Tekken 8 on the majors circuit.',
      },
    ],
  },
  {
    slug: 'players',
    label: 'Players',
    icon: Users,
    tagline: 'Active Roster',
    description:
      'The competitors who wear the kit. From veterans with silverware to academy call-ups earning their first LAN.',
    highlights: [
      {
        title: 'vex',
        meta: 'CS2 · IGL',
        body: 'Six years on the active roster. Reads the mid-round better than anyone in the league.',
      },
      {
        title: 'nyra',
        meta: 'Valorant · Duelist',
        body: 'Signed from academy last summer. Top-3 first-blood rate across VCT EMEA.',
      },
      {
        title: 'kairo',
        meta: 'LoL · AD Carry',
        body: "Rookie of the split. Coldest late-game teamfighter we've fielded in years.",
      },
      {
        title: 'sable',
        meta: 'Rocket League · Captain',
        body: 'Two-time World finalist. Runs the calmest comms channel in the org.',
      },
      {
        title: 'orbit',
        meta: 'Apex · Fragger',
        body: "Highest damage-per-game in ALGS EMEA. Team's designated first contact.",
      },
      {
        title: 'flint',
        meta: 'SF6 · Solo',
        body: 'Evo top 8. Won three regional majors in the last twelve months.',
      },
    ],
  },
  {
    slug: 'partners',
    label: 'Partners',
    icon: Handshake,
    tagline: 'Official Sponsors',
    description:
      'The brands funding the training houses, the boot camps and the flights. Long-term partners only.',
    highlights: [
      {
        title: 'Halcyon Peripherals',
        meta: 'Hardware',
        body: 'Custom-tuned mice and keyboards used by every CS2 and Valorant player on the roster.',
      },
      {
        title: 'Northwind Energy',
        meta: 'Nutrition',
        body: 'Zero-sugar performance drinks fuelling every scrim block and LAN weekend.',
      },
      {
        title: 'Vault Financial',
        meta: 'Fintech',
        body: 'Player banking, prize payouts and long-term financial planning for signed pros.',
      },
      {
        title: 'Ridgeline Apparel',
        meta: 'Fabric',
        body: 'The mill behind our jerseys and pro kit — moisture-wicking blends spun in Portugal.',
      },
      {
        title: 'Frame & Fibre',
        meta: 'Broadcast',
        body: 'Full-stack production partner for our watch parties and player documentaries.',
      },
      {
        title: 'Signal ISP',
        meta: 'Connectivity',
        body: "Dedicated low-latency lines into both training facilities and every player's home setup.",
      },
    ],
  },
  {
    slug: 'news',
    label: 'News',
    icon: Newspaper,
    tagline: 'Latest Updates',
    description:
      'Signings, results and behind-the-scenes drops. Everything moving inside the org, straight from the source.',
    highlights: [
      {
        title: 'vex re-signs through 2028',
        meta: '3 days ago',
        body: 'Our CS2 IGL commits to a two-year extension after a career-best playoffs run.',
      },
      {
        title: 'Pro Kit 2026 lands worldwide',
        meta: '1 week ago',
        body: 'The new match-worn jersey and training set is live in the store. Shipping now.',
      },
      {
        title: 'VCT EMEA week 4 recap',
        meta: '1 week ago',
        body: "Nyra's 41-kill duel keeps our playoff bid alive after a 2-1 series win.",
      },
      {
        title: 'New training facility opens in Berlin',
        meta: '3 weeks ago',
        body: 'Two-floor boot-camp space with dedicated scrim rooms for every division.',
      },
      {
        title: 'Rocket League World finals bound',
        meta: '1 month ago',
        body: "Sable's roster qualifies for a third consecutive World Championship.",
      },
      {
        title: 'Academy program applications open',
        meta: '1 month ago',
        body: 'Six trainee slots across CS2, Valorant and Apex. Applications close end of month.',
      },
    ],
  },
];

export const getTeamSection = (slug: string): TeamSection | undefined =>
  teamSections.find(s => s.slug === slug);
