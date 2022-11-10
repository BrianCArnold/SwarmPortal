import { JwtPayload } from "jwt-decode";


export interface JwtToken extends JwtPayload {
  acr: string;
  at_hash: string;
  aud: string;
  auth_time: number;
  azp: string;
  email: string;
  email_verified: boolean;
  exp: number;
  family_name: string;
  given_name: string;
  iat: number;
  iss: string;
  jti: string;
  name: string;
  nonce: string;
  preferred_username: string;
  roles: string[];
  session_state: string;
  sub: string;
  typ: string;
}
