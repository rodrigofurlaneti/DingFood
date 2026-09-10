import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { LoginResponse } from "../lib/types";

interface AuthState {
  accessToken: string | null;
  refreshToken: string | null;
  userName: string | null;
  companyId: number | null;
  businessGroupId: number | null;
  homeCompanyId: number | null;
  employeeId: number | null;
  branchId: number;
  setSession: (session: LoginResponse) => void;
  setBranchId: (branchId: number) => void;
  setCompany: (companyId: number, businessGroupId: number) => void;
  clear: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      refreshToken: null,
      userName: null,
      companyId: null,
      businessGroupId: null,
      homeCompanyId: null,
      employeeId: null,
      branchId: 0,
      setSession: (session) =>
        set((state) => ({
          accessToken: session.accessToken,
          refreshToken: session.refreshToken,
          userName: session.userName,
          companyId: state.companyId ?? session.companyId,
          homeCompanyId: session.homeCompanyId ?? state.homeCompanyId ?? session.companyId,
          businessGroupId: session.businessGroupId ?? state.businessGroupId,
          employeeId: state.companyId && state.companyId !== session.companyId ? state.employeeId : session.employeeId,
        })),
      setCompany: (companyId, businessGroupId) => set({ companyId, businessGroupId, branchId: 0, employeeId: null }),
      setBranchId: (branchId) => set({ branchId }),
      clear: () =>
        set({
          accessToken: null,
          refreshToken: null,
          userName: null,
          companyId: null,
          businessGroupId: null,
          homeCompanyId: null,
          branchId: 0,
          employeeId: null,
        }),
    }),
    { name: "syncbar-auth" },
  ),
);
