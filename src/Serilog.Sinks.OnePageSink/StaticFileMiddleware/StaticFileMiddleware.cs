using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Serilog.Sinks.OnePageSink.StaticFileMiddleware
{
    public class StaticFileMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IOptions<OnePageSinkOptions> options;

        public StaticFileMiddleware(RequestDelegate next, IOptions<OnePageSinkOptions> options)
        {
            this.next = next;
            this.options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string basePath = options.Value.BasePath;
            PathString requestPath = httpContext.Request.Path;

            if (requestPath.StartsWithSegments(new PathString(basePath), StringComparison.InvariantCultureIgnoreCase, out PathString remaining) == false)
            {
                await next.Invoke(httpContext);
                return;
            }

            if (Path.HasExtension(requestPath.Value) == false)
            {
                await next.Invoke(httpContext);
                return;
            }

            if (string.IsNullOrWhiteSpace(remaining.Value))
            {
                remaining = new PathString("index.html");
            }

            string remainingPart = remaining.Value.Replace("/~", string.Empty);

            var assetPath = new PathString("/dist");
            PathString filePath = assetPath.Add(remainingPart);

            Assembly assembly = typeof(StaticFileMiddleware).GetTypeInfo().Assembly;
            string resourceName = assembly.GetName().Name + filePath.Value.Replace("/", ".").Replace("@", "_");
            Stream resource = assembly.GetManifestResourceStream(resourceName);

            
            if (resource == null)
            {
                await next(httpContext);
                return;
            }
            
            var newBody = new MemoryStream();
            await resource.CopyToAsync(newBody);

            newBody.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(newBody);
            string content = reader.ReadToEnd();
            newBody.Seek(0, SeekOrigin.Begin);

            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            await httpContext.Response.WriteAsync(content);
        }
    }
}
