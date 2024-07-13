using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftCreateBackZips
{
    internal class DirectoryHelper
    {

        public static void CopyDirectory(string sourceDir, string destDir, List<string> logEntries)
        {
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);

                // Skip session.lock file
                if (fileName.Equals("session.lock", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Skipped (session.lock): {filePath}");
                    logEntries.Add($"Skipped (session.lock): {filePath}");
                    continue;
                }

                string destFilePath = Path.Combine(destDir, fileName);

                try
                {
                    File.Copy(filePath, destFilePath);
                    Console.WriteLine($"Copied: {filePath}");
                    logEntries.Add($"Copied: {filePath}");
                }
                catch (IOException ioEx) when ((ioEx.HResult & 0x0000FFFF) == 32) // ERROR_SHARING_VIOLATION
                {
                    Console.WriteLine($"Skipped (in use): {filePath}");
                    logEntries.Add($"Skipped (in use): {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying file: {filePath} - {ex.Message}");
                    logEntries.Add($"Error copying file: {filePath} - {ex.Message}");
                }
            }

            foreach (string directoryPath in Directory.GetDirectories(sourceDir))
            {
                string destDirectoryPath = Path.Combine(destDir, Path.GetFileName(directoryPath));
                Directory.CreateDirectory(destDirectoryPath);
                CopyDirectory(directoryPath, destDirectoryPath, logEntries);
            }
        }

    }
}
