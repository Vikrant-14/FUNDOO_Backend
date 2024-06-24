using BusinessLayer.Interface;
using BusinessLayer.Service;
using Fundoo.GLobalExceptionHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbFundoo"));
});

//User
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();

//Note
builder.Services.AddScoped<INoteBL, NoteBL>();
builder.Services.AddScoped<INoteRL, NoteRL>();

//Label
builder.Services.AddScoped<ILabelBL, LabelBL>();
builder.Services.AddScoped<ILabelRL, LabelRL>();

//NoteLabel
builder.Services.AddScoped<INoteLabelBL, NoteLabelBL>();
builder.Services.AddScoped<INoteLabelRL, NoteLabelRL>();

//Collaborator
builder.Services.AddScoped<ICollaboratorBL, CollaboratorBL>();
builder.Services.AddScoped<ICollaboratorRL, CollaboratorRL>();

builder.Services.AddControllers();

//Authentication(JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fundoo API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

//CORS
const string policyName = "CorsPolicy";
const string anotherName = "AnotherCorsPolicy";

builder.Services.AddCors(options => {
    options.AddPolicy(name: policyName, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    options.AddPolicy(name: anotherName, builder => 
    {
        builder.WithOrigins("http://localhost:5264")
            .WithMethods("GET", "POST")
            .WithHeaders("*");
    });
});

//Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession( options => { 
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.Name = "FundooCookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
    options.InstanceName = builder.Configuration["RedisCacheOptions:InstanceName"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fundoo API v1");
    });
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
