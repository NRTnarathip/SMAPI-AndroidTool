using Spectre.Console.Cli;

namespace SMAPI_AndroidTool;

partial class Program
{
    private static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.SetApplicationName("smapi.droid");
            config.AddCommand<UpFilesCommand>("upfiles")
              .WithDescription("Upload files with patterns");
            config.AddCommand<AppCommand>("app")
              .WithDescription("Interactive with app, such start or stop");
            config.AddCommand<AdbWifiCommand>("adb-wifi")
              .WithDescription("ADB Wifi Auto Connect");
        });

        int runResult = app.Run(args);

        return runResult;
    }
}