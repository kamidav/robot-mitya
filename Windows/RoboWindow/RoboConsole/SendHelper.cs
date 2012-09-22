﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SendHelper.cs" company="Dzakhov's jag">
//   Copyright © Dmitry Dzakhov 2012
// </copyright>
// <summary>
//   Набор функций для управления отправкой сообщений.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RoboConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using RoboCommon;

    /// <summary>
    /// Набор функций для управления отправкой сообщений.
    /// </summary>
    public static class SendHelper
    {
        /// <summary>
        /// История команд.
        /// </summary>
        private static CommandHistory commandHistory = new CommandHistory();

        /// <summary>
        /// Обработка введённых команд.
        /// </summary>
        /// <param name="commandLineTextBox">
        /// Поле ввода команд.
        /// </param>
        /// <param name="outputTextBox">
        /// Поле вывода результатов.
        /// </param>
        /// <param name="robotHelper">
        /// Объект, реализующий взаимодействие с роботом.
        /// </param>
        public static void CommandLineProcessor(TextBox commandLineTextBox, TextBox outputTextBox, IRobotHelper robotHelper)
        {
            commandHistory.Add(commandLineTextBox.Text);
            
            IEnumerable<string> commands = GetCommands(commandLineTextBox.Text);

            var notSentCommands = new List<string>();
            foreach (string command in commands)
            {
                // Пустышки в сторону!
                if (command.Trim().Equals(string.Empty))
                {
                    continue;
                }

                // Если есть ошибка, то все последующие команды не посылаются роботу.
                if (notSentCommands.Count > 0)
                {
                    notSentCommands.Add(command);
                }
                else
                {
                    bool sendResult = robotHelper.SendMessageToRobot(command);
                    if (sendResult)
                    {
                        outputTextBox.AppendText(robotHelper.LastSentMessage);
                    }
                    else
                    {
                        outputTextBox.AppendText(string.Format("Ошибка в {0}: {1}", command, robotHelper.LastErrorMessage));
                        notSentCommands.Add(command);
                    }

                    outputTextBox.AppendText(Environment.NewLine);
                }
            }

            // В поле ввода команд оставляем только неотправленные команды.
            if (notSentCommands.Count > 0)
            {
                commandLineTextBox.Text = GenerateCommandLine(notSentCommands);
            }
            else
            {
                commandLineTextBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Выбор и ввод в поле ввода предыдущей команды из истории команд.
        /// </summary>
        /// <param name="commandLineTextBox">
        /// Поле ввода команд.
        /// </param>
        public static void SelectPreviousCommand(TextBox commandLineTextBox)
        {
            commandLineTextBox.Text = commandHistory.GetPreviousCommand();
        }

        /// <summary>
        /// Выбор и ввод в поле ввода следующей команды из истории команд.
        /// </summary>
        /// <param name="commandLineTextBox">
        /// Поле ввода команд.
        /// </param>
        public static void SelectNextCommand(TextBox commandLineTextBox)
        {
            commandLineTextBox.Text = commandHistory.GetNextCommand();
        }

        /// <summary>
        /// Вычленение списка команд из строки ввода.
        /// </summary>
        /// <param name="commandLineText">
        /// Строка ввода. Может содержать несколько команд, разделённых запятой.
        /// </param>
        /// <returns>
        /// Список команд.
        /// </returns>
        private static IEnumerable<string> GetCommands(string commandLineText)
        {
            return commandLineText.Split(',').Select(x => x.Trim()).ToArray();
        }

        /// <summary>
        /// Получение строки из команд, разделённых запятыми.
        /// </summary>
        /// <param name="commands">
        /// Список команд.
        /// </param>
        /// <returns>
        /// Строка команд, разделённых запятыми.
        /// </returns>
        private static string GenerateCommandLine(IEnumerable<string> commands)
        {
            string result = string.Empty;
            const string CommandSeparator = ", ";
            foreach (string command in commands)
            {
                result += CommandSeparator + command;
            }

            if (result.Length > 0)
            {
                result = result.Remove(0, CommandSeparator.Length);
            }

            return result;
        }
    }
}