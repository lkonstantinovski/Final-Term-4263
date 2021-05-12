using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using midTerm.Data;
using midTerm.Infrastructure;
using midTerm.Services.Abstractions;
using midTerm.Services.Services;
using System;
using System.IO;
using System.Reflection;

namespace midTerm
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddDbContext<MidTermDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>().Default,
                    optionsBuilder =>
                    {
                        optionsBuilder.EnableRetryOnFailure();
                        optionsBuilder.CommandTimeout(60);
                        optionsBuilder.MigrationsAssembly("midTerm.Data");
                    });
                options.UseInternalServiceProvider(serviceProvider)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }).AddEntityFrameworkSqlServer();
            
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(QuestionProfile));
            }).CreateMapper();


            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IOptionService, OptionService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<ISurveyUserService, SurveyUserService>();

            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                     new OpenApiInfo
                     {
                         Title = "midTerm",
                         Version = "v1",
                         Description = "Internet Services Final",
                         Contact = new OpenApiContact
                         {
                             Name = "Leo Konstantinovski 4263",
                             Email = "konstantinovski.leo@outlook.com"
                         }
                     });
                c.UseInlineDefinitionsForEnums();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "midTerm v1");
                    c.DocumentTitle = "Internet Services Final API v1";
                    c.RoutePrefix = String.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
