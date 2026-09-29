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
        var data = CreateMockData();

        // Act
        var balance = data.Service.GetBalance(data.Alice);

        // Assert
        Assert.Equal(200, balance);
    }

    [Fact]
    public void Should_Show_Bob_Owes_100()
    {
        // Arrange
        var data = CreateMockData();

        // Act
        var balance = data.Service.GetBalance(data.Bob);

        // Assert
        Assert.Equal(-100, balance);
    }

    [Fact]
    public void Should_Show_Charlie_Owes_100()
    {
        // Arrange
        var data = CreateMockData();

        // Act
        var balance = data.Service.GetBalance(data.Charlie);

        // Assert
        Assert.Equal(-100, balance);
    }

    private static MockData CreateMockData()
    {
        var service = new ExpenseService();

        var alice = service.CreateUser(1, "Alice");
        var bob = service.CreateUser(2, "Bob");
        var charlie = service.CreateUser(3, "Charlie");

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

        return new MockData(service, alice, bob, charlie);
    }

    private record MockData(
        ExpenseService Service,
        User Alice,
        User Bob,
        User Charlie);
}
