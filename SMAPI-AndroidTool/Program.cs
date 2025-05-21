using Spectre.Console.Cli;

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
        });

        app.Run(args);

        Console.ReadLine();
        return 0;
    }
}