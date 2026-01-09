import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider } from "@/components/ui/sidebar"
import { useLocation } from "react-router"

export default function Layout({ children }: { children: React.ReactNode }) {
  const location = useLocation()
  const isAuthRoute = ['/login', '/register'].includes(location.pathname)

  if (isAuthRoute) {
    return children
  }

  return (
    <SidebarProvider defaultOpen={true}>
      <div className="container flex min-h-screen min-w-full gap-4">
        <AppSidebar />
        <main className="container mx-auto flex-1 pr-4 lg:pr-8">
          {children}
        </main>
      </div>
    </SidebarProvider>
  )
}

