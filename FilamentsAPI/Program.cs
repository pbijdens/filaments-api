using FilamentsAPI;
using FilamentsAPI.Model;
using FilamentsAPI.Persistence;
using FilamentsAPI.Services;
using FilamentsAPI.Services.Authentication;
using FilamentsAPI.Services.Filaments;
using FilamentsAPI.Services.Storage;
using FilamentsAPI.Services.Storageboxes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((c) => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FilamentsAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IDatabaseServices, MySQLDatabaseService>();
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
builder.Services.AddTransient<IPhotoStore, FilesystemPhotoStore>();
builder.Services.AddTransient<IFilamentsService, FilamentsService>();
builder.Services.AddTransient<IStorageboxesService, StorageboxesService>();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<CustomJwtBearerHandler>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<JwtBearerOptions, CustomJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.Use(async (ctx, next) =>
{
    if (ctx.Request.Method.Equals("options", StringComparison.InvariantCultureIgnoreCase) && ctx.Request.Headers.ContainsKey("Access-Control-Request-Private-Network"))
    {
        ctx.Response.Headers["Access-Control-Allow-Private-Network"] = "true";
    }

    await next();
});

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Use(async (ctx, next) => {
    using var scope = app.Services.CreateAsyncScope();
    ILogger<Program>? logger = scope.ServiceProvider.GetService<ILogger<Program>>();
    ctx.Items["logger"] = logger;

    await next();
});

app.UseMiddleware<ErrorHandlingMiddleware>();

await app.MigrateDatabases();

app.Run();
