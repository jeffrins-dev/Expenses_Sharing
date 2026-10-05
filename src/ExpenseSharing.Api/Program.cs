using ExpenseSharing.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ExpenseService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Frontend");

app.MapGet("/", () => "Expense Sharing API");

// User CRUD
app.MapGet("/users", (ExpenseService service) =>
{
    return service.GetUsers();
});

app.MapPost("/users", (
    int id,
    string name,
    ExpenseService service) =>
{
    return service.CreateUser(id, name);
});

app.MapPut("/users/{id}", (
    int id,
    string name,
    ExpenseService service) =>
{
    return service.UpdateUser(id, name);
});

app.MapDelete("/users/{id}", (
    int id,
    ExpenseService service) =>
{
    service.DeleteUser(id);
    return Results.NoContent();
});

// Expense CRUD
app.MapGet("/expenses", (ExpenseService service) =>
{
    return service.GetExpenses();
});

app.MapPost("/expenses", (
    int id,
    int paidByUserId,
    decimal amount,
    int[] participantUserIds,
    ExpenseService service) =>
{
    var users = service.GetUsers();

    var paidBy = users.Single(user => user.Id == paidByUserId);

    var participants = participantUserIds
        .Select(userId => users.Single(user => user.Id == userId))
        .ToList();

    return service.AddExpense(
        id,
        paidBy,
        amount,
        participants);
});

app.MapPut("/expenses/{id}", (
    int id,
    int paidByUserId,
    decimal amount,
    int[] participantUserIds,
    ExpenseService service) =>
{
    var users = service.GetUsers();

    var paidBy = users.Single(user => user.Id == paidByUserId);

    var participants = participantUserIds
        .Select(userId => users.Single(user => user.Id == userId))
        .ToList();

    return service.UpdateExpense(
        id,
        paidBy,
        amount,
        participants);
});

app.MapDelete("/expenses/{id}", (
    int id,
    ExpenseService service) =>
{
    service.DeleteExpense(id);
    return Results.NoContent();
});

// Balance
app.MapGet("/users/{id}/balance", (
    int id,
    ExpenseService service) =>
{
    var user = service.GetUsers()
        .Single(user => user.Id == id);

    return service.GetBalance(user);
});

app.Run();