using System;
using System.IO;

namespace Serilog.Sinks.OnePageSink.StaticFileMiddleware
{
    public class FileSystemManager : IFileSystemManager
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public Stream GetFileStream(string filePath)
        {
            return File.OpenRead(filePath);
        }
    }
}
