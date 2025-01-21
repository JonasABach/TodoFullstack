import { auth } from "./base";
import { 
signInWithEmailAndPassword,
createUserWithEmailAndPassword
} from "firebase/auth";
import { sendVerificationEmail } from './EmailVerification';

export const SignUpWithEmailAndPassword = async (email: string, password: string) => {
  try {
    const userCredential = await createUserWithEmailAndPassword(auth, email, password);
    await sendVerificationEmail(userCredential.user);
    return userCredential.user;
  } catch (error) {
    throw new Error((error as Error).message);
  }
};

export const SignInWithEmailAndPassword = async (email: string, password: string) => {
  try {
    const userCredential = await signInWithEmailAndPassword(auth, email, password);
    return userCredential.user;
  } catch (error) {
    throw new Error((error as Error).message);
  }
};
