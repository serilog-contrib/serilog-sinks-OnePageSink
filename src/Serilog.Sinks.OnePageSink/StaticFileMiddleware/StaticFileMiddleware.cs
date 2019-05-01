using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

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

            var remainingPart = remaining.Value.Replace("/~", string.Empty);

            var assetPath = new PathString("/assets");
            PathString filePath = assetPath.Add(remainingPart);

            Assembly assembly = typeof(Sinks.OnePageSink.StaticFileMiddleware.StaticFileMiddleware).GetTypeInfo().Assembly;
            var resourceName = assembly.GetName().Name  + filePath.Value.Replace("/",".").Replace("@","_");
            Stream resource = assembly.GetManifestResourceStream(resourceName);

            await next(httpContext);

            var newBody = new MemoryStream();
            await resource.CopyToAsync(newBody);

            newBody.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(newBody);
            var content = reader.ReadToEnd();
            newBody.Seek(0, SeekOrigin.Begin);

            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            await httpContext.Response.WriteAsync(content);
        }
    }
}
