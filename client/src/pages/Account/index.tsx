import { SidebarTrigger } from "@/components/ui/sidebar"
import Layout from "@/layout"
import { useAppStore } from "@/lib/store/useStore"
import { ChangePasswordForm } from "./componenets/ChangePasswordForm"
import { UpdateProfileForm } from "./componenets/UpdateUserProfileForm"
import { UserProfile } from "./componenets/UserProfile"

export function Account() {
  const { user } = useAppStore()

  return (
    <Layout>
      <div className="mx-auto p-3 space-y-8">
        <div className="flex items-center gap-4">
          <SidebarTrigger />
          <h1 className="text-3xl font-bold">Profile Settings</h1>
        </div>
        {user ? (
          <div className="space-y-4 mx-10">
            <UserProfile user={user} />
            <div className="grid grid-cols-2 gap-x-5">
              <UpdateProfileForm user={user} />
              <ChangePasswordForm userId={user.id} />
            </div>
          </div>
        ) : (
          <div className="mx-10 text-muted-foreground">
            Profile information is not available right now.
          </div>
        )}
      </div>
    </Layout>
  )
}