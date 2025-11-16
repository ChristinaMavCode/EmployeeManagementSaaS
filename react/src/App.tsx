import { useEffect, useState } from "react";
import type { Skill, Employee } from "./types";
import {
  login,
  fetchItems as fetchSkills,
  addSkill,
  fetchEmployees,
  assignSkillToEmployee,
} from "./api";

export default function App() {
  // --- Auth ---
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("token")
  );
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // --- Tabs ---
  const [activeTab, setActiveTab] = useState<"employees" | "skills">(
    "employees"
  );

  // --- Skills state ---
  const [skills, setSkills] = useState<Skill[]>([]);
  const [skillsLoading, setSkillsLoading] = useState(false);
  const [skillsError, setSkillsError] = useState<string | null>(null);
  const [newSkillName, setNewSkillName] = useState("");
  const [newSkillDesc, setNewSkillDesc] = useState("");
  const [addingSkill, setAddingSkill] = useState(false);

  // --- Employees state ---
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [employeesLoading, setEmployeesLoading] = useState(false);
  const [employeesError, setEmployeesError] = useState<string | null>(null);

  // --- Assign skill state ---
  const [selectedSkill, setSelectedSkill] = useState<string>("");
  const [assigningSkill, setAssigningSkill] = useState(false);
  const [assignError, setAssignError] = useState<string | null>(null);

  // --- Employee filter by skill ---
  const [filterSkill, setFilterSkill] = useState<string>("");

  // --- Fetch skills (always) ---
  useEffect(() => {
    if (!token) return;
    setSkillsLoading(true);
    fetchSkills(token)
      .then((data) => {
        setSkills(data);
        setSkillsError(null);
      })
      .catch((err: any) => setSkillsError(err.message))
      .finally(() => setSkillsLoading(false));
  }, [token]);

  // --- Fetch employees ---
  useEffect(() => {
    if (!token) return;
    setEmployeesLoading(true);
    fetchEmployees(token)
      .then((data) => {
        setEmployees(data);
        setEmployeesError(null);
      })
      .catch((err: any) => setEmployeesError(err.message))
      .finally(() => setEmployeesLoading(false));
  }, [token]);

  // --- Handlers ---
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
    setSkills([]);
    setEmployees([]);
  };

  const handleAddSkill = async () => {
    if (!token || !newSkillName) return setSkillsError("Name is required");
    try {
      setAddingSkill(true);
      const newSkill = await addSkill(token, {
        name: newSkillName,
        description: newSkillDesc,
      });
      setSkills([...skills, newSkill]);
      setNewSkillName("");
      setNewSkillDesc("");
      setSkillsError(null);
    } finally {
      setAddingSkill(false);
    }
  };

  const handleAssignSkill = async (employeeId: string) => {
    if (!token || !selectedSkill) return;
    try {
      setAssigningSkill(true);
      await assignSkillToEmployee(token, employeeId, selectedSkill);
      const updatedEmployees = await fetchEmployees(token);
      setEmployees(updatedEmployees);
      setSelectedSkill("");
      setAssignError(null);
    } catch (err: any) {
      setAssignError(err.message);
    } finally {
      setAssigningSkill(false);
    }
  };

  // --- Filtered employees using string skills ---
  const filteredEmployees = filterSkill
    ? employees.filter((emp) => {
        if (!emp.skills) return false;
        const skillNames = emp.skills.split(",").map((s) => s.trim());
        const selectedSkillName = skills.find(
          (s) => s.id === filterSkill
        )?.name;
        return selectedSkillName
          ? skillNames.includes(selectedSkillName)
          : false;
      })
    : employees;

  // --- Render login ---
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

  // --- Render main app ---
  return (
    <div style={{ padding: 20 }}>
      <h1>Employee Management SaaS</h1>
      <button onClick={handleLogout} style={{ marginBottom: 20 }}>
        Logout
      </button>

      {/* Tabs */}
      <div style={{ marginBottom: 20 }}>
        <button
          onClick={() => setActiveTab("employees")}
          style={{
            padding: 8,
            marginRight: 8,
            backgroundColor: activeTab === "employees" ? "#ccc" : "#fff",
          }}
        >
          Employees
        </button>
        <button
          onClick={() => setActiveTab("skills")}
          style={{
            padding: 8,
            backgroundColor: activeTab === "skills" ? "#ccc" : "#fff",
          }}
        >
          Skills
        </button>
      </div>

      {/* Employees tab */}
      {activeTab === "employees" && (
        <div>
          <h2>Employees</h2>

          {/* Filter by skill */}
          <div style={{ marginBottom: 10 }}>
            <label>Filter by Skill: </label>
            <select
              value={filterSkill}
              onChange={(e) => setFilterSkill(e.target.value)}
            >
              <option value="">All</option>
              {skills.map((skill) => (
                <option key={skill.id} value={skill.id}>
                  {skill.name}
                </option>
              ))}
            </select>
          </div>

          {employeesLoading && <div>Loading employees...</div>}
          {employeesError && (
            <div style={{ color: "red" }}>{employeesError}</div>
          )}

          {!employeesLoading && (
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead>
                <tr>
                  <th style={{ border: "1px solid #ccc", padding: 8 }}>ID</th>
                  <th style={{ border: "1px solid #ccc", padding: 8 }}>
                    Full Name
                  </th>
                  <th style={{ border: "1px solid #ccc", padding: 8 }}>
                    Assigned Skills
                  </th>
                  <th style={{ border: "1px solid #ccc", padding: 8 }}>
                    Assign Skill
                  </th>
                </tr>
              </thead>
              <tbody>
                {filteredEmployees.map((emp) => (
                  <tr key={emp.id}>
                    <td style={{ border: "1px solid #ccc", padding: 8 }}>
                      {emp.id}
                    </td>
                    <td style={{ border: "1px solid #ccc", padding: 8 }}>
                      {emp.fullName}
                    </td>
                    <td style={{ border: "1px solid #ccc", padding: 8 }}>
                      {emp.skills}
                    </td>
                    <td style={{ border: "1px solid #ccc", padding: 8 }}>
                      <select
                        value={selectedSkill}
                        onChange={(e) => setSelectedSkill(e.target.value)}
                        style={{ marginRight: 8 }}
                      >
                        <option value="">--Select Skill--</option>
                        {skills.map((skill) => (
                          <option key={skill.id} value={skill.id}>
                            {skill.name}
                          </option>
                        ))}
                      </select>
                      <button
                        onClick={() => handleAssignSkill(emp.id)}
                        disabled={assigningSkill || !selectedSkill}
                      >
                        {assigningSkill ? "Assigning..." : "Assign"}
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
          {assignError && (
            <div style={{ color: "red", marginTop: 8 }}>{assignError}</div>
          )}
        </div>
      )}

      {/* Skills tab */}
      {activeTab === "skills" && (
        <div>
          <h2>Skills</h2>
          {/* Add Skill Form */}
          <div
            style={{ marginBottom: 20, border: "1px solid #ccc", padding: 10 }}
          >
            <h3>Add New Skill</h3>
            <input
              placeholder="Name"
              value={newSkillName}
              onChange={(e) => setNewSkillName(e.target.value)}
              style={{ width: "100%", marginBottom: 8, padding: 8 }}
            />
            <input
              placeholder="Description"
              value={newSkillDesc}
              onChange={(e) => setNewSkillDesc(e.target.value)}
              style={{ width: "100%", marginBottom: 8, padding: 8 }}
            />
            <button
              onClick={handleAddSkill}
              style={{ width: "100%", padding: 8 }}
            >
              {addingSkill ? "Adding..." : "Add Skill"}
            </button>
            {skillsError && (
              <div style={{ color: "red", marginTop: 5 }}>{skillsError}</div>
            )}
          </div>

          {skillsLoading && <div>Loading skills...</div>}

          {!skillsLoading && (
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
                {skills.map((skill) => (
                  <tr key={skill.id}>
                    <td style={{ border: "1px solid #ccc", padding: 8 }}>
                      {skill.id}
                    </td>
                    <td style={{ border: "1px solid #ccc", padding: 8 }}>
                      {skill.name}
                    </td>
                    <td style={{ border: "1px solid #ccc", padding: 8 }}>
                      {skill.description}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}
    </div>
  );
}
