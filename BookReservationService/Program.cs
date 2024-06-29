using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using BookReservationBL.BusinessLayer;
using BookReservationDL.DataAccessLayer;
using BookReservationDL.DatabaseContext;
using System.Net;
using Serilog.Sinks.Email;
using Serilog;
using BookReservationService;

try
{
    var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

    var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");


    AppSettings appSettings = configuration.GetRequiredSection("AppSettings").Get<AppSettings>();

    var emailInfo = new EmailConnectionInfo
    {
        EmailSubject = appSettings.EmailSubject,
        EnableSsl = appSettings.EnableSsl,
        Port = appSettings.Port,
        FromEmail = appSettings.FromEmail,
        MailServer = appSettings.MailServer,
        ToEmail = appSettings.ToEmail,
        NetworkCredentials = new NetworkCredential(appSettings.FromEmail, appSettings.EmailPassword)
    };


    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Serilog\\log_.txt"), rollOnFileSizeLimit: true, fileSizeLimitBytes: 1000000, rollingInterval: RollingInterval.Month, retainedFileCountLimit: 24, flushToDiskInterval: TimeSpan.FromSeconds(1))
        //.WriteTo.Email(emailInfo)                           
        .CreateLogger();





    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Book Reservation API",
            Description = "Reservation services for the books."
        });

        c.EnableAnnotations();

        var mainAssembly = Assembly.GetExecutingAssembly();
        var referencedAssembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BookReservationModel.dll")); // Adjust the DLL name as needed

        var mainXmlFile = $"{mainAssembly.GetName().Name}.xml";
        var referencedXmlFile = $"{referencedAssembly.GetName().Name}.xml";

        var mainXmlPath = Path.Combine(AppContext.BaseDirectory, mainXmlFile);
        var referencedXmlPath = Path.Combine(AppContext.BaseDirectory, referencedXmlFile);

        c.IncludeXmlComments(mainXmlPath);
        c.IncludeXmlComments(referencedXmlPath);
    });

    //EF
    builder.Services.AddDbContext<SystemDbContext>(options =>
                options.UseSqlite(defaultConnectionString));

    //Models
    builder.Services.AddScoped<IBookDL, BookDL>();
    builder.Services.AddScoped<IBookBL, BookBL>();
    builder.Services.AddScoped<IReservationDL, ReservationDL>();
    builder.Services.AddScoped<IReservationBL, ReservationBL>();

    Log.Information("Book Reservation Service is started.");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    Log.Information("Book Reservation Service is stopped.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Book Reservation Service is failed to run correctly.");
}
finally
{
    Log.CloseAndFlush();
}