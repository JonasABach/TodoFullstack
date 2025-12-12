import { useMsal } from "@azure/msal-react";
import { loginRequest } from "@/lib/auth/msalConfig";
import { Button } from "@/components/ui/button";

export function DebugMsalToken() {
  const { instance, accounts } = useMsal();

  const handleClick = async () => {
    if (accounts.length === 0) {
      await instance.loginPopup(loginRequest);
    }
    const response = await instance.acquireTokenSilent({
      ...loginRequest,
      account: instance.getAllAccounts()[0],
    });
    console.log("MSAL access token:", response.accessToken);
  };

  return (
    <Button onClick={handleClick}>
      Test Entra token (log to console)
    </Button>
  );
}