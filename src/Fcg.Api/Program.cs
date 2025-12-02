using Fcg.Api;
using Fcg.Api.Middlewares;
using Fcg.IOC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerWithJwt();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();
app.Run();