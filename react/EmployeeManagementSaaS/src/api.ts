import type { Skill, LoginResponse, NewSkill } from "./types";

const API_URL = "http://localhost:5225/api/v1";

export async function login(username: string, password: string): Promise<LoginResponse> {
  const res = await fetch(`${API_URL}/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Accept": "application/json",
    },
    body: JSON.stringify({ username, password }),
  });

  const text = await res.text();
  let data: any;
  try {
    data = JSON.parse(text);
  } catch {
    throw new Error(`Login failed: Invalid JSON response: ${text}`);
  }

  if (!res.ok) {
    throw new Error(data.message || `Login failed: ${res.status}`);
  }

  console.log("Login response:", data); // debug
  return data as LoginResponse;
}

export async function fetchItems(token: string): Promise<Skill[]> {
  const res = await fetch(`${API_URL}/getskills`, {
    headers: { "Authorization": `Bearer ${token}` },
  });

  if (!res.ok) {
    if (res.status === 401) throw new Error("Unauthorized. Please log in again.");
    throw new Error(`API request failed: ${res.status}`);
  }

  return res.json();  
}

export async function addSkill(token: string, skill: NewSkill): Promise<Skill> {
  const res = await fetch(`${API_URL}/createskill`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token}`,
    },
    body: JSON.stringify(skill),
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`Add skill failed: ${res.status} - ${text}`);
  }

  const data = await res.json();
  return data as Skill;
}