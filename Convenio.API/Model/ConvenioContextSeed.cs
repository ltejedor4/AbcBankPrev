using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convenio.API.Model
{
    public class ConvenioContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ConvenioContext context = serviceScope.ServiceProvider.GetService<ConvenioContext>();
                context.Database.Migrate();
                if (!context.Convenios.Any())
                {
                    context.Convenios.AddRange(GetPreconfiguredConvenios());
                }

                await context.SaveChangesAsync();
            }            
        }

        static IEnumerable<Convenio> GetPreconfiguredConvenios()
        {
            return new List<Convenio>()
            {
                new Convenio(){Codigo="1234",Nombre="ETB",CasoUso="SOAP",BaseUrl="http://ec2-18-218-36-246.us-east-2.compute.amazonaws.com:8080/gas-natural/PagosService",Estado=true,Metodo="GET",Accion="consultar"},
                new Convenio(){Codigo="1234",Nombre="ETB",CasoUso="SOAP",BaseUrl="http://ec2-18-218-36-246.us-east-2.compute.amazonaws.com:8080/gas-natural/PagosService",Estado=true,Metodo="POST",Accion="pagar"},
                new Convenio(){Codigo="1234",Nombre="ETB",CasoUso="SOAP",BaseUrl="http://ec2-18-218-36-246.us-east-2.compute.amazonaws.com:8080/gas-natural/PagosService",Estado=true,Metodo="POST",Accion="compensar"},
                new Convenio(){Codigo="1234",Nombre="GAS",CasoUso="REST",BaseUrl="http://ec2-3-16-152-105.us-east-2.compute.amazonaws.com:8080/servicios/pagos/v1/payments",Estado=true,Metodo="GET",Accion="consultar"},
                new Convenio(){Codigo="1234",Nombre="GAS",CasoUso="REST",BaseUrl="http://ec2-3-16-152-105.us-east-2.compute.amazonaws.com:8080/servicios/pagos/v1/payments",Estado=true,Metodo="POST",Accion="pagar"},
                new Convenio(){Codigo="1234",Nombre="GAS",CasoUso="REST",BaseUrl="http://ec2-3-16-152-105.us-east-2.compute.amazonaws.com:8080/servicios/pagos/v1/payments",Estado=true,Metodo="DELETE",Accion="compensar"}
            };

        }
    }
}
