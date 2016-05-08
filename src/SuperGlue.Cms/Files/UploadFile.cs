using System.IO;

namespace SuperGlue.Cms.Files
{
    public class UploadFile
    {
        public UploadFile(Stream data, string name, string contentType, string category)
        {
            Category = category;
            ContentType = contentType;
            Name = name;
            Data = data;
        }

        public Stream Data { get; private set; }
        public string Name { get; private set; }
        public string ContentType { get; private set; }
        public string Category { get; private set; }
    }
}