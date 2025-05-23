
using SMAPI_AndroidTool;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace SMAPI_AndroidTool;

public class UpFilesSettings : CommandSettings
{
    [CommandOption("--files <FILES>")]
    [Description("File patterns to upload (e.g., *.dll, *.xml)")]
    public string[] Files { get; set; }

    [CommandOption("-d|--dest <DESTDIR>")]
    [Description("Dest file dir")]
    public string DestDir { get; set; }
}


public class UpFilesCommand : Command<UpFilesSettings>
{
    public static List<string> SearchFiles(string[] patterns)
    {
        var results = new List<string>();

        foreach (var pattern in patterns)
        {
            var baseDir = Directory.GetCurrentDirectory();
            var searchPattern = pattern;

            // ถ้ามี path ย่อย เช่น "data/*.json"
            if (pattern.Contains(Path.DirectorySeparatorChar))
            {
                var parts = pattern.Split(Path.DirectorySeparatorChar);
                baseDir = Path.Combine(baseDir, Path.Combine(parts[..^1]));
                searchPattern = parts[^1];
            }

            if (Directory.Exists(baseDir))
            {
                var found = Directory.GetFiles(baseDir, searchPattern, SearchOption.TopDirectoryOnly);
                results.AddRange(found);
            }
        }

        return results.Distinct().ToList();
    }

    static void Log(params string[] args)
    {
        var text = string.Join(" ", args);
        Console.WriteLine(text);
    }

    public override int Execute(CommandContext context, UpFilesSettings settings)
    {
        AnsiConsole.MarkupLine("[green]Uploading files into AppData/files/... [/]");
        // assert
        settings.DestDir = settings.DestDir.Replace("\\", "/");

        // ready
        var files = SearchFiles(settings.Files);
        Log("Found files: " + files.Count);

        var destDirPath = Utils.GameFilesDir + "/" + settings.DestDir;

        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            if (fileInfo is null) continue;
            Log(" - file: " + fileInfo.FullName);
            var fileSrcPath = fileInfo.FullName;
            if (Utils.PushFile(fileSrcPath, destDirPath))
            {
                Log("Done push file to:", destDirPath);
            }
        }

        AnsiConsole.MarkupLine("[green] Successfully UpFiles Command [/]");

        return 0;
    }
}