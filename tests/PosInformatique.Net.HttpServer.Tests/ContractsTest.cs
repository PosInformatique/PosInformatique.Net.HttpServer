//-----------------------------------------------------------------------
// <copyright file="ContractsTest.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ContractsTest
    {
        [TestMethod]
        public void IsNotNull()
        {
            Contracts.IsNotNull("Not null", "The argument");
        }

        [TestMethod]
        public void IsNotNullException()
        {
            Action code = () => Contracts.IsNotNull((string)null, "The argument");

            code.ShouldThrow<ArgumentNullException>().Which
                .ParamName.Should().Be("The argument");
        }
    }
}
