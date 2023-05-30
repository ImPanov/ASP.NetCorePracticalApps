using Packt.Shared;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddNorthwindContext();
var app = builder.Build();

if(!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseDefaultFiles(); // index.html, default.html, and so on
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapRazorPages();

app.MapGet("/hello", () => "Hello World!");

app.Run();
