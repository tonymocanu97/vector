'use client';

import { createContext, useContext, useEffect, useState, type ReactNode } from 'react';

import { apiFetch } from '@/lib/api-client';

export type AuthUser = {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
};

type AuthResponse = {
  token: string;
  expiresAt: string;
  user: AuthUser;
};

type AuthContextValue = {
  user: AuthUser | null;
  token: string | null;
  isLoading: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (email: string, password: string, firstName: string, lastName: string) => Promise<void>;
  logout: () => void;
};

const STORAGE_KEY = 'vector-auth';

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    /* eslint-disable react-hooks/set-state-in-effect */
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored) {
      const parsed = JSON.parse(stored) as AuthResponse;
      setUser(parsed.user);
      setToken(parsed.token);
    }
    setIsLoading(false);
    /* eslint-enable react-hooks/set-state-in-effect */
  }, []);

  const persist = (response: AuthResponse) => {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(response));
    setUser(response.user);
    setToken(response.token);
  };

  const login = async (email: string, password: string) => {
    const response = await apiFetch<AuthResponse>('/auth/login', {
      method: 'POST',
      body: { email, password },
    });
    persist(response);
  };

  const register = async (email: string, password: string, firstName: string, lastName: string) => {
    const response = await apiFetch<AuthResponse>('/auth/register', {
      method: 'POST',
      body: { email, password, firstName, lastName },
    });
    persist(response);
  };

  const logout = () => {
    localStorage.removeItem(STORAGE_KEY);
    setUser(null);
    setToken(null);
  };

  return (
    <AuthContext.Provider value={{ user, token, isLoading, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
