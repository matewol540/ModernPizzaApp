using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using MongoDB.Driver;
using ModernPizzaApi.Connetors;

namespace ModernPizzaApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers();

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            var key = Encoding.ASCII.GetBytes("PizzaDemoKeyAndSomeRandomData");
            var SignKey = new SymmetricSecurityKey(key);
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SignKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });
            services.AddSwaggerGen(c =>
            c.SwaggerDoc("Version_1.0.0", new OpenApiInfo()
            {
                Title = "Modern Pizza API",

            }));

            var migrationOptions = new MongoStorageOptions { MigrationOptions = new MongoMigrationOptions { Strategy = MongoMigrationStrategy.Drop, BackupStrategy = MongoBackupStrategy.None } };

            services.AddHangfire(config => config.UseMongoStorage(new MongoClientSettings()
            {
                ConnectionMode = ConnectionMode.Automatic,
                Credential = MongoCredential.CreateCredential("bvjlr3yieol9j03", "u2bcoixvwja8lneywmyo", "LWSdD6FD3x9g4WrT9Dv8"),
                Server = new MongoServerAddress("bvjlr3yieol9j03-mongodb.services.clever-cloud.com", 27017)
            }, "bvjlr3yieol9j03",migrationOptions));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire",new DashboardOptions()
            {
                Authorization = new [] { new HangfireAuth() }
            });
            RecurringJob.AddOrUpdate("Check for expired reservaions",   () => DBConnector.CheckReservagtionsForExpired(), "*/15 * * * *");
            RecurringJob.AddOrUpdate("Clean up old reservations", () => DBConnector.CleanUpReservations(), "* */24 * * * *");

            app.UseCors(x =>
            {
                x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/Version_1.0.0/swagger.json", "ModernPizzaApi");
                x.RoutePrefix = String.Empty;
            });
        }
    }
}
