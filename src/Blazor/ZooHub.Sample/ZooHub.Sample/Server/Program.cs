using Microsoft.OpenApi.Models;

using Orleans;
using Orleans.Hosting;

using ZooHub.Sample.Server;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(builder =>
    {
        builder.ConfigureApplicationParts(manager =>
        {
            manager.AddApplicationPart(typeof(IAssemblyRegistrer).Assembly).WithReferences();
        });
        builder.UseLocalhostClustering();
        builder.AddMemoryGrainStorageAsDefault();
        builder.AddSimpleMessageStreamProvider("SMS");
        builder.AddMemoryGrainStorage("PubSubStore");
    })
    .ConfigureWebHostDefaults( webBuilder => {
        webBuilder.ConfigureServices(services =>
        {
            services.AddControllers()
            .AddApplicationPart(typeof(IAssemblyRegistrer).Assembly);
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Zoo Hub", Version = "v1" });
            });
            services.AddRazorPages();
            services.AddCors(options =>
            {
                options.AddPolicy("ApiService",
                    builder =>
                    {
                        builder
                            .WithOrigins(
                                "http://localhost:20370",
                                "http://localhost:20370")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }).Configure(app =>
        {

            app.UseHttpsRedirection();
            app.UseCors("ApiService");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo Hub");
            });
            app.MapWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api"), blazor =>
            {
                blazor.UseBlazorFrameworkFiles();
                blazor.UseStaticFiles();

                blazor.UseRouting();
                blazor.UseEndpoints(endpoints =>
                {
                    endpoints.MapFallbackToFile("index.html");
                });
            });

            //explicitly map api endpoints only when path starts with api
            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), api =>
            {
                //if you are not using a blazor app, you can move these files out of this closure
                api.UseStaticFiles();
                api.UseRouting();
                api.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });

        })
        .UseUrls("http://localhost:5000;https://localhost:5001");
    })
    .ConfigureServices(services => {
        services.Configure<ConsoleLifetimeOptions>(options => {
            options.SuppressStatusMessages = true;
        });
    })
    .Build().RunAsync();
