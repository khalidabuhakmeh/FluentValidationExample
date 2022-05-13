using FluentValidation;
using FluentValidationExample;
using static Microsoft.AspNetCore.Http.Results;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapPost("/person", (Validated<Person> req) =>
{
    // deconstruct to bool & Person
    var (isValid, value) = req;

    return isValid 
        ? Ok(value) 
        : ValidationProblem(req.Errors);
});

app.Run();

public record Person(string? Name, int? Age);

// ReSharper disable once UnusedType.Global
public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(m => m.Name).NotEmpty();
        RuleFor(m => m.Age).NotEmpty().GreaterThan(0);
    }
}