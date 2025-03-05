using Autofac;
using Autofac.Extensions.DependencyInjection;
using GonzaShoes.Data;
using GonzaShoes.Model;
using GonzaShoes.Model.Configurations;
using GonzaShoes.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var connectionStrings = new ConnectionStrings();
builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

var appConfiguration = new AppConfiguration();
builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("AppConfiguration"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionStrings.DefaultConnection,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "GonzaShoes",
            ValidAudience = "GonzaShoes",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration.JWTSecretKey)),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("AuthToken"))
                    context.Token = context.Request.Cookies["AuthToken"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy => policy.RequireAuthenticatedUser());
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterAssemblyTypes(typeof(AppDbContext).Assembly)
    .Where(t => t.Name.EndsWith("Repository"))
    .AsImplementedInterfaces()
    .InstancePerLifetimeScope();
    
    containerBuilder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
    .Where(t => t.Name.EndsWith("Service") && !t.CustomAttributes.Any(p => p.AttributeType == typeof(NotInjectable)))
    .AsImplementedInterfaces()
    .InstancePerLifetimeScope();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401)
        context.Response.Redirect("/Account/Login");
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
    app.UseDeveloperExceptionPage();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();