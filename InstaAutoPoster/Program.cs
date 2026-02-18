using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Quartz;
using InstaAutoPoster;
using InstaAutoPoster.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService();


builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("InstaJob");

    q.AddJob<InstaJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(t => t.ForJob(jobKey)
        .WithCronSchedule("0 0 6 * * ?"));

    q.AddTrigger(t => t.ForJob(jobKey)
        .WithCronSchedule("0 0 18 * * ?"));

    q.AddTrigger(t => t.ForJob(jobKey)
        .WithCronSchedule("0 30 22 * * ?"));
});

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddTransient<InstaJob>();
builder.Services.AddSingleton<AiService>();
builder.Services.AddSingleton<ImageGenerator>();
builder.Services.AddSingleton<InstagramService>();

var host = builder.Build();
host.Run();
