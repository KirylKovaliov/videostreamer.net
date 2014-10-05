using System.Configuration;
using System.IO;
using System.Web.Hosting;
using VideoStreamer.Net.Configuration;

namespace VideoStreamer.Net.Storage
{
    public class FolderStorage : StorageBase
    {
        public override void ValidateConfig(StorageTypeElement config)
        {
            if (!Directory.Exists(Folder))
                throw new ConfigurationErrorsException("VideoStreaming.Net configuration error. Folder '{0}' doesn't exist.");
        }

        public override long GetLength(string file)
        {
            string filePath = GetFileAbsolutePath(file);
            FileInfo fileInfo = new FileInfo(filePath);

            return fileInfo.Length;
        }

        public override byte[] Read(string file, long start, long length)
        {
            string filePath = GetFileAbsolutePath(file);
            using (StreamReader reader = new StreamReader(filePath))
            {
                byte[] buffer = new byte[length];
                reader.BaseStream.Seek(start, SeekOrigin.Begin);
                reader.BaseStream.Read(buffer, 0, (int)length);

                return buffer;
            }
        }

        public string Folder
        {
            get
            {
                if (Config.Folder.StartsWith("~/"))
                    return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, Config.Folder.Replace("~/", ""));

                return Config.Folder;
            }
        }

        private string GetFileAbsolutePath(string file)
        {
            string fileName = file.Replace("/", @"\");
            if (fileName.StartsWith(@"\"))
                fileName = fileName.Substring(1);

            string result = Path.Combine(Folder, fileName);

            if (!File.Exists(result))
                throw new FileNotFoundException("Cannot find file '{0}'", result);

            return result;
        }
    }
}
