using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Proact.Configurations;
using Proact.EncryptionAgentService.Configurations;
using Proact.EncryptionAgentService.Controllers;
using Proact.EncryptionAgentService.Decryption;
using System;
using System.IO;

namespace Proact.EncryptionAgentService {
    public class Startup {
        public Startup( IConfiguration configuration ) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services ) {

            services.AddDbContext<ProactAgentDatabaseContext>(
                options => options.UseSqlServer(
                Configuration.GetConnectionString( "DefaultConnection" ) )
                .UseLazyLoadingProxies() );

            services.AddControllers();
            services.AddScoped<IDecryptionService, MixedSymAsymDecryptionService>();

            services.AddSwaggerGen( c => {
                c.SwaggerDoc( "v1", new OpenApiInfo { 
                    Title = "EncryAgentServ", 
                    Version = "v1" 
                } );

                var filePath = Path.Combine( 
                    AppContext.BaseDirectory,
                    "EncryAgentServ.xml" );

                c.IncludeXmlComments( filePath );
            } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env ) {
            if ( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();   
            }

            app.UseSwagger();
            app.UseSwaggerUI( c => c.SwaggerEndpoint( 
                "/swagger/v1/swagger.json", 
                "Proact.EncryptionAgentService v1" ) );

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints( endpoints => {
                endpoints.MapControllers();
            } );

            AutoMapperConfiguration.Configure();

            ProactConfiguration.Init( Configuration );
        }
    }
}
