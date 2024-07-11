using DemoTask.DAL;
using DemoTask.Interfaces;
using DemoTask.Models;
using DemoTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
        .SetBasePath(System.IO.Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
var appconfig = builder.Configuration.GetSection("AppSettings");
Env _env = new Env(appconfig);

builder.Services.AddSingleton<Env>(_env);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CbtDbContext>(options => options.UseMySQL(_env.MSQL_CONNECTION_STRING));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDBMigrationService, DBMigrationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/db/migrate", (IDBMigrationService migrationService) => migrationService.ApplyMigration());

//app.MapGet("/api/checklogin/{sIcNumber}", (IUserService userService, string sIcNumber) => userService.CheckLogin(new LoginReq() { sIcNumber = sIcNumber }));

app.MapPost("/api/checklogin", (IUserService userService, [FromBody] LoginReq objModel) => userService.CheckLogin(objModel));

app.MapPost("/api/register", (IUserService userService, [FromBody] RegisterUserReq objModel) => userService.RegisterUser(objModel));

app.MapPost("/api/sentMobileOtp", (IUserService userService, [FromBody] GenerateMobileOtpReq objModel) => userService.GenerateMobileOTP(objModel));

app.MapPost("/api/sentEmailOtp", (IUserService userService, [FromBody] GenerateEmailOtpReq objModel) => userService.GenerateEmailOTP(objModel));

app.MapPost("/api/verifyOtp", (IUserService userService, [FromBody] VerifyOtpReq objModel) => userService.VerifyOtp(objModel));

app.MapPatch("/api/updatePrivacyPolicy", (IUserService userService, [FromBody] UpdatePrivacyPolicyReq objModel) => userService.UpdatePrivacyPolicy(objModel));

app.MapPatch("/api/updateAccountPin", (IUserService userService, [FromBody] UpdateAccountPinReq objModel) => userService.UpdateAccountPin(objModel));

app.MapPatch("/api/updateBiometric", (IUserService userService, [FromBody] BiometricReq objModel) => userService.UpdateBiometric(objModel));

app.MapPost("/api/verifyAccountPin", (IUserService userService, [FromBody] VerifyPinReq objModel) => userService.VerifyAccountPin(objModel));

app.MapPost("/api/verifyBiometric", (IUserService userService, [FromBody] BiometricReq objModel) => userService.VerifyBiometric(objModel));

app.Run();

