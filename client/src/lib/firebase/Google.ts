import { auth, googleProvider } from "./base";
import { 
  signInWithPopup,
  signInWithRedirect,
  getRedirectResult,
  GoogleAuthProvider
} from "firebase/auth";

// Sign in with Google using Popup
export const signInWithGoogle = async () => {
  try {
    const result = await signInWithPopup(auth, googleProvider);
    const credential = GoogleAuthProvider.credentialFromResult(result);
    const token = credential?.accessToken;
    const user = result.user;
    return {
      user,
      token,
      error: null
    };
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (error: any) {
    // Handle Errorsss
    const errorCode = error.code;
    const errorMessage = error.message;
    // The email of the user's account used
    const email = error.customData?.email;
    // The AuthCredential type that was used
    const credential = GoogleAuthProvider.credentialFromError(error);

    return {
      user: null,
      token: null,
      error: {
        code: errorCode,
        message: errorMessage,
        email,
        credential
      }
    };
  }
};

// Sign in with Google using Redirect (better for mobile devices)
export const signInWithGoogleRedirect = async () => {
  try {
    await signInWithRedirect(auth, googleProvider);
  } catch (error) {
    throw new Error((error as Error).message);
  }
};

// Handle redirect result
export const handleGoogleRedirectResult = async () => {
  try {
    const result = await getRedirectResult(auth);
    if (result) {
      // This gives you a Google Access Token
      const credential = GoogleAuthProvider.credentialFromResult(result);
      const token = credential?.accessToken;
      
      // The signed-in user info
      const user = result.user;
      
      return {
        user,
        token,
        error: null
      };
    } 
    return {
      user: null,
      token: null,
      error: null
    };
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (error: any) {
    // Handle Errors
    const errorCode = error.code;
    const errorMessage = error.message;
    // The email of the user's account used
    const email = error.customData?.email;
    // The AuthCredential type that was used
    const credential = GoogleAuthProvider.credentialFromError(error);

    return {
      user: null,
      token: null,
      error: {
        code: errorCode,
        message: errorMessage,
        email,
        credential
      }
    };
  }
};

// Handle account linking if user exists with different credential
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const handleExistingAccountError = async (error: any) => {
  if (error.code === 'auth/account-exists-with-different-credential') {
    const email = error.customData.email;
    const credential = GoogleAuthProvider.credentialFromError(error);
    
    // You might want to show a message to the user here
    // explaining they already have an account with a different provider
    
    return {
      email,
      pendingCredential: credential
    };
  }
  return null;
};