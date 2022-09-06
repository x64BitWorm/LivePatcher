using System;

namespace LivePatcher.Commands
{
    public class CommandAttribute: Attribute
    {
        public string Label { get; private set; }
        public string Description { get; private set; }

        public CommandAttribute(string label, string description)
        {
            Label = label;
            Description = description;
        }
    }
}
