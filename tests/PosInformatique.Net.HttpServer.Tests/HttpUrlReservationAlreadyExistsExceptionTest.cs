//-----------------------------------------------------------------------
// <copyright file="HttpUrlReservationAlreadyExistsExceptionTest.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer.Tests
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HttpUrlReservationAlreadyExistsExceptionTest
    {
        [TestMethod]
        public void Constructor()
        {
            var lastErrorCode = Marshal.GetLastWin32Error();
            var exception = new HttpUrlReservationAlreadyExistsException();

            exception.NativeErrorCode.Should().Be(lastErrorCode);
            exception.Message.Should().Be(new Win32Exception(lastErrorCode).Message);
        }

        [TestMethod]
        public void Constructor_WithMessage()
        {
            var lastErrorCode = Marshal.GetLastWin32Error();
            var exception = new HttpUrlReservationAlreadyExistsException("The message");

            exception.NativeErrorCode.Should().Be(lastErrorCode);
            exception.Message.Should().Be("The message");
        }

        [TestMethod]
        public void Constructor_WithErrorCode()
        {
            var exception = new HttpUrlReservationAlreadyExistsException(5);

            exception.NativeErrorCode.Should().Be(5);
            exception.Message.Should().Be("Accès refusé");
        }

        [TestMethod]
        public void Constructor_WithErrorCodeAndMessage()
        {
            var exception = new HttpUrlReservationAlreadyExistsException(5, "The message");

            exception.NativeErrorCode.Should().Be(5);
            exception.Message.Should().Be("The message");
        }

        [TestMethod]
        public void Constructor_WithWin32ErrorCodeAndMessage()
        {
            var exception = new HttpUrlReservationAlreadyExistsException((Win32Error)5, "The message");

            exception.NativeErrorCode.Should().Be(5);
            exception.Message.Should().Be("The message");
        }

        [TestMethod]
        public void Serialization()
        {
            var exception = new HttpUrlReservationAlreadyExistsException((Win32Error)5, "The message");

            exception.Should().BeBinarySerializable();
        }
    }
}
