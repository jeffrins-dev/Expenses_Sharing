"use client";

import { FormEvent, useEffect, useState } from "react";

const API_URL =
  process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5199";

type User = {
  id: number;
  name: string;
};

type Expense = {
  id: number;
  paidBy: User;
  amount: number;
  participants: User[];
};

export default function Home() {
  const [users, setUsers] = useState<User[]>([]);
  const [expenses, setExpenses] = useState<Expense[]>([]);
  const [balances, setBalances] = useState<Record<number, number>>({});

  const [userId, setUserId] = useState("");
  const [userName, setUserName] = useState("");

  const [expenseId, setExpenseId] = useState("");
  const [paidByUserId, setPaidByUserId] = useState("");
  const [amount, setAmount] = useState("");
  const [participantUserIds, setParticipantUserIds] = useState("");

  const [editingUserId, setEditingUserId] = useState<number | null>(null);
  const [editingUserName, setEditingUserName] = useState("");

  const [editingExpenseId, setEditingExpenseId] = useState<number | null>(
    null
  );

  useEffect(() => {
    loadUsers();
    loadExpenses();
  }, []);

  async function loadUsers() {
    const response = await fetch(`${API_URL}/users`);
    const data = await response.json();

    setUsers(data);

    const balanceData: Record<number, number> = {};

    for (const user of data) {
      const balanceResponse = await fetch(
        `${API_URL}/users/${user.id}/balance`
      );

      balanceData[user.id] = await balanceResponse.json();
    }

    setBalances(balanceData);
  }

  async function loadExpenses() {
    const response = await fetch(`${API_URL}/expenses`);
    const data = await response.json();

    setExpenses(data);
  }

  async function addUser(event: FormEvent) {
    event.preventDefault();

    await fetch(
      `${API_URL}/users?id=${userId}&name=${encodeURIComponent(userName)}`,
      {
        method: "POST",
      }
    );

    setUserId("");
    setUserName("");

    await loadUsers();
  }

  async function updateUser(event: FormEvent) {
    event.preventDefault();

    if (editingUserId === null) {
      return;
    }

    await fetch(
      `${API_URL}/users/${editingUserId}?name=${encodeURIComponent(
        editingUserName
      )}`,
      {
        method: "PUT",
      }
    );

    setEditingUserId(null);
    setEditingUserName("");

    await loadUsers();
    await loadExpenses();
  }

  async function deleteUser(id: number) {
    await fetch(`${API_URL}/users/${id}`, {
      method: "DELETE",
    });

    await loadUsers();
    await loadExpenses();
  }

  async function addExpense(event: FormEvent) {
    event.preventDefault();

    const participantIds = participantUserIds
      .split(",")
      .map((id) => id.trim())
      .filter((id) => id.length > 0);

    const params = new URLSearchParams();

    params.set("id", expenseId);
    params.set("paidByUserId", paidByUserId);
    params.set("amount", amount);

    participantIds.forEach((id) => {
      params.append("participantUserIds", id);
    });

    await fetch(`${API_URL}/expenses?${params.toString()}`, {
      method: "POST",
    });

    clearExpenseForm();

    await loadExpenses();
    await loadUsers();
  }

  async function updateExpense(event: FormEvent) {
    event.preventDefault();

    if (editingExpenseId === null) {
      return;
    }

    const participantIds = participantUserIds
      .split(",")
      .map((id) => id.trim())
      .filter((id) => id.length > 0);

    const params = new URLSearchParams();

    params.set("paidByUserId", paidByUserId);
    params.set("amount", amount);

    participantIds.forEach((id) => {
      params.append("participantUserIds", id);
    });

    await fetch(
      `${API_URL}/expenses/${editingExpenseId}?${params.toString()}`,
      {
        method: "PUT",
      }
    );

    clearExpenseForm();

    await loadExpenses();
    await loadUsers();
  }

  async function deleteExpense(id: number) {
    await fetch(`${API_URL}/expenses/${id}`, {
      method: "DELETE",
    });

    await loadExpenses();
    await loadUsers();
  }

  function startUserEdit(user: User) {
    setEditingUserId(user.id);
    setEditingUserName(user.name);
  }

  function startExpenseEdit(expense: Expense) {
    setEditingExpenseId(expense.id);
    setExpenseId(expense.id.toString());
    setPaidByUserId(expense.paidBy.id.toString());
    setAmount(expense.amount.toString());

    setParticipantUserIds(
      expense.participants
        .map((participant) => participant.id)
        .join(",")
    );
  }

  function clearExpenseForm() {
    setEditingExpenseId(null);
    setExpenseId("");
    setPaidByUserId("");
    setAmount("");
    setParticipantUserIds("");
  }

  return (
    <main className="container">
      <h1>Expense Sharing</h1>

      {/* USERS */}

      <section className="card">
        <h2>Add User</h2>

        <form onSubmit={addUser} className="form">
          <input
            type="number"
            placeholder="User ID"
            value={userId}
            onChange={(event) => setUserId(event.target.value)}
            required
          />

          <input
            type="text"
            placeholder="Name"
            value={userName}
            onChange={(event) => setUserName(event.target.value)}
            required
          />

          <button type="submit">Add User</button>
        </form>
      </section>

      <section className="card">
        <h2>Users</h2>

        {users.length === 0 ? (
          <p>No users yet.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Balance</th>
                <th>Actions</th>
              </tr>
            </thead>

            <tbody>
              {users.map((user) => (
                <tr key={user.id}>
                  <td>{user.id}</td>

                  <td>
                    {editingUserId === user.id ? (
                      <input
                        value={editingUserName}
                        onChange={(event) =>
                          setEditingUserName(event.target.value)
                        }
                      />
                    ) : (
                      user.name
                    )}
                  </td>

                  <td>{balances[user.id] ?? 0}</td>

                  <td>
                    {editingUserId === user.id ? (
                      <button onClick={updateUser}>Save</button>
                    ) : (
                      <button onClick={() => startUserEdit(user)}>
                        Edit
                      </button>
                    )}

                    <button onClick={() => deleteUser(user.id)}>
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </section>

      {/* EXPENSE */}

      <section className="card">
        <h2>
          {editingExpenseId === null
            ? "Add Expense"
            : "Update Expense"}
        </h2>

        <form
          onSubmit={
            editingExpenseId === null
              ? addExpense
              : updateExpense
          }
          className="form"
        >
          <input
            type="number"
            placeholder="Expense ID"
            value={expenseId}
            onChange={(event) => setExpenseId(event.target.value)}
            disabled={editingExpenseId !== null}
            required
          />

          <input
            type="number"
            placeholder="Paid By User ID"
            value={paidByUserId}
            onChange={(event) =>
              setPaidByUserId(event.target.value)
            }
            required
          />

          <input
            type="number"
            placeholder="Amount"
            value={amount}
            onChange={(event) => setAmount(event.target.value)}
            required
          />

          <input
            type="text"
            placeholder="Participant IDs: 1,2,3"
            value={participantUserIds}
            onChange={(event) =>
              setParticipantUserIds(event.target.value)
            }
            required
          />

          <button type="submit">
            {editingExpenseId === null
              ? "Add Expense"
              : "Update Expense"}
          </button>

          {editingExpenseId !== null && (
            <button
              type="button"
              onClick={clearExpenseForm}
            >
              Cancel
            </button>
          )}
        </form>
      </section>

      {/* EXPENSE LIST */}

      <section className="card">
        <h2>Expenses</h2>

        {expenses.length === 0 ? (
          <p>No expenses yet.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Paid By</th>
                <th>Amount</th>
                <th>Participants</th>
                <th>Actions</th>
              </tr>
            </thead>

            <tbody>
              {expenses.map((expense) => (
                <tr key={expense.id}>
                  <td>{expense.id}</td>

                  <td>{expense.paidBy.name}</td>

                  <td>₹{expense.amount}</td>

                  <td>
                    {expense.participants
                      .map((participant) => participant.name)
                      .join(", ")}
                  </td>

                  <td>
                    <button
                      onClick={() =>
                        startExpenseEdit(expense)
                      }
                    >
                      Edit
                    </button>

                    <button
                      onClick={() =>
                        deleteExpense(expense.id)
                      }
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </section>
    </main>
  );
}