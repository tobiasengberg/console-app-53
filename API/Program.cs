using API;
using console_app;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var sitePolicy = "_site-policy";
builder.Services.AddDbContext<EducationContext>(options =>
    options.UseSqlite("DataSource=edu.db"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(sitePolicy, built =>
    {
        built.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(sitePolicy);
app.MapPost("/link", (string user, int number) => $"User was: {user}");
app.MapPost("/form", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();
    Person newPerson = new Person(form["firstname"], form["lastname"]);
    List<string> interests = form["interest"].ToList();
    Application newApplication = new Application(newPerson, interests); 
    return $"Form submitted by {newApplication.Person.FirstName} {newApplication.Person.LastName} who likes {
        newApplication.Interests[newApplication.Interests.Count() -1]}";
});

app.MapGet("/recognize/{number}", (int number) => WebHelp.DoWithNumber(number));
app.MapGet("/calculate/{number}", (int number) => (number - 9) * 3);
app.MapGet("/message/{name}", (string name) => $"Hello, {name}!");
app.MapGet("/boolean", () => DictionaryDemo.UseDictionary());
app.MapGet("/about/staff", () => 23 + 67);
app.MapGet("/about", () => "The about section");
app.MapGet("/", () => "Hello World!");
app.MapControllers();


app.Run();