using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAPI_AndroidTool;

internal static class Utils
{
    public const string GameFullPath = "/storage/emulated/0/Android/data/abc.smapi.gameloader";
    public const string GameFilesDir = GameFullPath + "/files";

    public static bool PushFile(string localFilePath, string remotePath)
    {
        remotePath = remotePath.Replace("\\", "");

        Console.WriteLine("try copy file: " + localFilePath);
        Console.WriteLine(" to: " + remotePath);
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "adb",
            Arguments = $"push \"{localFilePath}\" \"{remotePath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        bool anyError = false;
        using (Process process = Process.Start(psi))
        {
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                AnsiConsole.MarkupLine("[red]ADB Error:\n" + error + "[/]");
                anyError = true;
            }
        }

        return !anyError;
    }
}
