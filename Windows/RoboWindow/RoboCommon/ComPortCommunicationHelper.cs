﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComPortCommunicationHelper.cs" company="Dzakhov's jag">
//   Copyright © Dmitry Dzakhov 2013
// </copyright>
// <summary>
//   Implementation of COM-port communication between PC and robot's controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RoboCommon
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementation of COM-port communication between PC and robot's controller.
    /// </summary>
    /// <remarks>
    /// This type of communication is not necessary in ordinary robot's functioning.
    /// It is used only in debug and research purposes.
    /// </remarks>
    public sealed class ComPortCommunicationHelper : CommunicationHelper
    {
        /// <summary>
        /// COM-port object.
        /// </summary>
        private SerialPort serialPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComPortCommunicationHelper" /> class.
        /// </summary>
        /// <param name="portName">COM-port name.</param>
        /// <param name="baudRate">COM-port communication speed.</param>
        /// <param name="nonrecurrentMessageRepetitions">Number of repetitions we send nonrecurrent messages to the robot.</param>
        public ComPortCommunicationHelper(string portName, int baudRate, int nonrecurrentMessageRepetitions)
            : base(nonrecurrentMessageRepetitions)
        {
            this.PortName = portName;
            this.BaudRate = baudRate;

            this.serialPort = new SerialPort(this.PortName, this.BaudRate);
            this.serialPort.Open();
        }

        /// <summary>
        /// Gets COM-port name.
        /// </summary>
        public string PortName { get; private set; }

        /// <summary>
        /// Gets the port's speed.
        /// </summary>
        public int BaudRate { get; private set; }

        /// <summary>
        /// Internal method for message transmition throught COM-port.
        /// Doesn't correct message. Doesn't handle errors, just generate exceptions.
        /// </summary>
        /// <param name="message">
        /// Message to transmit.
        /// </param>
        protected override void TransmitMessage(string message)
        {
            // todo: Перейти на ресурсы.
            if (this.serialPort == null)
            {
                throw new AccessViolationException("Не инициализирован COM-порт.");
            }

            if (!this.serialPort.IsOpen)
            {
                throw new AccessViolationException("Не инициализирован COM-порт.");
            }

            this.serialPort.Write(message);
        }

        /// <summary>
        /// COM-port finalization.
        /// </summary>
        protected override void FinalizePort()
        {
            if (this.serialPort == null)
            {
                return;
            }

            if (this.serialPort.IsOpen)
            {
                this.serialPort.Close();
                this.serialPort = null;
            }
        }
    }
}
