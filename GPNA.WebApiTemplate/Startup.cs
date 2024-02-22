
using AutoMapper;
using GPNA.Converters.TagValues;
using gRPCServer.Configuration;
using gRPCServer.Extensions;
using gRPCServer.ServiceDouble;
using Hellang.Middleware.ProblemDetails;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace GPNA.gRPCServer
{
    public class Startup
    {
        #region Fields
        private ILogger<Startup>? _logger;

        private readonly IConfiguration _configuration;
        #endregion Fields

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            //var clientConfiguration = _configuration.GetSection<ClientConfiguration>();
            services.AddSingleton(s => config.CreateMapper());

            //services.AddSingleton(clientConfiguration);

            services.AddProblemDetails(ConfigureProblemDetails);
            services.AddControllers();
            services.gRPCConfigureServer(new gRPCServerConfiguration
                {
                    HelthCheckPeriod = 30,
                    HelthCheckDelay = 5,
                    EnableDetailedErrors = true,
                    BatchCount = 10000
                },
                true,
                true
                );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "GPNA.gRPCServer",
                        Version = "v1.0",
                        Contact = new OpenApiContact
                        {
                            Name = "Example Contact",
                            Url = new Uri("https://example.com/contact")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Example License",
                            Url = new Uri("https://example.com/license")
                        }
                    });

                var filePath = Path.Combine(AppContext.BaseDirectory, "GPNA.gRPCServer.xml");
                c.IncludeXmlComments(filePath);
                //c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ServerGreeterDouble serverGreeterDouble, ServerGreeterBool serverGreeterBool)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GPNA.gRPCServer v1");
            }
            );

            app.UseStaticFiles();
            app.UseProblemDetails();
            app.UseCors(builder =>
                builder.WithOrigins()
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseRouting();
          
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client..."); });
                endpoints.MapControllers();
            });
            app.gRPCConfigureAppBuilder(true, true);

            FakeParameters.GenerateTagValueDouble(100000);
            FakeParameters.GenerateTagValueBool(100000);
            var _doubles = new List<TagValueDouble>(FakeParameters.StorageListDouble);
            foreach (var item in _doubles)
            {
                serverGreeterDouble.AddTagValueDouble(item);
            }
            var _bools = new List<TagValueBool>(FakeParameters.StorageListBool);
            foreach (var item in _bools)
            {
                serverGreeterBool.AddTagValueBool(item);
            }
        }

        private void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
        {
            options.OnBeforeWriteDetails = (ctx, problem) =>
            {
                problem.Extensions["traceId"] = ctx.TraceIdentifier;
            };
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }
    }
}
