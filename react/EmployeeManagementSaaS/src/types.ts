export interface Skill {
  id: string;
  name: string;
  description: string;
}

export interface NewSkill {
  name: string;
  description: string;
}

export interface LoginResponse {
  token: string;
  refreshToken?: string;
}