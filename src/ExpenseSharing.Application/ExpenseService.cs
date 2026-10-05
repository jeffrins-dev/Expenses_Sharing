using ExpenseSharing.Domain;

namespace ExpenseSharing.Application;

public class ExpenseService
{
    private readonly List<User> _users = new();
    private readonly List<Expense> _expenses = new();

    public User CreateUser(int id, string name)
    {
        var user = new User(id, name);

        _users.Add(user);

        return user;
    }

    public IReadOnlyList<User> GetUsers()
    {
        return _users;
    }

    public User UpdateUser(int id, string name)
    {
        var user = _users.Single(user => user.Id == id);

        var updatedUser = new User(user.Id, name);

        _users.Remove(user);
        _users.Add(updatedUser);

        return updatedUser;
    }

    public void DeleteUser(int id)
    {
        var user = _users.Single(user => user.Id == id);

        _users.Remove(user);
    }

    public Expense AddExpense(
        int id,
        User paidBy,
        decimal amount,
        IReadOnlyList<User> participants)
    {
        var expense = new Expense(
            id,
            paidBy,
            amount,
            participants);

        _expenses.Add(expense);

        return expense;
    }

    public IReadOnlyList<Expense> GetExpenses()
    {
        return _expenses;
    }

    public Expense UpdateExpense(
        int id,
        User paidBy,
        decimal amount,
        IReadOnlyList<User> participants)
    {
        var expense = _expenses.Single(expense => expense.Id == id);

        var updatedExpense = new Expense(
            expense.Id,
            paidBy,
            amount,
            participants);

        _expenses.Remove(expense);
        _expenses.Add(updatedExpense);

        return updatedExpense;
    }

    public void DeleteExpense(int id)
    {
        var expense = _expenses.Single(expense => expense.Id == id);

        _expenses.Remove(expense);
    }

    public decimal GetBalance(User user)
    {
        return _expenses.Sum(
            expense => expense.GetBalanceFor(user));
    }
}