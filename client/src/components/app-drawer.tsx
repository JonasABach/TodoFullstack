import React from "react"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"

interface DrawerProps {
  form: React.ReactNode,
  title: string,
  description: string,
  dueDate?: string | null,
  open: boolean,
  setOpen: React.Dispatch<React.SetStateAction<boolean>>
}

export function AppDrawer({ title, description, dueDate, form, open, setOpen  }: DrawerProps) {
    return (
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent className="sm:max-w-[425px]">
          <DialogHeader>
            <DialogTitle>{ title }</DialogTitle>
            <DialogDescription>
              { description }
            </DialogDescription>
            {dueDate && <DialogDescription>Due Date: {dueDate}</DialogDescription>}
          </DialogHeader>
          { form }
        </DialogContent>
      </Dialog>
    )
}
