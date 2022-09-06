using LivePatcher.Commands;
using System;
using System.IO;
using System.Linq;

namespace LivePatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var patchManager = new PatchManager();
            var variableMemory = new VariablesMemory();
            var commandsHandler = new CommandsHandler(patchManager, variableMemory);
            if(args.Length == 1)
            {
                string[] commands;
                try
                {
                    commands = File.ReadAllLines(args[0]);
                }
                catch
                {
                    Console.WriteLine($"Cannot open patch file '{args[0]}'");
                    return;
                }
                var commandsParser = new CommandsParser(commands, commandsHandler);
                try
                {
                    commandsParser.ExecuteCommands();
                }
                catch(InvalidOperationException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Specify patch file. Supported commands:");
                var commands = CommandsParser.GetCommandsInfo(commandsHandler);
                Console.WriteLine(string.Join('\n', commands.Select(command => $"{command.Key} - {command.Value}")));
            }
        }
    }
}
