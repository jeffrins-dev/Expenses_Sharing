using ExpenseSharing.Application;
using Xunit;

namespace ExpenseSharing.Tests;

public class UserTests
{
    [Fact]
    public void Should_Create_A_User()
    {
        // Arrange
        var expenseService = new ExpenseService();

        // Act
        expenseService.CreateUser(1, "Alice");

        // Assert
        var user = expenseService.GetUser(1);

        Assert.Equal(1, user.Id);
        Assert.Equal("Alice", user.Name);
    }
}