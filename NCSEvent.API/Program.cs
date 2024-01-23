using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using NCSEvent.API.Commons.Exceptions;
using NCSEvent.API.Commons.Extensions;
using NCSEvent.API.Entities;
using NCSEvent.API.Services;
using NCSEvent.API.Services.Implementations;
using NCSEvent.API.Services.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Xml;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using NCSEvent.API.Commons.Models;
using Hangfire;

namespace NCSEvent.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddIdentity<Users, Roles>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddDbContext<AppDbContext>(db =>
            {
                db.UseSqlServer(builder.Configuration.GetConnectionString("NCSEventConnection"),
                    provideroptions => provideroptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            });
            {
                builder.Services.AddScoped<ISessionService, SessionService>();
                builder.Services.AddScoped<IAccount, AccountService>();
                builder.Services.AddScoped<RoleManager<Roles>>();
                builder.Services.AddScoped<UserManager<Users>>();
                builder.Services.AddScoped<IEventManagementService, EventManagementService>();
                builder.Services.AddScoped<IMembershipTypeService, MembershipTypeService>();
                builder.Services.AddScoped<IReportService, ReportService>();
                builder.Services.AddScoped<IUploadManagementService, UploadManagementService>();
                builder.Services.AddScoped<IDashboardService, DashboardService>();
                builder.Services.AddScoped<IMembershipTypeService, MembershipTypeService>();
                builder.Services.AddScoped<IHotelManagementService, HotelManagementService>();
                builder.Services.AddScoped<IMembershipManagementService, MembershipManagementService>();
                builder.Services.AddScoped<IUploadManagementService, UploadManagementService>();
                builder.Services.AddScoped<EmailServiceBinding>();
                builder.Services.AddScoped<EmailHelper>();
                builder.Services.AddScoped<IRegistrationService,  RegistrationService>();
                builder.Services.AddScoped<IEventImageService, EventImageService>();
                builder.Services.AddScoped<ITagManagementService, TagManagementService>();
                builder.Services.AddScoped<IUploadImageHelper, UploadImageHelper>();
                builder.Services.AddScoped<IGuestSpeaker, GuestSpeakerService>();
                builder.Services.AddScoped<IFeedback, FeedbackService>();
                


                builder.Services.AddDistributedMemoryCache();
                builder.Services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromDays(24);//We set Time here
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });
               
            }

            // Register your services, repositories, etc.
            builder.Services.AddTransient<FeedbackEmailService>();
            // Configure EmailServiceBinding
            builder.Services.Configure<EmailServiceBinding>(builder.Configuration.GetSection("EmailServiceBinding"));
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<EmailServiceBinding>>().Value);


            //builder.Services.AddScoped<IMembershipManagementService, MembershipManagementService>();

            //builder.Services.AddScoped<IUploadMemberService, UploadMemberService>();    
            builder.Services.AddControllers(); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagger();
            builder.Services.AddHttpContextAccessor();

            // Add Hangfire services
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("NCSEventConnection"))
            );
            // Add Hangfire server
            builder.Services.AddHangfireServer();

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    dbContext.Database.Migrate();
            //}
            app.SeedAdmin().Wait();

            var config = app.Services.GetService<IConfiguration>() ?? throw new ArgumentNullException("config");
            var env = app.Services.GetService<IHostEnvironment>() ?? throw new ArgumentNullException("env");
            SwaggerOptionsHelper.SwaggerOptionsHelperConfifure(config);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NCS Event  V1");
                });
                // Configure Hangfire
                app.UseHangfireDashboard();
                app.UseHangfireServer();

                // Schedule the job to run every minute
                RecurringJob.AddOrUpdate<FeedbackEmailService>("SendFeedbackEmails", x => x.SendFeedbackEmails(), Cron.MinuteInterval(10));

            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardOptions);
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


