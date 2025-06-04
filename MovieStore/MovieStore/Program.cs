
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MovieStore.BL;
using MovieStore.HealthChecks;
using MovieStore.MapsterConfig;
using MovieStore.ServiceExtentions;
using MovieStore.Validators;
using MovieStoreB.DL;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace MovieStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme:
                    AnsiConsoleTheme.Code)
                .CreateLogger();

            builder.Logging.AddSerilog(logger);

            // Add services to the container.
            builder.Services
                .AddConfigurations(builder.Configuration)
                .AddDataDependencies(builder.Configuration)
                .AddBusinessDependencies();

            MapsterConfiguration.Configure();
            builder.Services.AddMapster();


            builder.Services
                .AddValidatorsFromAssemblyContaining<AddMovieRequestValidator>();
            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddHealthChecks();

            builder.Services.AddHealthChecks()
                .AddCheck<SampleHealthCheck>("Sample");

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHealthChecks("/Sample");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
