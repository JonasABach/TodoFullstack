import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { List, Task } from "@/lib/api/interfaces";
import { useAppStore } from "@/lib/store/useStore";
import { TaskItem } from "./TaskItem";

interface TaskCompletedViewListProps {
  tasks: Task[];
}

export default function TaskCompletedViewList({ tasks }: TaskCompletedViewListProps) {
  const { lists } = useAppStore();

  // Group completed tasks by list
  const tasksByList = tasks.reduce((acc, task) => {
    // `tasks` should already be only completed, but this guard is harmless
    if (!task.isCompleted) return acc;

    const list = lists.find(l => l.id === task.listId);
    if (!list) return acc;

    if (!acc[list.id]) {
      acc[list.id] = {
        list,
        tasks: [] as Task[],
      };
    }
    acc[list.id].tasks.push(task);
    return acc;
  }, {} as Record<string, { list: List; tasks: Task[] }>);

  const groups = Object.values(tasksByList);

  if (groups.length === 0) {
    return (
      <div className="text-center py-8 text-gray-500">
        No completed tasks found.
      </div>
    );
  }
  console.log("completed tasks passed in", tasks);
  console.log("tasksByList keys", Object.keys(tasksByList));
  return (
    <div className="space-y-4">
      <Accordion type="single" collapsible>
        {groups.map(({ list, tasks }) => (
          tasks.length > 0 && (
            <AccordionItem key={list.id} value={list.id}>
              <AccordionTrigger>
                {list.name} ({tasks.length})
              </AccordionTrigger>
              <AccordionContent>
                <div className="space-y-2">
                  {tasks.map(task => (
                    <TaskItem key={task.id} task={task} />
                  ))}
                </div>
              </AccordionContent>
            </AccordionItem>
          )
        ))}
      </Accordion>
    </div>
  );
}