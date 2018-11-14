using System;

namespace NFlags.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// Exception thrown, when trying to register more than one default sub command for command.
    /// </summary>
    public class TooManyDefaultCommandsException : Exception
    {
    }
}