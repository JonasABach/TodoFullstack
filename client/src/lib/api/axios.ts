import axios from "axios";
import { BASE_URL } from "./URLs";
import { msalInstance, loginRequest } from "@/lib/auth/msalInstance";

const api = axios.create({
	baseURL: BASE_URL,
	headers: {
		"Content-Type": "application/json",
	},
});

api.interceptors.request.use(
	async (config) => {
		const accounts = msalInstance.getAllAccounts();
		if (accounts.length > 0) {
			const response = await msalInstance.acquireTokenSilent({
				...loginRequest,
				account: accounts[0],
			});
			config.headers = config.headers ?? {};
			config.headers.Authorization = `Bearer ${response.accessToken}`;
		}
		return config;
	},
	(error) => {
		return Promise.reject(error);
	}
);

api.interceptors.response.use(
	(response) => response,
	(error) => Promise.reject(error)
);

export async function withRetry<T>(
	fn: () => Promise<T>,
	options: { retries?: number; delayMs?: number } = {}
): Promise<T> {
	const { retries = 1, delayMs = 1000 } = options;
	let attempt = 0;
	while (true) {
		try {
			return await fn();
		} catch (err) {
			attempt++;
			if (attempt > retries) throw err;
			await new Promise((res) => setTimeout(res, delayMs));
		}
	}
}

export default api;
