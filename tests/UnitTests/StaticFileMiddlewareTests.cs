using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Serilog.Sinks.OnePageSink;
using Serilog.Sinks.OnePageSink.StaticFileMiddleware;
using Xunit;

namespace UnitTests
{
    public class StaticFileMiddlewareTests
    {
        [Fact]
        public async Task Invoke_JavascriptFile_FileContents()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.ASCII.GetBytes("test"));
            IOptions<OnePageSinkOptions> options = Options.Create(new OnePageSinkOptions());
            var middleware = new StaticFileMiddleware((innerHttpContext) => throw new InvalidOperationException(), options);

            var context = new DefaultHttpContext();
            context.Request.Path = "/onepage/index.html";

            //Act
            // await middleware.Invoke(context); 
            //var reader = new StreamReader(context.Response.Body);
            //string streamText = await reader.ReadToEndAsync();

            //Assert
            //Assert.Equal("test", streamText);
        }
    }
}
