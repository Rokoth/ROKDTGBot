using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ROTGBot.Db.Context;
using ROTGBot.Db.Interface;
using ROTGBot.Db.Repository;
using ROTGBot.Service;

namespace ROTGBot
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CommonOptions>(Configuration);
            services.AddControllersWithViews();
            services.AddLogging();           
            services.AddDbContextPool<DbPgContext>((opt) =>
            {
                opt.EnableSensitiveDataLogging();
                var connectionString = Configuration.GetConnectionString("MainConnection");
                opt.UseNpgsql(connectionString);
            });

            services.AddCors();
            services
                .AddAuthentication()
                .AddJwtBearer("Token", (options) =>
                {
                    var settings = (Configuration.GetSection("AuthOptions")?.Get<AuthOptions>()) ?? throw new ArgumentNullException(nameof(AuthOptions));
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //// укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        //// строка, представляющая издателя
                        ValidIssuer = settings.Issuer,
                        //// будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        //// установка потребителя токена
                        ValidAudience = settings.Audience,
                        //// будет ли валидироваться время существования
                        ValidateLifetime = true,
                        // установка ключа безопасности
                        IssuerSigningKey = settings.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization(options =>
            {
                var defPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Token")
                    .Build();
                options.AddPolicy("Token", defPolicy);
                options.DefaultPolicy = defPolicy;
            });

            services.AddScoped<IRepository<Db.Model.User>, Repository<Db.Model.User>>();                        
            services.AddScoped<IRepository<Db.Model.Role>, Repository<Db.Model.Role>>();
            services.AddScoped<IRepository<Db.Model.UserRole>, Repository<Db.Model.UserRole>>();
            services.AddScoped<IRepository<Db.Model.News>, Repository<Db.Model.News>>();
            services.AddScoped<IRepository<Db.Model.NewsMessage>, Repository<Db.Model.NewsMessage>>();
            services.AddScoped<IRepository<Db.Model.Groups>, Repository<Db.Model.Groups>>();
            services.AddScoped<IRepository<Db.Model.NewsButton>, Repository<Db.Model.NewsButton>>();

            services.AddDataServices();           

            services.AddRazorPages();

            services.AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                          Array.Empty<string>()
                    }
                });
            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
