using Scalar.AspNetCore;
using Playtestr.Api.Dtos;
using Playtestr.Api.Services.Games;
using Playtestr.Api.Data;
using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Endpoints;
using Playtestr.Api.Services.Playtests;
using System.Text.Json.Serialization;
using Playtestr.Api.Services.Questions;
using Playtestr.Api.Services.Submissions;
using Playtestr.Api.Services.PlaytestSummaries;
using Playtestr.Api.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Playtestr.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IPlaytestService, PlaytestService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IPlaytestSummaryService, PlaytestSummaryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddValidation();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

//Endpoints
app.MapGameEndpoints();
app.MapPlaytestEndpoints();
app.MapQuestionEndpoints();
app.MapSubmissionEndpoints();
app.MapPlaytestSummaryEndpoints();
app.MapAuthEndpoints();

app.Run();