using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace MinecraftCreateBackZips
{
    internal class Program
    {
        static void Main(string[] args)
        {

            BackupManager backupManager = new BackupManager(
                sourceDir: @"C:\Server\Minecraft",
                backupDir: @"C:\Server\MC backup",
                logDir: @"C:\Server\MC backup\logs",
                dailyBackupDir: @"C:\Server\MC backup\daily"
            );

            backupManager.StartHourlyBackup();
            Console.WriteLine("Press [Enter] to exit...");
            Console.ReadLine();
        }
    }
}




/* Old code
 string sourceDir = @"C:\Server\Minecraft";
            string backupDir = @"C:\Server\MC backup";
            string logDir = @"C:\Server\MC backup\logs";
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string tempBackupDir = Path.Combine(backupDir, $"Backup_{timestamp}");
            string zipFileName = $"Backup_{timestamp}.zip";
            string zipFilePath = Path.Combine(backupDir, zipFileName);
            string logFilePath = Path.Combine(logDir, $"BackupLog_{timestamp}.txt");

            // Ensure directories exist
            Directory.CreateDirectory(backupDir);
            Directory.CreateDirectory(logDir);
            Directory.CreateDirectory(tempBackupDir);

            List<string> logEntries = new List<string>();

            Console.WriteLine($"Backup started at {DateTime.Now}");
            logEntries.Add($"Backup started at {DateTime.Now}");

            try
            {
                CopyDirectory(sourceDir, tempBackupDir, logEntries);

                // Create the ZIP file
                ZipFile.CreateFromDirectory(tempBackupDir, zipFilePath);

                // Delete the temporary backup directory
                Directory.Delete(tempBackupDir, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during backup: {ex.Message}");
                logEntries.Add($"Error during backup: {ex.Message}");
            }

            Console.WriteLine($"Backup completed at {DateTime.Now}");
            logEntries.Add($"Backup completed at {DateTime.Now}");

            // Write log entries to the log file
            File.WriteAllLines(logFilePath, logEntries);

            Console.WriteLine($"Backup and logging completed. Log file: {logFilePath}");
        }

        static void CopyDirectory(string sourceDir, string destDir, List<string> logEntries)
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

        static string GetRelativePath(string relativeTo, string path)
        {
            Uri fromUri = new Uri(relativeTo);
            Uri toUri = new Uri(path);

            if (fromUri.Scheme != toUri.Scheme) { return path; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
 */
