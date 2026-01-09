import api from "./axios";
import { DueDateSummary, Task, TaskFilter } from "./interfaces";
import { TASKS_URLs } from "./URLs";

const SUSSCESSFUL_STATUS = 200;

export const tasksApi = {
	getTasksByListId: async (listId: string): Promise<Task[]> => {
		const response = await api.get<Task[]>(
			`${TASKS_URLs.GET_TASKS_BY_LIST_ID}/${listId}`
		);
		if (response.status !== SUSSCESSFUL_STATUS) {
			throw new Error("Failed to get tasks by list id: " + listId);
		}
		return response.data;
	},

	createTask: async (
		task: Omit<Task, "id" | "isCompleted">
	): Promise<Task> => {
		const response = await api.post<Task>(TASKS_URLs.CREATE_TASK, {
			name: task.name,
			description: task.description,
			dueDate: task.dueDate,
			priority: task.priority,
			listId: task.listId,
		});
		if (response.status !== SUSSCESSFUL_STATUS) {
			throw new Error("Failed to create task: " + task.name);
		}
		return response.data;
	},

	updateTask: async (task: Task): Promise<Task> => {
		const response = await api.put<Task>(TASKS_URLs.UPDATE_TASK, {
			id: task.id,
			name: task.name,
			description: task.description,
			dueDate: task.dueDate,
			priority: task.priority,
			isCompleted: task.isCompleted,
			listId: task.listId,
		});
		if (response.status !== SUSSCESSFUL_STATUS) {
			throw new Error("Failed to update task with id: " + task.id);
		}
		return response.data;
	},

	deleteTask: async (id: string): Promise<void> => {
		const response = await api.delete(
			`${TASKS_URLs.DELETE_TASK_BY_TASK_ID}/${id}`
		);
		if (response.status !== SUSSCESSFUL_STATUS) {
			throw new Error("Failed to delete task with id: " + id);
		}
	},

	getDueDateSummary: async (): Promise<DueDateSummary> => {
    	const response = await api.get("/tasks/due-summary");
    	return response.data;
  	},
  
	filterTasks: async (filter: TaskFilter): Promise<Task[]> => {
		const params = new URLSearchParams();
		if (filter.listId) params.append("listId", filter.listId);
		if (filter.search) params.append("search", filter.search);
		if (filter.isCompleted !== undefined) params.append("isCompleted", filter.isCompleted.toString());
		if (filter.dueBefore) params.append("dueBefore", filter.dueBefore);

		const response = await api.get<Task[]>(`/tasks/filter`, { params });
		if (response.status !== SUSSCESSFUL_STATUS) {
			throw new Error("Failed to filter tasks");
		}
		return response.data;
	},
};