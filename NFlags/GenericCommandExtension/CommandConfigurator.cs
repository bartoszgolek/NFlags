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

        public void SetExecute(Func<TArguments, IOutput, int> execute)
        {
            _commandConfigurator.SetExecute(
                (args, output) => execute(Build(args), output));
        }

        public void SetExecute(Action<TArguments, IOutput> execute)
        {
            _commandConfigurator.SetExecute(
                (args, output) =>
                {
                    execute(Build(args), output);
                });
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