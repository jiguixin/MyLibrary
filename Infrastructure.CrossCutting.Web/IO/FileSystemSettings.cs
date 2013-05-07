using Infrastructure.Crosscutting.Configuration;

namespace Infrastructure.CrossCutting.Web.IO
{
    public class FileSystemSettings : ISettings
    {
        public string DirectoryName { get; set; }
    }
}