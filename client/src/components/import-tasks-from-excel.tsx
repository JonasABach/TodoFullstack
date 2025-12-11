import { ImportResult, importTasksFromSpreadsheet } from "@/lib/api/ImportsApi";
import React, { useState } from "react";

export function ImportTasksFromExcel() {
    const [file, setFile] = useState<File | null>(null);
    const [result, setResult] = useState<ImportResult | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const f = e.target.files?.[0] || null;
        setFile(f);
        setResult(null);
        setError(null);
    }

    const handleImport = async () => {
        if (!file) return;
        setIsLoading(true);
        setError(null);

        try {
            const res = await importTasksFromSpreadsheet(file);
            setResult(res);
        } catch (err: unknown) {
            setError(err instanceof Error && 'response' in err && typeof (err as { response?: { data?: string } }).response?.data === 'string' ? (err as { response: { data: string } }).response.data : "Failed to import");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="space-y-2">
            <input type="file" accept=".xlsx, .xls" onChange={handleFileChange} />
            <button
                onClick={handleImport}
                disabled={!file || isLoading}>
                {isLoading ? "Importing..." : "Import tasks from Excel"}
            </button>

            {error && <div className="text-red-500 text-sm">Error: {error}</div>}

            {result && (
                <div className="text-sm">
                    <p>Total Rows: {result.totalRows}</p>
                    <p>Imported Tasks: {result.importedTasks}</p>
                    <p>Skipped Rows: {result.skippedRows}</p>
                    {result.errors.length > 0 && (
                        <ul className="list-disc ml-4 mt-1">
                            {result.errors.map((err: string, idx: number) => (
                                <li key={idx}> {err}</li>
                            ))}
                        </ul>)}
                </div>
            )}
        </div>
    );
}