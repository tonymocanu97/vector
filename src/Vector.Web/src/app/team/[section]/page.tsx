import { notFound } from 'next/navigation';
import Link from 'next/link';

import { getTeamSection, teamSections } from '@/lib/team';

type Props = { params: Promise<{ section: string }> };

const TeamSectionPage = async ({ params }: Props) => {
  const { section: slug } = await params;
  const section = getTeamSection(slug);

  if (!section) notFound();

  const Icon = section.icon;

  return (
    <>
      {/* Team chips */}
      <section>
        <div className="mx-auto flex max-w-7xl flex-wrap gap-2 px-6 py-8 sm:px-10 sm:py-10">
          {teamSections.map(s => {
            const active = s.slug === section.slug;
            const ChipIcon = s.icon;
            return (
              <Link
                key={s.slug}
                href={`/team/${s.slug}`}
                className={`inline-flex items-center gap-2 rounded-full border px-4 py-1.5 text-xs font-bold uppercase tracking-widest transition-colors ${
                  active
                    ? 'border-primary bg-primary text-primary-foreground'
                    : 'border-border text-muted-foreground hover:border-primary hover:text-primary'
                }`}
              >
                <ChipIcon className="h-3.5 w-3.5" />
                {s.label}
              </Link>
            );
          })}
        </div>
      </section>

      {/* Section header */}
      <section className="mx-auto max-w-7xl px-6 sm:px-10">
        <div className="flex items-start gap-5">
          <div className="grid h-16 w-16 shrink-0 place-items-center rounded-xl border border-border bg-surface text-primary">
            <Icon className="h-7 w-7" />
          </div>
          <div className="min-w-0">
            <div className="text-xs font-semibold uppercase tracking-[0.25em] text-primary">
              {section.tagline}
            </div>
            <h1 className="text-display mt-1 text-3xl sm:text-4xl">{section.label}</h1>
            <p className="mt-2 max-w-2xl text-sm text-muted-foreground">{section.description}</p>
          </div>
        </div>
      </section>

      {/* Highlights grid */}
      <section className="mx-auto max-w-7xl px-6 pb-24 pt-10 sm:px-10">
        <div className="grid grid-cols-1 gap-5 md:grid-cols-2 lg:grid-cols-3">
          {section.highlights.map(h => (
            <article
              key={h.title}
              className="group flex flex-col rounded-lg border border-border bg-surface p-5 transition-colors hover:border-primary/60"
            >
              {h.meta && (
                <div className="text-[10px] font-bold uppercase tracking-[0.25em] text-primary">
                  {h.meta}
                </div>
              )}
              <h2 className="text-display mt-2 text-xl group-hover:text-primary">{h.title}</h2>
              <p className="mt-2 text-sm text-muted-foreground">{h.body}</p>
            </article>
          ))}
          {Array.from({
            length: ((3 - (section.highlights.length % 3)) % 3) + 3,
          }).map((_, i) => (
            <div key={`ph-${i}`} aria-hidden className="h-40" />
          ))}
        </div>
      </section>
    </>
  );
};

export default TeamSectionPage;
