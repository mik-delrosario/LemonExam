using System;

namespace LemonExam.Infrastructure
{
    #region Command Interfaces

    public interface ICommand : ICommand <UnitType> { }
    public interface ICommand<out TResult> { }
    public interface IHandler { }       //For future expansion
    public interface IResultHandler { } //For future expansion

    #endregion

    #region CommandHandler Interfaces

    public interface ICommandHandler<in TCommand> : IHandler where TCommand : ICommand {
        void Handle(TCommand command);
    }

    public interface ICommandHandler<in TCommand, out TResult> : IHandler where TCommand : ICommand<TResult> {
        TResult Handle(TCommand command);
    }

    public interface ICommandResultHandler<in TCommand, out TResult> : IResultHandler {
        TResult Handle(TCommand command);
    }

    #endregion

    #region CommandHandler Methods

    public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand> where TCommand : ICommand {
        public abstract void Handle(TCommand command);
    }

    public abstract class CommandResultHandler<TCommand, TResult> : ICommandResultHandler<TCommand, TResult> {
        public abstract TResult Handle(TCommand command);
    }

    #endregion
}
