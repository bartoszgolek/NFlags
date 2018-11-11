using System;
using NFlags.Commands;

namespace NFlags.GenericCommandExtension
{
    public class CommandConfigurator<TArguments>
    {
        private readonly CommandConfigurator _commandConfigurator;
        private readonly CommandRegisterer _commandRegisterer;

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

        public CommandConfigurator<TArguments> SetExecute(Func<TArguments, IOutput, int> execute)
        {
            _commandConfigurator.SetExecute(
                (args, output) => execute(Build(args), output));

            return this;
        }

        public CommandConfigurator<TArguments> SetExecute(Action<TArguments, IOutput> execute)
        {
            _commandConfigurator.SetExecute(
                (args, output) =>
                {
                    execute(Build(args), output);
                });

            return this;
        }

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