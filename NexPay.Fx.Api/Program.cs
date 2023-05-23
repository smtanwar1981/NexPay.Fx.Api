using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using NexPay.Fx.Api.Common;
using NexPay.Fx.Api.Core;
using NexPay.Fx.Api.Service;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddHttpClient();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ILoginApiProxyService, LoginApiProxyService>();
builder.Services.AddSwaggerGen(options =>
{
    SwaggerDefinition.AddSwaggerDefinition(options);
});
builder.Services.AddScoped<IFxService, FxService>();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("SessionPolicy", policy =>
//    {
//        policy.Requirements.Add(new SessionRequirement("Authorization"));
//    });
//});

var app = builder.Build();

app.UseCors(builder =>
        builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "NexPay Foreign Exchange Api");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
