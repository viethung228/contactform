//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;
using Manager.SharedLibs;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manager.WebApp.Helpers
{
    public class FileHelpers<T>
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(FileHelpers));
        public static T ReadJsonFileToObject(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
                else
                {
                    FileStream str = File.Create(filePath);
                    str.Close();
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not ReadJsonFileToObject because: {0}", ex.ToString());
                _logger.Error(strError);
            }

            return default(T);
        }

        public static void WriteObjectToJsonFile(T model, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not WriteObjectToJsonFile because: {0}", ex.ToString());
                _logger.Error(strError);
            }
        }
    }

    public class FileHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(FileHelpers));

        public static void WriteTextToJsonFile(string data, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, data);
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not WriteTextToJsonFile because: {0}", ex.ToString());
                _logger.Error(strError);
            }
        }

        public static string ReadJsonFileToText(string filePath)
        {
            var result = string.Empty;

            try
            {
               result = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not ReadJsonFileToText because: {0}", ex.ToString());
                _logger.Error(strError);
            }

            return result;
        }

        public static string MakeSureFolderExists(string folderPath)
        {
            bool exists = System.IO.Directory.Exists(folderPath);

            if (!exists)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("MakeSureFolderExists error because: {0}", ex.ToString()));
                }
            }

            return folderPath;
        }

        public static string GetFileContent(string filePath)
        {
            var fileContent = string.Empty;
            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console.
                    fileContent = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetFileContent error because: {0}", ex.ToString()));
            }

            return fileContent;
        }

        public static async void WriteTextAsync(string folderPath, string fileName, string text, string fileExt = "txt")
        {
            // Set a variable to the My Documents path.
            //string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the text asynchronously to a new file named "WriteTextAsync.txt".
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(folderPath, fileName + "." + fileExt)))
            {
                await outputFile.WriteAsync(text);
            }
        }

        public static bool UrlIsImage(string url)
        {
            var isImage = false;

            var imageTypes = new[] { "png", "jpg", "jpeg", "tiff", "gif", "webp" };

            var fileExt = System.IO.Path.GetExtension(url).Substring(1).ToLower();
            if (imageTypes.Contains(fileExt))
            {
                isImage = true;
            }

            return isImage;
        }

        public static bool UrlIsVideo(string url)
        {
            var isVideo = false;

            var videoTypes = new[] {
                "mp4", "mov", "mkv", "webm", "ogg", "avi", "wmv",
                "flv", "swf", "mpe"
            };

            var fileExt = System.IO.Path.GetExtension(url).Substring(1).ToLower();
            if (videoTypes.Contains(fileExt))
            {
                isVideo = true;
            }

            return isVideo;
        }
    }
}