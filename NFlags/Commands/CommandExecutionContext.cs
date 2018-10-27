using System;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents configured command ready to execute
    /// </summary>
    public class CommandExecutionContext
    {
        /// <summary>
        /// </summary>
        /// <param name="execute">Function to execute command</param>
        /// <param name="args">Command args</param>
        public CommandExecutionContext(Func<CommandArgs, IOutput, int> execute, CommandArgs args)
        {
            Execute = execute;
            Args = args;
        }

        /// <summary>
        /// Command execution function
        /// </summary>
        public Func<CommandArgs, IOutput, int> Execute { get; }

        /// <summary>
        /// Command arguments
        /// </summary>
        public CommandArgs Args { get; }
    }
}