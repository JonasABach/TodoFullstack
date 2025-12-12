import { Navigate } from 'react-router'
import { useMsal } from "@azure/msal-react"
import { msalInstance } from "@/lib/auth/msalInstance"

export function PrivateRoute({ children }: { children: React.ReactNode }) {
  const { accounts } = useMsal()

  const hasAccountsFromHook = accounts.length > 0
  const hasAccountsFromCache = msalInstance.getAllAccounts().length > 0
  const isAuthenticated = hasAccountsFromHook || hasAccountsFromCache

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />
  }

  return children
}