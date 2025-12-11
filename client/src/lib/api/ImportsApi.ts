import api from "./axios";

export interface ImportResult {
    totalRows: number;
    importedTasks: number;
    skippedRows: number;
    errors: string[];
}

export async function importTasksFromSpreadsheet(file: File): Promise<ImportResult> {
    const formData = new FormData();
    formData.append("file", file);

    const response = await api.post<ImportResult>("/imports/tasks-from-spreadsheet", formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
    return response.data;
}