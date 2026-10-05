using ExpenseSharing.Application;
using ExpenseSharing.Domain;
using Xunit;


namespace ExpenseSharing.Tests;

public class ExpenseCrudTests
{
    [Fact]
    public void Should_Create_Expense()
    {
        // Arrange
        var data = CreateMockData();

        // Act
        var expense = data.Service.AddExpense(
            1,
            data.Alice,
            300,
            data.Participants);

        // Assert
        Assert.Equal(1, expense.Id);
        Assert.Equal(300, expense.Amount);
        Assert.Equal(data.Alice, expense.PaidBy);
    }

    [Fact]
    public void Should_Read_Expenses()
    {
        // Arrange
        var data = CreateMockData();

        data.Service.AddExpense(
            1,
            data.Alice,
            300,
            data.Participants);

        // Act
        var expenses = data.Service.GetExpenses();

        // Assert
        Assert.Single(expenses);
    }

    [Fact]
    public void Should_Update_Expense()
    {
        // Arrange
        var data = CreateMockData();

        data.Service.AddExpense(
            1,
            data.Alice,
            300,
            data.Participants);

        // Act
        var expense = data.Service.UpdateExpense(
            1,
            data.Alice,
            600,
            data.Participants);

        // Assert
        Assert.Equal(600, expense.Amount);
    }

    [Fact]
    public void Should_Delete_Expense()
    {
        // Arrange
        var data = CreateMockData();

        data.Service.AddExpense(
            1,
            data.Alice,
            300,
            data.Participants);

        // Act
        data.Service.DeleteExpense(1);

        // Assert
        Assert.Empty(data.Service.GetExpenses());
    }

    private static MockData CreateMockData()
    {
        var service = new ExpenseService();

        var alice = service.CreateUser(1, "Alice");
        var bob = service.CreateUser(2, "Bob");
        var charlie = service.CreateUser(3, "Charlie");

        var participants = new List<User>
        {
            alice,
            bob,
            charlie
        };

        return new MockData(
            service,
            alice,
            participants);
    }

    private record MockData(
        ExpenseService Service,
        User Alice,
        List<User> Participants);
}