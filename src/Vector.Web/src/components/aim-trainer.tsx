'use client';

import { Crosshair, X } from 'lucide-react';
import { useCallback, useEffect, useRef, useState } from 'react';

const GAME_DURATION_SECONDS = 10;
const HITS_TO_WIN = 10;
const DISCOUNT_CODE = 'ACES10';

type GameState = 'idle' | 'playing' | 'won' | 'lost';

const AimTrainer = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [gameState, setGameState] = useState<GameState>('idle');
  const [score, setScore] = useState(0);
  const [timeLeft, setTimeLeft] = useState(GAME_DURATION_SECONDS);
  const [target, setTarget] = useState({ x: 50, y: 50 });
  const [copied, setCopied] = useState(false);
  const intervalRef = useRef<ReturnType<typeof setInterval> | null>(null);

  const randomizeTarget = useCallback(() => {
    setTarget({ x: Math.random() * 80 + 10, y: Math.random() * 80 + 10 });
  }, []);

  const startGame = () => {
    setScore(0);
    setTimeLeft(GAME_DURATION_SECONDS);
    setCopied(false);
    randomizeTarget();
    setGameState('playing');
  };

  useEffect(() => {
    if (gameState !== 'playing') return;

    intervalRef.current = setInterval(() => {
      setTimeLeft(prev => (prev <= 1 ? 0 : prev - 1));
    }, 1000);

    return () => {
      if (intervalRef.current) clearInterval(intervalRef.current);
    };
  }, [gameState]);

  useEffect(() => {
    if (gameState === 'playing' && timeLeft === 0) {
      setGameState(score >= HITS_TO_WIN ? 'won' : 'lost');
    }
  }, [timeLeft, gameState, score]);

  const handleHit = () => {
    setScore(s => s + 1);
    randomizeTarget();
  };

  const handleClose = () => {
    if (intervalRef.current) clearInterval(intervalRef.current);
    setIsOpen(false);
    setGameState('idle');
  };

  const handleCopy = () => {
    navigator.clipboard.writeText(DISCOUNT_CODE);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <>
      <button
        onClick={() => setIsOpen(true)}
        className="inline-flex cursor-pointer items-center gap-2 text-xs font-bold uppercase tracking-widest text-muted-foreground transition-colors hover:text-primary"
      >
        <Crosshair className="h-3.5 w-3.5" />
        Feeling confident? Test your aim for a discount
      </button>

      {isOpen && (
        <div
          onClick={handleClose}
          className="fixed inset-0 z-50 flex items-center justify-center bg-black/70 px-4"
        >
          <div
            onClick={e => e.stopPropagation()}
            className="w-full max-w-lg rounded-xl border border-border bg-surface p-6"
          >
            <div className="flex items-center justify-between">
              <h3 className="text-display text-xl">
                {gameState === 'idle' && 'Aim Trainer'}
                {gameState === 'playing' && `Time: ${timeLeft}s · Hits: ${score}`}
                {gameState === 'won' && 'Target Locked.'}
                {gameState === 'lost' && 'So Close.'}
              </h3>
              <button
                onClick={handleClose}
                aria-label="Close"
                className="grid h-8 w-8 shrink-0 cursor-pointer place-items-center rounded-md border border-border hover:border-primary/60 hover:text-primary"
              >
                <X className="h-4 w-4" />
              </button>
            </div>

            {gameState === 'idle' && (
              <div className="mt-6 text-center">
                <p className="text-sm text-muted-foreground">
                  Hit {HITS_TO_WIN} targets in {GAME_DURATION_SECONDS} seconds to unlock a discount
                  code.
                </p>
                <button
                  onClick={startGame}
                  className="mt-6 cursor-pointer rounded-md bg-primary px-6 py-3 text-sm font-bold uppercase tracking-widest text-primary-foreground transition-colors hover:bg-primary/90"
                >
                  Start
                </button>
              </div>
            )}

            {gameState === 'playing' && (
              <div className="relative mt-4 h-72 overflow-hidden rounded-lg border border-border bg-background">
                <button
                  onClick={handleHit}
                  aria-label="Target"
                  style={{ top: `${target.y}%`, left: `${target.x}%` }}
                  className="absolute grid h-12 w-12 -translate-x-1/2 -translate-y-1/2 cursor-pointer place-items-center rounded-full bg-primary text-primary-foreground shadow-[0_0_20px_rgba(255,103,23,0.6)] transition-transform hover:scale-95"
                >
                  <Crosshair className="h-5 w-5" />
                </button>
              </div>
            )}

            {gameState === 'won' && (
              <div className="mt-6 text-center">
                <p className="text-sm text-muted-foreground">{score} hits. Here&apos;s your code:</p>
                <div className="mt-4 flex items-center justify-center gap-3">
                  <span className="text-display rounded-md border border-primary/60 bg-primary/10 px-4 py-2 text-2xl text-primary">
                    {DISCOUNT_CODE}
                  </span>
                  <button
                    onClick={handleCopy}
                    className="cursor-pointer rounded-md border border-border px-4 py-2 text-xs font-bold uppercase tracking-widest hover:border-primary hover:text-primary"
                  >
                    {copied ? 'Copied!' : 'Copy'}
                  </button>
                </div>
              </div>
            )}

            {gameState === 'lost' && (
              <div className="mt-6 text-center">
                <p className="text-sm text-muted-foreground">
                  {score} hits - needed {HITS_TO_WIN}. Give it another shot.
                </p>
                <button
                  onClick={startGame}
                  className="mt-6 cursor-pointer rounded-md bg-primary px-6 py-3 text-sm font-bold uppercase tracking-widest text-primary-foreground transition-colors hover:bg-primary/90"
                >
                  Retry
                </button>
              </div>
            )}
          </div>
        </div>
      )}
    </>
  );
};

export default AimTrainer;
