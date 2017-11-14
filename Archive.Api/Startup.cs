using Archive.Api.Models;
using Archive.Api.Service;
using Archive.Mq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Encodings.Web;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Archive.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            // TODO(hekj): This might be duplicated with the similar lines in Program.cs.
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LogContext>(opt => opt.UseInMemoryDatabase("Logs"));
            services.AddMvc();

            services.Configure<RabbitMQSettings>(Configuration.GetSection("RabbitMQSettings"));

            services.AddSingleton<IMessageConsumer, MqConsumer>();
            services.AddSingleton<IHostedService, ReceiveLogsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Setup some error diagnostics.
            app.UseStatusCodePages();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.Map("/error", error =>
            {
                error.Run(async context =>
                {
                    var builder = new StringBuilder();
                    builder.AppendLine("<html><body>");
                    builder.AppendLine("An error occurred.<br />");
                    var path = context.Request.Path.ToString();
                    if (path.Length > 1)
                    {
                        builder.AppendLine("Status Code: " +
                            HtmlEncoder.Default.Encode(path.Substring(1)) + "<br />");
                    }
                    var referrer = context.Request.Headers["referer"];
                    if (!string.IsNullOrEmpty(referrer))
                    {
                        builder.AppendLine("Return to <a href=\"" +
                            HtmlEncoder.Default.Encode(referrer) + "\">" +
                            WebUtility.HtmlEncode(referrer) + "</a><br />");
                    }
                    builder.AppendLine("</body></html>");
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(builder.ToString());
                });
            });

            app.UseMvc();
        }
    }
}
