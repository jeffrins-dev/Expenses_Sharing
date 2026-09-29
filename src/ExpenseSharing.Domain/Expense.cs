namespace ExpenseSharing.Domain;

public class Expense
{
    public int Id { get; }
    public User PaidBy { get; }
    public decimal Amount { get; }
    public IReadOnlyList<User> Participants { get; }

    public Expense(
        int id,
        User paidBy,
        decimal amount,
        IReadOnlyList<User> participants)
    {
        Id = id;
        PaidBy = paidBy;
        Amount = amount;
        Participants = participants;
    }

    public decimal GetShare()
    {
        return Amount / Participants.Count;
    }

    public decimal GetBalanceFor(User user)
    {
        var share = GetShare();

        return PaidBy == user
            ? Amount - share
            : Participants.Contains(user)
                ? -share
                : 0;
    }
}