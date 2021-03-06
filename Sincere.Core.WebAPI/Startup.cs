﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sincere.Core.IRepository.Base;
using Sincere.Core.IServices.Base;
using Sincere.Core.Model.EFCore;
using Sincere.Core.Model.Models;
using Sincere.Core.Repository.Base;
using Sincere.Core.Services.Base;
using Sincere.Core.WebAPI.ServiceExtension;

namespace Sincere.Core.WebAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContextPool<BaseCoreContext>(options =>
                options.UseSqlServer(BaseDBConfig.ConnectionString));
            services.AddScoped<IBaseContext, BaseCoreContext>();
            //泛型引用方式
            services.AddScoped(typeof(IBaseServices<>), typeof(BaseServices<>));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.RegisterAssembly("IServices");
            services.RegisterAssembly("IRepository");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
