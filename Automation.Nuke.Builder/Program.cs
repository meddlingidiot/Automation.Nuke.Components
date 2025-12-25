using System.CommandLine;
using Automation.Nuke.Builder.Commands;

namespace Automation.Nuke.Builder;

class Program
{
    static int Main(string[] args)
    {
        var rootCommand = new RootCommand("Automation Nuke Builder - Simplify Nuke build setup");

        var setupCommand = new Command("setup", "Setup Nuke build for the current project");
        setupCommand.SetAction(async (parseResult, cancellationToken) =>
        {
            return await SetupCommand.ExecuteAsync();
        });

        rootCommand.Subcommands.Add(setupCommand);

        return rootCommand.Parse(args).Invoke();
    }
}
