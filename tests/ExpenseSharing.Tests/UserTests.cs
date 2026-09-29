using ExpenseSharing.Domain;
using Xunit;

namespace ExpenseSharing.Tests;

public class UserTests
{
    [Fact]
    public void Should_Create_User()
    {
        // Arrange
        var id = 1;
        var name = "Alice";

        // Act
        var user = new User(id, name);

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("Alice", user.Name);
    }
}