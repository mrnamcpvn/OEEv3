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
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OEE_API.Helpers.AutoMapper;
using OEE_API._Repositories.Repositories;
using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API._Services.Interfaces;
using OEE_API._Services.Services;

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
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();

             // config Mapper
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IMapper>(sp =>
            {
                return new Mapper(AutoMapperConfig.RegisterMappings());
            });
            services.AddSingleton(AutoMapperConfig.RegisterMappings ());
            services.AddCors();
            // Add the temp data provider
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSession();

            services.AddMvc(option => option.EnableEndpointRouting = false)
            .AddSessionStateTempDataProvider()
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // Repository
            services.AddScoped<IActionTimeForOEERepository, ActionTimeForOEERepository>();
            services.AddScoped<IDowntimeReasonRepository, DowntimeReasonRepository>();
            services.AddScoped<IDowntimeRecordRepository, DowntimeRecordRepository>();
            services.AddScoped<IFactoryRepository, FactoryRepository>();
            services.AddScoped<IMachineInformationRepository, MachineInformationRepository>();
            services.AddScoped<IMachineStatusRepository, MachineStatusRepository>();
            services.AddScoped<IMachineTypeRepository, MachineTypeRepository>();
            services.AddScoped<IMaintenanceTimeRepository, MaintenanceTimeRepository>();
            services.AddScoped<IOEE_IDRepository, OEE_IDRepository>();
            services.AddScoped<IOEE_MMRepository, OEE_MMRepository>();
            services.AddScoped<IOEE_VNRepository, OEE_VNRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRoleUserRepository, RoleUserRepository>();
            services.AddScoped<IRowIndexRepository, RowIndexRepository>();
            services.AddScoped<IShiftRepository, ShiftRepository>();
            services.AddScoped<IShiftTimeConfigRepository, ShiftTimeConfigRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            // Service
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<ITrendService, TrendService>();
            services.AddScoped<IDownTimeReasonService, DownTimeReasonService>();
            services.AddScoped<IAuthService,AuthService>();
            services.AddScoped<IDowntimeAnalysisService,DowntimeAnalysisService>();
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
                endpoints.MapControllers();
            });
        }
    }
}
