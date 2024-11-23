using backend.Entities;
using backend.Repositories.AuthRepo;
using backend.Repositories.EntitiesRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
        builder.WithOrigins("http://localhost:3000") 
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EcommerceDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
            });

// Register repositories
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IRepo<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepo<Customer>, CustomerRepo>();
builder.Services.AddScoped<IRepo<Product>, ProductRepo>();
builder.Services.AddScoped<IRepo<Order>, OrderRepo>();
builder.Services.AddScoped<IRepo<OrderItem>, OrderItemRepo>();
builder.Services.AddScoped<IRepo<ProductSize>, ProductSizeRepo>();
builder.Services.AddScoped<IRepo<User>, UserRepo>();
builder.Services.AddScoped<IRepo<CartItem>, CartItemRepo>();
builder.Services.AddScoped<PasswordHasher<Customer>>();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<IRepo<CartItem>, CartItemRepo>();
builder.Services.AddScoped<CartItemRepo>();
builder.Services.AddScoped<OrderRepo>();

// Register services
builder.Services.AddScoped<VNPayService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Register auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
