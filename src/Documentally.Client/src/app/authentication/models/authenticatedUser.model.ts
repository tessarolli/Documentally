// Contract: Authentication.AuthenticationResponse.cs
export interface AuthenticatedUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  token: string;
}
