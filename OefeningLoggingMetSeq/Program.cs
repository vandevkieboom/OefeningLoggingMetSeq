using Serilog;
using Serilog.Formatting.Json;

namespace OefeningLoggingMetSeq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //clear all the default logging providers.
            builder.Logging.ClearProviders();
            //add the console logging provider
            //builder.logging.addconsole();
            //add the debug logging provider
            //builder.logging.adddebug();
            //this is to enable the eventlog provider for windows only. you need to specify the logging level in an `eventlog` section in the appsettings.json file.
            //builder.logging.addeventlog();

            //configure serilog
            var logger = new LoggerConfiguration()
                .WriteTo.File(formatter: new JsonFormatter(), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log.txt"), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90)
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.Seq("http://localhost:5341/")
                .CreateLogger();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
