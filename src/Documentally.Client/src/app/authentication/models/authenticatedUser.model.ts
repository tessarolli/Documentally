import { UserRole } from "../enums/UserRole.enum";

// Contract: Authentication.AuthenticationResponse.cs
export interface AuthenticatedUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
  token: string;
}
