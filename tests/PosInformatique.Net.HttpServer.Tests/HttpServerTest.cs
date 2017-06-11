//-----------------------------------------------------------------------
// <copyright file="HttpServerTest.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer.Tests
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HttpServerTest
    {
        [TestMethod]
        public void UrlReservations()
        {
            var rule = new HttpUrlReservationAccessRule(WindowsIdentity.GetCurrent().User, HttpUrlReservationAccessRights.ListenAndDelegate);

            // Add an URL reservation
            var urlReservation = HttpServer.RegisterUrlReservation("http://localhost:1664/", rule);

            // Find it
            var urlReservationFound = HttpServer.GetUrlReservations().Single(ur => ur.Url == "http://localhost:1664/");
            urlReservationFound.Should().NotBeNull();

            // Check the access
            var accessRules = urlReservation.GetAccessRules(true, true, typeof(NTAccount));
            accessRules.Should().HaveCount(1);
            ((HttpUrlReservationAccessRule)accessRules[0]).HttpUrlReservationAccessRights.Should().Be(HttpUrlReservationAccessRights.ListenAndDelegate);
            ((HttpUrlReservationAccessRule)accessRules[0]).IdentityReference.Should().Be(WindowsIdentity.GetCurrent().User.Translate(typeof(NTAccount)));

            // Delete the URL reservation
            urlReservationFound.Delete();

            HttpServer.GetUrlReservations().Count(ur => ur.Url == "http://localhost:1664/")
                .Should().Be(0);
        }

        [TestMethod]
        public void RegisterUrlReservation_WithNoUrl()
        {
            Action code = () => HttpServer.RegisterUrlReservation(null, null);

            code.ShouldThrow<ArgumentNullException>().Which
                .ParamName.Should().Be("url");
        }

        [TestMethod]
        public void RegisterUrlReservation_WithNoRule()
        {
            Action code = () => HttpServer.RegisterUrlReservation("http://localhost:1664/", null);

            code.ShouldThrow<ArgumentNullException>().Which
                .ParamName.Should().Be("rule");
        }
    }
}
