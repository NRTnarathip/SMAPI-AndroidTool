using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAPI_AndroidTool;

class AppCommandSetting : CommandSettings
{
    [CommandOption("--start")]
    [Description("Force Start App")]
    public bool IsStart { get; set; }

    [CommandOption("--stop")]
    [Description("Force Stop App")]
    public bool IsStop { get; set; }

    [CommandOption("--restart | --re")]
    [Description("Restart app")]
    public bool IsRestart { get; set; }
}
internal class AppCommand : Command<AppCommandSetting>
{
    public static bool StartApp()
    {
        Console.WriteLine("Try start app...");
        var done = Adb.Run("shell am start -n \"abc.smapi.gameloader/crc64e91f1276c636690c.LauncherActivity\" --ez \"IsClickStartGame\" true");
        if (!done)
            return false;

        Console.WriteLine("Started App");
        return true;
    }
    public static bool StopApp()
    {
        Console.WriteLine("Try force stop app...");
        if (!Adb.Run("shell am force-stop \"abc.smapi.gameloader"))
            return false;

        Console.WriteLine("Stopped App");
        return true;
    }
    public static bool RestartApp()
    {
        Console.WriteLine("Try restart app...");
        if (StopApp() && StartApp())
        {
            Console.WriteLine("Restarted app");
            return true;
        }

        return false;
    }
    public override int Execute(CommandContext context, AppCommandSetting settings)
    {
        AnsiConsole.MarkupLine("[green]App Interaction Tool[/]");
        if (context.Arguments.Count <= 1)
        {
            AnsiConsole.MarkupLine("[red]args empty!![/]");
            return -1;
        }

        if (settings.IsRestart)
        {
            return RestartApp() ? 0 : -1;
        }
        else if (settings.IsStart)
        {
            return StartApp() ? 0 : -1;
        }
        else if (settings.IsStop)
        {
            return StopApp() ? 0 : -1;
        }
        AnsiConsole.MarkupLine("[green]Done App Interaction Tool[/]");

        return 0;
    }
}
