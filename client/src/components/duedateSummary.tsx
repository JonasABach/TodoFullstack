import { tasksApi, type DueDateSummary } from "@/lib/api/TasksApi";
import { useAppStore } from "@/lib/store/useStore";
import { useEffect, useState } from "react";

export function DueDateSummaryWidget() {
    const [summary, setSummary] = useState<DueDateSummary | null>(null);
    const [error, setError] = useState<string | null>(null);
    const tasks = useAppStore();

    useEffect(() => {
        let cancelled = false;
        (async () => {
            try {
                const data = await tasksApi.getDueDateSummary();
                if (!cancelled) {
                setSummary(data);
                setError(null);
                }
            } catch (err) {
                if (!cancelled) {
                setError("Failed to load due date summary");
                }
                console.error(err);
            }
        })();
        return () => {
            cancelled = true;
        };
    }, [tasks]);

    if (error) return <p className="text-sm text-destructive">{error}</p>;
    if (!summary) return <p className="text-sm text-muted-foreground">Loading summary...</p>;

    return (
        <div className="text-sm text-muted-foreground flex gap-4">
            <span>Overdue: {summary.overdueCount}</span>
            <span>Today: {summary.dueTodayCount}</span>
            <span>This week: {summary.dueThisWeekCount}</span>
            <span>Future: {summary.dueInFutureCount}</span>
        </div>
    );
}