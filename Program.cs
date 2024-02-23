using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using SkillsManagement;
using SkillsManagement.Models;
using SkillsManagement_TestTask;
using Controller = SkillsManagement_TestTask.Controller;


namespace SkillManagement_TestTask
{
    public class Program
    {
        private const string Endpoint = "api/v1/persons";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Serilog configuration        
            var logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.File("logs.txt")
                .CreateLogger();

            builder.Logging.AddSerilog(logger);

            builder.Services.AddDbContext<SkillsContext>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            app.UseExceptionHandler(exceptionHandlerApp
                        => exceptionHandlerApp.Run(async context
                               => await Results.Problem().ExecuteAsync(context)));

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            if (Controller.DbIsEmpty())
                Controller.InitializeDb();

            app.MapGet(Endpoint, async () => await Controller.GetPersonsAsync())
                .WithTags("Getters");

            app.MapGet(Endpoint + "/{id}",
                async (long id) => await Controller.GetPersonAsync(id))
                .WithTags("Getters");

            app.MapPost(Endpoint, (Person person) => Controller.AddPersonAsync(person))
                .Accepts<Person>("application/json")
                .WithTags("Creators");

            app.MapPut(Endpoint + "/{id}",
                async (Person person, long id) => await Controller.UpdatePersonAsync(person, id))
                .WithTags("Updaters");

            app.MapDelete(Endpoint + "/{id}",
                async (long id) => await Controller.DeletePersonAsync(id))
                .WithTags("Deleters");

            app.Run();
        }
    }
}
