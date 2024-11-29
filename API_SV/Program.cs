using Microsoft.EntityFrameworkCore;
using API_SV;
using StockVision.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ���������� ��������� ���� ������
builder.Services.AddDbContext<ApplicationContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("StockVisionDb")));

// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins("http://localhost:8080") // ������� URL ���������
               .AllowAnyOrigin()  // ��������� ����� ���������
               .AllowAnyMethod()  // ��������� ����� ������ (GET, POST � �. �.)
               .AllowAnyHeader());  // ��������� ����� ���������
});

// ��������� JWT ��������������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // ���������, ��� "Jwt:Issuer" ����� ���������
            ValidAudience = builder.Configuration["Jwt:Audience"], // ���������, ��� "Jwt:Audience" ���������
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)) // ��������� ����
        };
    });

// ���������� �����������
builder.Services.AddAuthorization();

// ����������� ������������
builder.Services.AddControllers();

var app = builder.Build();

// ���������� CORS (������ ���� �� UseAuthentication � UseAuthorization)
app.UseCors("AllowAll");

// ��������� �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

// ����������� ��������� ��� ������������
app.MapControllers();

// �������� API
app.MapGet("/", () => "StockVision API");

app.MapUserCredentialsApi();

app.MapGroup("/users").MapUsersApi().RequireAuthorization();
app.MapGroup("/assets").MapAssetsApi().RequireAuthorization();
app.MapGroup("/portfolios").MapPortfoliosApi().RequireAuthorization();
app.MapGroup("/transactions").MapTransactionsApi().RequireAuthorization();
app.MapGroup("/historicaldata").MapHistoricalDataApi().RequireAuthorization();

// ������ ����������
app.Run();

