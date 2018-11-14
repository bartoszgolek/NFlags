using System;
using NFlags.Commands;

namespace NFlags.GenericCommandExtension
{
    /// <summary>
    /// Represents Command configuration ability. Used when registering arguments trough custom args type.
    /// </summary>
    /// <typeparam name="TArguments">Type od custom args type</typeparam>
    public class CommandConfigurator<TArguments>
    {
        private readonly CommandConfigurator _commandConfigurator;
        private readonly CommandRegisterer _commandRegisterer;

        /// <summary>
        /// Creates new instance of CommandConfigurator&lt;TArguments&gt;
        /// </summary>
        /// <param name="commandConfigurator">Command configurator used internally to configure command.</param>
        public CommandConfigurator(CommandConfigurator commandConfigurator)
        {
            _commandConfigurator = commandConfigurator;
            _commandRegisterer = new CommandRegisterer(commandConfigurator);
        }

        internal Command CreateCommand()
        {
            RegisterArguments();
            return _commandConfigurator.CreateCommand();
        }

        /// <summary>
        /// Sets function to execute when command is called. Returns exit code.
        /// </summary>
        /// <param name="execute">Function invoked when executing command. Returns exit code.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator<TArguments> SetExecute(Func<TArguments, IOutput, int> execute)
        {
            _commandConfigurator.SetExecute(
                (args, output) => execute(Build(args), output));

            return this;
        }

        /// <summary>
        /// Sets action to execute when command is called.
        /// </summary>
        /// <param name="execute">Action invoked when executing command.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator<TArguments> SetExecute(Action<TArguments, IOutput> execute)
        {
            _commandConfigurator.SetExecute(
                (args, output) =>
                {
                    execute(Build(args), output);
                });

            return this;
        }

        /// <summary>
        /// Configures command to print help when executed. Useful when command is a bucket to group sub commands and not require implementation.
        /// </summary>
        /// <returns>Self instance</returns>
        public CommandConfigurator<TArguments> PrintHelpOnExecute()
        {
            _commandConfigurator.PrintHelpOnExecute();

            return this;
        }

        /// <summary>
        /// Register sub command for the command
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="description">Command description for help.</param>
        /// <param name="configureCommand">Command configuration callback</param>
        /// <typeparam name="TCommandArguments">Type of callback arguments type.</typeparam>
        /// <returns>Self instance</returns>
        public CommandConfigurator<TArguments> RegisterCommand<TCommandArguments>(string name, string description,
            Action<CommandConfigurator<TCommandArguments>> configureCommand)
        {
            _commandConfigurator.RegisterCommand(
                name,
                description,
                configurator =>
                {
                    var commandConfigurator = new CommandConfigurator<TCommandArguments>(configurator);
                    configureCommand.Invoke(
                        commandConfigurator
                    );
                    commandConfigurator.RegisterArguments();
                });

            return this;
        }

        private void RegisterArguments()
        {
            ProcessTArgsTemplate.Process<TArguments>(
                _commandRegisterer.RegisterFlag,
                _commandRegisterer.RegisterOption,
                _commandRegisterer.RegisterParameter,
                _commandRegisterer.RegisterParameterSeries
            );
        }

        private static TArguments Build(CommandArgs args)
        {
            var tArgs = (TArguments)Activator.CreateInstance(typeof(TArguments));
            var argSetter = new GenericArgsBuilder<TArguments>(args, tArgs);

            ProcessTArgsTemplate.Process<TArguments>(
                argSetter.SetFlagValue,
                argSetter.SetOptionValue,
                argSetter.SetParameterValue,
                argSetter.SetParameterSeriesValues
            );

            return tArgs;
        }
    }
}