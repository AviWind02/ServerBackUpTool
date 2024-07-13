using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MinecraftCreateBackZips
{
    internal class BackupManager
    {

        private readonly string _sourceDir;
        private readonly string _backupDir;
        private readonly string _logDir;
        private readonly string _dailyBackupDir;
        private readonly Timer _timer;

        public BackupManager(string sourceDir, string backupDir, string logDir, string dailyBackupDir)
        {
            _sourceDir = sourceDir;
            _backupDir = backupDir;
            _logDir = logDir;
            _dailyBackupDir = dailyBackupDir;

            Directory.CreateDirectory(_backupDir);
            Directory.CreateDirectory(_logDir);
            Directory.CreateDirectory(_dailyBackupDir);

            _timer = new Timer();
            _timer.Interval = GetIntervalToNextHour();
            _timer.Elapsed += TimerElapsed;
        }

        public void StartHourlyBackup()
        {
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            CreateBackup();

            if (DateTime.Now.Hour == 23) // End of the day at 11 PM
            {
                MoveLastDailyBackup();
                DeleteOldBackups();
            }
        }

        private void CreateBackup()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string tempBackupDir = Path.Combine(_backupDir, $"Backup_{timestamp}");
            string zipFileName = $"Backup_{timestamp}.zip";
            string zipFilePath = Path.Combine(_backupDir, zipFileName);
            string logFilePath = Path.Combine(_logDir, $"BackupLog_{timestamp}.txt");

            List<string> logEntries = new List<string>();

            Console.WriteLine($"Backup started at {DateTime.Now}");
            logEntries.Add($"Backup started at {DateTime.Now}");

            try
            {
                Directory.CreateDirectory(tempBackupDir);
                DirectoryHelper.CopyDirectory(_sourceDir, tempBackupDir, logEntries);

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

        private void MoveLastDailyBackup()
        {
            var backups = Directory.GetFiles(_backupDir, "*.zip")
                                   .OrderByDescending(f => f)
                                   .ToList();

            if (backups.Count > 0)
            {
                string lastBackup = backups.First();
                string destFilePath = Path.Combine(_dailyBackupDir, Path.GetFileName(lastBackup));

                File.Copy(lastBackup, destFilePath, true);
                Console.WriteLine($"Moved last backup of the day to daily folder: {destFilePath}");
            }
        }

        private void DeleteOldBackups()
        {
            var backups = Directory.GetFiles(_backupDir, "*.zip")
                                   .OrderBy(f => f)
                                   .ToList();

            if (backups.Count > 48)
            {
                foreach (var backup in backups.Take(backups.Count - 48))
                {
                    File.Delete(backup);
                    Console.WriteLine($"Deleted old backup: {backup}");
                }
            }
        }

        private double GetIntervalToNextHour()
        {
            DateTime nextHour = DateTime.Now.AddHours(1).Date.AddHours(DateTime.Now.Hour + 1);
            return (nextHour - DateTime.Now).TotalMilliseconds;
        }

    }
}
