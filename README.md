# ServerBackUpTool

ServerBackUpTool is a utility for creating hourly backups of Me and my friends Minecraft server(Can work on other games too). It ensures that the latest backups are available and maintains a clean backup directory by keeping only the most recent backups. Additionally, it stores a daily backup of the last backup of each day.

## Features

- **Hourly Backups**: Creates a backup every hour.
- **Daily Backups**: Moves the last backup of each day to a separate daily backup folder.
- **Retention Policy**: Keeps hourly backups for the last 48 hours and daily backups indefinitely.



## Usage

1. Update the source and backup directory paths in the `BackupManager` class constructor in `Program.cs`:
    ```csharp
    BackupManager backupManager = new BackupManager(
        sourceDir: @"C:\Server\Minecraft",
        backupDir: @"C:\Server\MC backup",
        logDir: @"C:\Server\MC backup\logs",
        dailyBackupDir: @"C:\Server\MC backup\daily"
    );
    ```

## Directory Structure

- **sourceDir**: The directory containing the Minecraft server files.
- **backupDir**: The directory where hourly backups are stored.
- **logDir**: The directory where log files are stored.
- **dailyBackupDir**: The directory where daily backups are stored.

## How It Works

1. **Hourly Backups**: The tool uses a timer to trigger a backup every hour. The backups are stored as ZIP files in the `backupDir`.
2. **Daily Backups**: At 11 PM each day, the tool moves the last backup of the day to the `dailyBackupDir`.
3. **Retention Policy**: The tool ensures that only the last 48 hourly backups are kept in the `backupDir` by deleting older backups.

## Logging

Each backup operation logs its activity to a log file in the `logDir`. The log file name includes the timestamp of the backup operation.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

