using System;
using System.IO;

namespace Core.Utils;

// Odredjuje gde su fajlovi sa podacima: folder "Data" pored .sln/.slnx fajla,
// tako da konzolna i (kasnije) WPF aplikacija koriste iste fajlove.
// Putanje se grade preko Path.DirectorySeparatorChar (zahtev iz specifikacije).
public static class DataPaths {
    private const string DataFolderName = "Data";

    private static string? _dataFolder;

    public static string DataFolder {
        get {
            if (_dataFolder == null) {
                _dataFolder = FindDataFolder();
                Directory.CreateDirectory(_dataFolder);
            }
            return _dataFolder;
        }
    }

    public static string GetFilePath(string fileName) {
        return DataFolder + Path.DirectorySeparatorChar + fileName;
    }

    private static string FindDataFolder() {
        // Krece se od foldera u kom je .exe i ide se navise do foldera sa .sln/.slnx fajlom.
        DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null) {
            if (dir.GetFiles("*.sln").Length > 0 || dir.GetFiles("*.slnx").Length > 0)
                return dir.FullName + Path.DirectorySeparatorChar + DataFolderName;

            dir = dir.Parent;
        }

        // Ako resenje nije pronadjeno, koristi se folder pored .exe fajla.
        return AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar)
               + Path.DirectorySeparatorChar + DataFolderName;
    }
}
