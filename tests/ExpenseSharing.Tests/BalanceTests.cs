using ExpenseSharing.Application;
using ExpenseSharing.Domain;
using Xunit;

namespace ExpenseSharing.Tests;

public class BalanceTests
{
    [Fact]
    public void Should_Show_Alice_Receives_200()
    {
        // Arrange
        var service = CreateService();

        var alice = service.CreateUser(1, "Alice");
        var bob = service.CreateUser(2, "Bob");
        var charlie = service.CreateUser(3, "Charlie");

        AddEqualExpense(service, alice, bob, charlie);

        // Act
        var balance = service.GetBalance(alice);

        // Assert
        Assert.Equal(200, balance);
    }

    [Fact]
    public void Should_Show_Bob_Owes_100()
    {
        // Arrange
        var service = CreateService();

        var alice = service.CreateUser(1, "Alice");
        var bob = service.CreateUser(2, "Bob");
        var charlie = service.CreateUser(3, "Charlie");

        AddEqualExpense(service, alice, bob, charlie);

        // Act
        var balance = service.GetBalance(bob);

        // Assert
        Assert.Equal(-100, balance);
    }

    [Fact]
    public void Should_Show_Charlie_Owes_100()
    {
        // Arrange
        var service = CreateService();

        var alice = service.CreateUser(1, "Alice");
        var bob = service.CreateUser(2, "Bob");
        var charlie = service.CreateUser(3, "Charlie");

        AddEqualExpense(service, alice, bob, charlie);

        // Act
        var balance = service.GetBalance(charlie);

        // Assert
        Assert.Equal(-100, balance);
    }

    private static ExpenseService CreateService()
    {
        return new ExpenseService();
    }

    private static void AddEqualExpense(
        ExpenseService service,
        User alice,
        User bob,
        User charlie)
    {
        service.AddExpense(
            1,
            alice,
            300,
            new List<User>
            {
                alice,
                bob,
                charlie
            });
    }
}