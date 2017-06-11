//-----------------------------------------------------------------------
// <copyright file="HttpUrlReservationAccessRuleTest.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer.Tests
{
    using System;
    using System.Security.Principal;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HttpUrlReservationAccessRuleTest
    {
        [TestMethod]
        public void Constructor()
        {
            var rule = new HttpUrlReservationAccessRule(WindowsIdentity.GetCurrent().User, HttpUrlReservationAccessRights.Listen);

            rule.HttpUrlReservationAccessRights.Should().HaveFlag(HttpUrlReservationAccessRights.Listen);
            rule.IdentityReference.Should().Be(WindowsIdentity.GetCurrent().User);
        }
    }
}
