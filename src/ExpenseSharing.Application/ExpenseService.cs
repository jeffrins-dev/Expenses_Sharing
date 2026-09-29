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

    public decimal GetBalance(User user)
    {
        return _expenses.Sum(expense => expense.GetBalanceFor(user));
    }
}