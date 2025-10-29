using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public static class ExtractingPdf
    {
        public static void GetPdfFile(UserData user)
        {
            Thread.Sleep(1000);
            string PathStorageUser = Path.Combine(Storage.BasePath, "Users", $"{user.TelegramId}");

            string[] zipPath = Directory.GetFiles((Path.Combine(PathStorageUser, "DownloadDeclaration")));
            if(zipPath.Length > 0)
            {
                string extractionPath = Path.Combine(PathStorageUser, "DeclarationInPdf");
                int CountDeclaration = 1;
                foreach (var file in zipPath)
                {
                    using (ZipArchive archive = ZipFile.OpenRead(file))
                    {
                        foreach(ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.FullName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                            {
                                string destinationPath = Path.GetFullPath(Path.Combine(extractionPath, $"Declaration{CountDeclaration++}.pdf"));
                                entry.ExtractToFile(destinationPath, overwrite: true);
                                Thread.Sleep(300);

                                
                            }
                            
                        }
                    }
                }
            }
        }
    }
}
