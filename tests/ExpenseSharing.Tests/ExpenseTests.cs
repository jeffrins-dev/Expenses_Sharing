using ExpenseSharing.Domain;
using Xunit;

namespace ExpenseSharing.Tests;

public class ExpenseTests
{
    [Fact]
    public void Should_Calculate_Equal_Share()
    {
        // Arrange
        var data = CreateMockData();

        // Act
        var share = data.Expense.GetShare();

        // Assert
        Assert.Equal(100, share);
    }

    private static MockData CreateMockData()
    {
        var alice = new User(1, "Alice");
        var bob = new User(2, "Bob");
        var charlie = new User(3, "Charlie");

        var participants = new List<User>
        {
            alice,
            bob,
            charlie
        };

        var expense = new Expense(
            1,
            alice,
            300,
            participants);

        return new MockData(expense);
    }

    private record MockData(Expense Expense);
}
