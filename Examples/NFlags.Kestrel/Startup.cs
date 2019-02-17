using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NFlags.Commands;

namespace NFlags.Kestrel
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, CommandArgs commandArgs)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(commandArgs.GetOption<string>(Options.Greeting));
            });
        }
    }
}