import api from "./axios";
import { AuthResponse, LoginRequest, RegisterRequest } from "./interfaces";
import { withRetry } from "./axios";
import { AUTH_URLs } from "./URLs";

const SUCCESSFUL_RESPONSE = 200;

export const authApi = {
	register: async (userInfo: RegisterRequest): Promise<AuthResponse> => {
		const response = await api.post(AUTH_URLs.REGISTER, userInfo);
		if (response.status !== SUCCESSFUL_RESPONSE)
			throw new Error("Registration Faild");
		return response.data;
	},

	login: async (loginRequest: LoginRequest): Promise<AuthResponse> => {
		return withRetry(
			async () => {
		const response = await api.post(AUTH_URLs.LOGIN, loginRequest);
		if (response.status !== SUCCESSFUL_RESPONSE)
			throw new Error("Login Faild");
		return response.data;
	},
			{
				retries: 10, // try one extra time
				delayMs: 1500, // 1.5s for DB to wake up
			}
		);
	},

	refresh: async (refreshToken: string): Promise<AuthResponse> => {
		const response = await api.post(
			AUTH_URLs.REFRESH_ACCESS_TOKEN_BY_REFRESH_TOKEN,
			refreshToken
		);
		if (response.status !== SUCCESSFUL_RESPONSE)
			throw new Error("Refresh Access Token Faild");
		return response.data;
	},

	logout: async () => {
		const response = await api.post(AUTH_URLs.LOGOUT);
		if (response.status !== SUCCESSFUL_RESPONSE)
			throw new Error("Logout Faild");
		return response.data;
	},

};
