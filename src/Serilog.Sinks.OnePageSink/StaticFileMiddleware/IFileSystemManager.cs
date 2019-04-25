using System;
using System.IO;

namespace Serilog.Sinks.OnePageSink.StaticFileMiddleware
{
    public interface IFileSystemManager
    {
        bool Exists(string path);
        Stream GetFileStream(string filePath);
    }
}