import { useEffect, useState } from "react";
import type { Skill } from "./types";
import { login, fetchItems, addSkill } from "./api";

export default function App() {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("token")
  );
  const [items, setItems] = useState<Skill[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  // New skill fields
  const [newName, setNewName] = useState("");
  const [newDescription, setNewDescription] = useState("");
  const [adding, setAdding] = useState(false);

  // Fetch items whenever token changes
  useEffect(() => {
    if (!token) return;

    setLoading(true);
    fetchItems(token)
      .then((data) => {
        setItems(data);
        setError(null);
      })
      .catch((err: any) => {
        setError(err.message);
        if (err.message.includes("Unauthorized")) {
          setToken(null);
          localStorage.removeItem("token");
        }
      })
      .finally(() => setLoading(false));
  }, [token]);

  const handleLogin = async () => {
    try {
      setLoading(true);
      const res = await login(username, password);
      localStorage.setItem("token", res.token);
      setToken(res.token);
      setError(null);
    } catch (err: any) {
      setError(err.message || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setToken(null);
    setItems([]);
  };

  // --- Add Skill ---
  const handleAddSkill = async () => {
    if (!token) return;
    if (!newName) return setError("Name is required");

    try {
      setAdding(true);
      const newSkill = await addSkill(token, {
        name: newName,
        description: newDescription,
      });
      setItems([...items, newSkill]); // append new skill to table
      setNewName("");
      setNewDescription("");
      setError(null);
    } catch (err: any) {
      setError(err.message || "Failed to add skill");
    } finally {
      setAdding(false);
    }
  };

  // --- Render ---
  if (!token) {
    return (
      <div style={{ padding: 20, maxWidth: 400 }}>
        <h1>Login</h1>
        {error && <div style={{ color: "red" }}>{error}</div>}
        <input
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          style={{ width: "100%", marginBottom: 8, padding: 8 }}
        />
        <input
          placeholder="Password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          style={{ width: "100%", marginBottom: 8, padding: 8 }}
        />
        <button onClick={handleLogin} style={{ width: "100%", padding: 8 }}>
          {loading ? "Logging in..." : "Login"}
        </button>
      </div>
    );
  }

  return (
    <div style={{ padding: 20 }}>
      <h1>Skills List</h1>
      <button onClick={handleLogout} style={{ marginBottom: 20 }}>
        Logout
      </button>

      {/* Add Skill Form */}
      <div style={{ marginBottom: 20, border: "1px solid #ccc", padding: 10 }}>
        <h3>Add New Skill</h3>
        <input
          placeholder="Name"
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
          style={{ width: "100%", marginBottom: 8, padding: 8 }}
        />
        <input
          placeholder="Description"
          value={newDescription}
          onChange={(e) => setNewDescription(e.target.value)}
          style={{ width: "100%", marginBottom: 8, padding: 8 }}
        />
        <button onClick={handleAddSkill} style={{ width: "100%", padding: 8 }}>
          {adding ? "Adding..." : "Add Skill"}
        </button>
      </div>

      {loading && <div>Loading...</div>}
      {error && <div style={{ color: "red" }}>{error}</div>}

      {!loading && !error && (
        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead>
            <tr>
              <th style={{ border: "1px solid #ccc", padding: 8 }}>ID</th>
              <th style={{ border: "1px solid #ccc", padding: 8 }}>Name</th>
              <th style={{ border: "1px solid #ccc", padding: 8 }}>
                Description
              </th>
            </tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.id}>
                <td style={{ border: "1px solid #ccc", padding: 8 }}>
                  {item.id}
                </td>
                <td style={{ border: "1px solid #ccc", padding: 8 }}>
                  {item.name}
                </td>
                <td style={{ border: "1px solid #ccc", padding: 8 }}>
                  {item.description}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
