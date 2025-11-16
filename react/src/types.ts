export interface Skill {
  id: string;
  name: string;
  description: string;
}

export interface NewSkill {
  name: string;
  description: string;
}

export interface Employee {
  id: string;   // GUID
  fullName: string;
  skills: string;
}

export interface LoginResponse {
  token: string;
  refreshToken?: string;
}