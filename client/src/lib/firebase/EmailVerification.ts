import { 
  sendEmailVerification, 
  User,
  ActionCodeSettings
} from "firebase/auth";

// Function to send verification email
export const sendVerificationEmail = async (user: User) => {
  try {
    // Configure action code settings (optional)
    const actionCodeSettings: ActionCodeSettings = {
      url: `${window.location.origin}/verify-email`, // Redirect URL after verification
      handleCodeInApp: true
    };

    await sendEmailVerification(user, actionCodeSettings);
    return true;
  } catch (error) {
    throw new Error((error as Error).message);
  }
};

// Function to check if email is verified
export const checkEmailVerified = (user: User | null): boolean => {
  if (!user) return false;
  return user.emailVerified;
};

// Function to reload user to get updated email verification status
export const reloadUser = async (user: User) => {
  try {
    await user.reload();
    return user.emailVerified;
  } catch (error) {
    throw new Error((error as Error).message);
  }
};