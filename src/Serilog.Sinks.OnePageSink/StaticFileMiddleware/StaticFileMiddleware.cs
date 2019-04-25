using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace Serilog.Sinks.OnePageSink.StaticFileMiddleware
{
    public class StaticFileMiddleware
    {
        private readonly FileExtensionContentTypeProvider fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
        private readonly IFileSystemManager fileSystemManager;
        private readonly RequestDelegate next;
        private readonly IOptions<OnePageSinkOptions> options;

        public StaticFileMiddleware(RequestDelegate next, IOptions<OnePageSinkOptions> options, IFileSystemManager fileSystemManager)
        {
            this.next = next;
            this.options = options;
            this.fileSystemManager = fileSystemManager;
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
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string pathLocation = Path.GetDirectoryName(assemblyLocation);
            string fullPath = Path.Combine(pathLocation, filePath.Value.TrimStart('/'));
            string normalizedFullPath = Path.GetFullPath(fullPath);

            bool fileExists = fileSystemManager.Exists(normalizedFullPath);
            if (fileExists == false)
            {
                await next.Invoke(httpContext);
                return;
            }

            if (fileExtensionContentTypeProvider.TryGetContentType(fullPath, out string contentType) == false)
            {
                await next.Invoke(httpContext);
                return;
            }

            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            httpContext.Response.ContentType = contentType;
            await httpContext.Response.SendFileAsync(normalizedFullPath);
            // Stream stream = fileSystemManager.GetFileStream(fullPath);
            // var memoryStream = new MemoryStream();
            // await stream.CopyToAsync(memoryStream);
            // memoryStream.Position = 0;
            // httpContext.Response.Body = memoryStream;
            // await next.Invoke(httpContext);
        }
    }
}
