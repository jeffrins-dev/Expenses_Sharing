using ExpenseSharing.Application;
using Xunit;

namespace ExpenseSharing.Tests;

public class UserCrudTests
{
    [Fact]
    public void Should_Create_User()
    {
        // Arrange
        var service = new ExpenseService();

        // Act
        var user = service.CreateUser(1, "Alice");

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("Alice", user.Name);
    }

    [Fact]
    public void Should_Read_Users()
    {
        // Arrange
        var service = new ExpenseService();

        service.CreateUser(1, "Alice");
        service.CreateUser(2, "Bob");

        // Act
        var users = service.GetUsers();

        // Assert
        Assert.Equal(2, users.Count);
    }

    [Fact]
    public void Should_Update_User()
    {
        // Arrange
        var service = new ExpenseService();

        service.CreateUser(1, "Alice");

        // Act
        var user = service.UpdateUser(1, "Alicia");

        // Assert
        Assert.Equal("Alicia", user.Name);
    }

    [Fact]
    public void Should_Delete_User()
    {
        // Arrange
        var service = new ExpenseService();

        service.CreateUser(1, "Alice");

        // Act
        service.DeleteUser(1);

        // Assert
        Assert.Empty(service.GetUsers());
    }
}