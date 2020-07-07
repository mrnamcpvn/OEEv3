using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OEE_API.Models.SHB;
using OEE_API.Models.SHW_SHD;
using OEE_API.Models.SYF;
using AutoMapper;
using OEE_API.Data.Repository;
using OEE_API.Data.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OEE_API.Application.Interfaces;
using OEE_API.Application.Implementation;
using SHW_SHD_IService = OEE_API.Application.Interfaces.SHW_SHD;
using SHW_SHD_Service = OEE_API.Application.Implementation.SHW_SHD;
using SHB_IService = OEE_API.Application.Interfaces.SHB;
using SHB_Service = OEE_API.Application.Implementation.SHB;
using SYF_IService = OEE_API.Application.Interfaces.SYF;
using SYF_Service = OEE_API.Application.Implementation.SYF;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OEE_API.Application.AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OEE_API.Application.Implementation.SHW_SHD;
using OEE_API.Application.Interfaces.SHW_SHD;

namespace OEE_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DBContextSHW_SHD>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("SHW_SHDConnection"))
              .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))
              );
            services.AddDbContext<DBContextSHB>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("SHBConnection"))
               .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))
                );
            services.AddDbContext<DBContextSYF>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("SYFConnection"))
              .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))
              );

            services.AddControllers();
            // config Mapper
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(AutoMapperConfig.RegisterMappings());
            services.AddScoped<IMapper>(sp =>
            {
                return new Mapper(AutoMapperConfig.RegisterMappings());
            });

            services.AddCors();
            // Add the temp data provider
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSession();

            services.AddTransient(typeof(IRepositorySHW_SHD<,>), typeof(RepositorySHW_SHD<,>));
            services.AddTransient(typeof(IRepositorySHB<,>), typeof(RepositorySHB<,>));
            services.AddTransient(typeof(IRepositorySYF<,>), typeof(RepositorySYF<,>));
            // Service SHW_SHD
            services.AddTransient<SHW_SHD_IService.ICell_OEEService, SHW_SHD_Service.Cell_OEEService>();
            services.AddTransient<SHW_SHD_IService.IActionTimeService, SHW_SHD_Service.ActionTimeService>();
            //Service SHB
            services.AddTransient<SHB_IService.ICell_OEEService, SHB_Service.Cell_OEEService>();
            // Service SYF
            services.AddTransient<SYF_IService.ICell_OEEService, SYF_Service.Cell_OEEService>();
            services.AddTransient<SYF_IService.IActionTimeService, SYF_Service.ActionTimeService>();
            //Serrvices general
            services.AddTransient<IAvailabilityService, AvailabilityService>();
            services.AddTransient<ITrendService, TrendService>();
                    services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IDowntimeReasonsService, DowntimeReasonsService>();
            services.AddTransient<IDowntimeAnalysisService, DowntimeAnalysisService>();
            services.AddTransient<IDownTimeDetailService, DowntimeDetailService>();


            services.AddMvc(option => option.EnableEndpointRouting = false)
            .AddSessionStateTempDataProvider()
              .AddNewtonsoftJson(opt =>
              {
                  opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
              });
            // services.AddMvc()
            //   .SetCompatibilityVersion(CompatibilityVersion.Latest)
            // .AddSessionStateTempDataProvider()
            //   .AddNewtonsoftJson(opt =>
            //   {
            //       opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //   });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/OEE-System-{Date}.txt");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Cấu hình hiển thị lỗi ở chế độ production
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            // app.UseHttpsRedirection();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "spa-fallback",
                    pattern: "{controller=FallBack}/{action=Index}/{id?}"
                );

                endpoints.MapFallbackToController("Index", "FallBack");
            });
        }
    }
}
