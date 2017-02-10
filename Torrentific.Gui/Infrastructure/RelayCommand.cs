// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="RelayCommand.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Windows.Input;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Represents RelayCommand that uses the WPF CommandManager
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public sealed class RelayCommand : ICommand
    {
        /// <summary>
        /// The can execute method
        /// </summary>
        private readonly Func<bool> _canExecuteMethod;
        /// <summary>
        /// The execute method
        /// </summary>
        private readonly Action _executeMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="System.ArgumentNullException">executeMethod</exception>
        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod = null)
        {
            if (executeMethod == null)
                throw new ArgumentNullException("executeMethod");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can
        /// be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can
        /// be set to null.</param>
        public void Execute(object parameter)
        {
            if (_executeMethod != null)
                _executeMethod();
        }
    }

    /// <summary>
    /// Class RelayCommand. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public sealed class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// The can execute method
        /// </summary>
        private readonly Predicate<T> _canExecuteMethod;
        /// <summary>
        /// The execute method
        /// </summary>
        private readonly Action<T> _executeMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="System.ArgumentNullException">executeMethod</exception>
        public RelayCommand(Action<T> executeMethod, Predicate<T> canExecuteMethod = null)
        {
            if (executeMethod == null)
                throw new ArgumentNullException("executeMethod");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can
        /// be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod((T) parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can
        /// be set to null.</param>
        public void Execute(object parameter)
        {
            _executeMethod((T) parameter);
        }
    }
}