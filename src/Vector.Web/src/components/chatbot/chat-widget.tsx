'use client';

import { Bot, MessageCircle, Send, X } from 'lucide-react';
import { useEffect, useRef, useState } from 'react';

import { fallbackAnswer, knowledgeBase, welcomeMessage } from '@/lib/chatbot/knowledge-base';
import { findBestMatch } from '@/lib/chatbot/match';

type Message = {
  id: string;
  sender: 'bot' | 'user';
  text: string;
};

const TYPING_DELAY_MS = 500;

const makeId = () => `${Date.now()}-${Math.random().toString(36).slice(2, 8)}`;

const ChatWidget = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [messages, setMessages] = useState<Message[]>([{ id: makeId(), sender: 'bot', text: welcomeMessage }]);
  const [draft, setDraft] = useState('');
  const [isTyping, setIsTyping] = useState(false);
  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    scrollRef.current?.scrollTo({ top: scrollRef.current.scrollHeight, behavior: 'smooth' });
  }, [messages, isTyping]);

  const respond = (userText: string) => {
    setMessages(prev => [...prev, { id: makeId(), sender: 'user', text: userText }]);
    setIsTyping(true);

    setTimeout(() => {
      const match = findBestMatch(userText, knowledgeBase);
      setMessages(prev => [...prev, { id: makeId(), sender: 'bot', text: match?.answer ?? fallbackAnswer }]);
      setIsTyping(false);
    }, TYPING_DELAY_MS);
  };

  const handleSend = () => {
    const trimmed = draft.trim();
    if (!trimmed) return;
    setDraft('');
    respond(trimmed);
  };

  const handleTopicClick = (topic: string) => {
    respond(topic);
  };

  return (
    <div className="fixed bottom-6 right-6 z-50">
      {isOpen && (
        <div className="mb-4 flex h-[28rem] w-[22rem] max-w-[calc(100vw-3rem)] flex-col rounded-xl border border-border bg-surface shadow-card">
          <div className="flex items-center justify-between border-b border-border px-4 py-3">
            <div className="flex items-center gap-2">
              <span className="grid h-8 w-8 place-items-center rounded-full bg-primary/10 text-primary">
                <Bot className="h-4 w-4" />
              </span>
              <span className="text-sm font-bold uppercase tracking-widest">VectorGG Bot</span>
            </div>
            <button
              onClick={() => setIsOpen(false)}
              aria-label="Close chat"
              className="grid h-8 w-8 cursor-pointer place-items-center rounded-md border border-border hover:border-primary/60 hover:text-primary"
            >
              <X className="h-4 w-4" />
            </button>
          </div>

          <div ref={scrollRef} className="flex-1 space-y-3 overflow-y-auto px-4 py-3">
            {messages.map(message => (
              <div key={message.id} className={`flex ${message.sender === 'user' ? 'justify-end' : 'justify-start'}`}>
                <p
                  className={`max-w-[85%] rounded-lg px-3 py-2 text-sm ${
                    message.sender === 'user'
                      ? 'bg-primary text-primary-foreground'
                      : 'bg-background text-foreground'
                  }`}
                >
                  {message.text}
                </p>
              </div>
            ))}

            {isTyping && (
              <div className="flex justify-start">
                <p className="rounded-lg bg-background px-3 py-2 text-sm text-muted-foreground">...</p>
              </div>
            )}

            {!isTyping && (
              <div className="flex flex-wrap gap-2 pt-1">
                {knowledgeBase.map(entry => (
                  <button
                    key={entry.id}
                    onClick={() => handleTopicClick(entry.topic)}
                    className="cursor-pointer rounded-full border border-border px-3 py-1 text-xs text-muted-foreground transition-colors hover:border-primary/60 hover:text-primary"
                  >
                    {entry.topic}
                  </button>
                ))}
              </div>
            )}
          </div>

          <div className="flex items-center gap-2 border-t border-border p-3">
            <input
              value={draft}
              onChange={e => setDraft(e.target.value)}
              onKeyDown={e => e.key === 'Enter' && handleSend()}
              placeholder="Ask a question..."
              className="flex-1 rounded-md border border-border bg-background px-3 py-2 text-sm outline-none focus:border-primary/60"
            />
            <button
              onClick={handleSend}
              aria-label="Send message"
              className="grid h-9 w-9 shrink-0 cursor-pointer place-items-center rounded-md bg-primary text-primary-foreground transition-colors hover:bg-primary/90"
            >
              <Send className="h-4 w-4" />
            </button>
          </div>
        </div>
      )}

      <button
        onClick={() => setIsOpen(open => !open)}
        aria-label={isOpen ? 'Close chat' : 'Open chat'}
        className="ml-auto grid h-14 w-14 cursor-pointer place-items-center rounded-full bg-primary text-primary-foreground shadow-[0_0_20px_rgba(255,103,23,0.5)] transition-transform hover:scale-105"
      >
        {isOpen ? <X className="h-5 w-5" /> : <MessageCircle className="h-5 w-5" />}
      </button>
    </div>
  );
};

export default ChatWidget;
