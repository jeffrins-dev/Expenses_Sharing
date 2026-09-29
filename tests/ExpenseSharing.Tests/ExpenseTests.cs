using ExpenseSharing.Domain;
using Xunit;

namespace ExpenseSharing.Tests;

public class ExpenseTests
{
    [Fact]
    public void Should_Calculate_Equal_Share()
    {
        // Arrange
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

        // Act
        var share = expense.GetShare();

        // Assert
        Assert.Equal(100, share);
    }
}