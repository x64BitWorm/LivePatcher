using System;
using System.Collections.Generic;
using System.Linq;

namespace LivePatcher.Commands
{
    public class CommandsParser
    {
        private object _handler;
        private List<string> _commands;
        private Dictionary<string, List<System.Reflection.MethodInfo>> _functions;

        public CommandsParser(IEnumerable<string> commands, object handler)
        {
            _handler = handler;
            _commands = commands.ToList();
            InitFunctions();
        }

        public void ExecuteCommands()
        {
            foreach(var command in _commands)
            {
                var (name, arguments) = SplitCommand(command);
                if (name is null)
                {
                    continue;
                }
                if (!_functions.ContainsKey(name))
                {
                    throw new InvalidOperationException($"Unknown command '{name}'");
                }
                var function = _functions[name].FirstOrDefault(method => method.GetParameters().Length == arguments.Length);
                if(function != null)
                {
                    try
                    {
                        function.Invoke(_handler, arguments);
                        continue;
                    }
                    catch
                    {
                        throw new InvalidOperationException($"Unable to execute '{name} {string.Join(',', arguments)}' command");
                    }
                }
                function = _functions[name].FirstOrDefault(method => method.GetParameters().FirstOrDefault()?.ParameterType == typeof(string[]));
                if (function != null)
                {
                    try
                    {
                        function.Invoke(_handler, new object[] { arguments });
                        continue;
                    }
                    catch
                    {
                        throw new InvalidOperationException($"Unable to execute '{name} {string.Join(',', arguments)}' universal command");
                    }
                }
                throw new InvalidOperationException($"Command '{name}' cannot have {arguments.Length} arguments");
            }
        }

        public static Dictionary<string, string> GetCommandsInfo(object handler)
        {
            var result = new Dictionary<string, string>();
            foreach (var method in handler.GetType().GetMethods())
            {
                var attributes = method.GetCustomAttributes(typeof(CommandAttribute), false);
                if (attributes.Length == 0)
                {
                    continue;
                }
                var attribute = attributes[0] as CommandAttribute;
                if(!result.ContainsKey(attribute.Label))
                {
                    result.Add(attribute.Label, attribute.Description);
                }
            }
            return result;
        }

        private (string, string[]) SplitCommand(string command)
        {
            var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                return (null, null);
            }
            return (parts[0], parts[1..]);
        }

        private void InitFunctions()
        {
            _functions = new();
            foreach(var method in _handler.GetType().GetMethods())
            {
                var attributes = method.GetCustomAttributes(typeof(CommandAttribute), false);
                if (attributes.Length == 0)
                {
                    continue;
                }
                var attribute = attributes[0] as CommandAttribute;
                var label = attribute.Label;
                if(!_functions.ContainsKey(label))
                {
                    _functions.Add(label, new());
                }
                _functions[label].Add(method);
            }
        }
    }
}
