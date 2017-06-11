//-----------------------------------------------------------------------
// <copyright file="HttpUrlReservationTest.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Text;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HttpUrlReservationTest
    {
        [TestMethod]
        public void Constructor()
        {
            var urlReservation = new HttpUrlReservation("The url reservation", new HttpUrlReservationAccessRule[0]);

            urlReservation.Url.Should().Be("The url reservation");
            urlReservation.AccessRightType.Should().Be(typeof(HttpUrlReservationAccessRights));
            urlReservation.AccessRuleType.Should().Be(typeof(HttpUrlReservationAccessRule));
        }

        [TestMethod]
        public void AuditRuleType()
        {
            var urlReservation = new HttpUrlReservation("The url reservation", new HttpUrlReservationAccessRule[0]);

            urlReservation.Invoking(ur => { var type = ur.AuditRuleType; })
                .ShouldThrow<NotSupportedException>();
        }

        [TestMethod]
        public void AccessRuleFactory()
        {
            var urlReservation = new HttpUrlReservation("The url reservation", new HttpUrlReservationAccessRule[0]);

            var rule = (HttpUrlReservationAccessRule)urlReservation.AccessRuleFactory(WindowsIdentity.GetCurrent().User, (int)HttpUrlReservationAccessRights.Listen, true, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow);

            rule.IdentityReference.Should().Be(WindowsIdentity.GetCurrent().User);
            rule.HttpUrlReservationAccessRights.Should().Be(HttpUrlReservationAccessRights.Listen);
        }

        [TestMethod]
        public void AuditRuleFactory()
        {
            var urlReservation = new HttpUrlReservation("The url reservation", new HttpUrlReservationAccessRule[0]);

            urlReservation.Invoking(ur => ur.AuditRuleFactory(null, 0, true, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None))
                .ShouldThrow<NotSupportedException>();
        }

        [TestMethod]
        public new void ToString()
        {
            var urlReservation = new HttpUrlReservation("The url reservation", new HttpUrlReservationAccessRule[0]);

            Assert.AreEqual("The url reservation", urlReservation.ToString());
        }

        [TestMethod]
        public void Delete_NotFoundUrl()
        {
            var rule = new HttpUrlReservationAccessRule(WindowsIdentity.GetCurrent().User, HttpUrlReservationAccessRights.ListenAndDelegate);

            // Add an URL reservation
            var urlReservation = new HttpUrlReservation("http://localhost:1664/", new[] { rule });

            urlReservation.Invoking(ur => ur.Delete())
                .ShouldThrow<HttpUrlReservationNotFoundException>()
                .Where(ex => ex.NativeErrorCode == (int)Win32Error.ERROR_FILE_NOT_FOUND)
                .WithMessage("The 'http://localhost:1664/' URL to delete has not been registered in the HTTP server.");
        }

        [TestMethod]
        public void AddAccessRule()
        {
            var rule = new HttpUrlReservationAccessRule(WindowsIdentity.GetCurrent().User, HttpUrlReservationAccessRights.ListenAndDelegate);

            // Add an URL reservation
            var urlReservation = HttpServer.RegisterUrlReservation("http://localhost:1664/", rule);

            try
            {
                // Add an access rule for "everyone" for Delegate access
                urlReservation.AddAccessRule(new HttpUrlReservationAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), HttpUrlReservationAccessRights.Delegate));

                // Check that the "current user" and "everyone" rights has been registered
                urlReservation = HttpServer.GetUrlReservations().Single(ur => ur.Url == "http://localhost:1664/");

                var accessRules = urlReservation.GetAccessRules(true, true, typeof(NTAccount));
                accessRules.Should().HaveCount(2);

                ((HttpUrlReservationAccessRule)accessRules[0]).HttpUrlReservationAccessRights.Should().Be(HttpUrlReservationAccessRights.Delegate);
                ((HttpUrlReservationAccessRule)accessRules[0]).IdentityReference.Should().Be(new SecurityIdentifier(WellKnownSidType.WorldSid, null).Translate(typeof(NTAccount)));

                ((HttpUrlReservationAccessRule)accessRules[1]).HttpUrlReservationAccessRights.Should().Be(HttpUrlReservationAccessRights.ListenAndDelegate);
                ((HttpUrlReservationAccessRule)accessRules[1]).IdentityReference.Should().Be(WindowsIdentity.GetCurrent().User.Translate(typeof(NTAccount)));
            }
            finally
            {
                // Delete the URL reservation
                urlReservation.Delete();
            }
        }

        [TestMethod]
        public void AddAccessRule_NotExist()
        {
            var rule = new HttpUrlReservationAccessRule(WindowsIdentity.GetCurrent().User, HttpUrlReservationAccessRights.ListenAndDelegate);

            // Add an URL reservation
            var urlReservation = HttpServer.RegisterUrlReservation("http://localhost:1664/", rule);

            urlReservation.Delete();

            urlReservation.Invoking(ur => ur.AddAccessRule(new HttpUrlReservationAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), HttpUrlReservationAccessRights.Delegate)))
                .ShouldThrow<HttpUrlReservationNotFoundException>()
                .Where(e => e.Message == "The 'http://localhost:1664/' URL to update has not been registered in the HTTP server.")
                .Where(e => e.NativeErrorCode == 2);
        }

        [TestMethod]
        public void RemoveAccessRule()
        {
            var rule = new HttpUrlReservationAccessRule(WindowsIdentity.GetCurrent().User, HttpUrlReservationAccessRights.ListenAndDelegate);

            // Add an URL reservation
            var urlReservation = HttpServer.RegisterUrlReservation("http://localhost:1664/", rule);

            try
            {
                // Add an access rule for "everyone" for Delegate access
                urlReservation.AddAccessRule(new HttpUrlReservationAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), HttpUrlReservationAccessRights.Delegate));

                // Check that the "current user" and "everyone" rights has been registered
                urlReservation = HttpServer.GetUrlReservations().Single(ur => ur.Url == "http://localhost:1664/");

                // Remove the access rule of "everyone"
                var result = urlReservation.RemoveAccessRule(new HttpUrlReservationAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), HttpUrlReservationAccessRights.Delegate));
                result.Should().Be(true);

                // Check that only the "current user" rights has been registred only
                urlReservation = HttpServer.GetUrlReservations().Single(ur => ur.Url == "http://localhost:1664/");

                var accessRules = urlReservation.GetAccessRules(true, true, typeof(NTAccount));
                accessRules.Should().HaveCount(1);

                ((HttpUrlReservationAccessRule)accessRules[0]).HttpUrlReservationAccessRights.Should().Be(HttpUrlReservationAccessRights.ListenAndDelegate);
                ((HttpUrlReservationAccessRule)accessRules[0]).IdentityReference.Should().Be(WindowsIdentity.GetCurrent().User.Translate(typeof(NTAccount)));
            }
            finally
            {
                // Delete the URL reservation
                urlReservation.Delete();
            }
        }

        [TestMethod]
        public void RemoveAccessRule_NotExist()
        {
            var rule = new HttpUrlReservationAccessRule(WindowsIdentity.GetCurrent().User, HttpUrlReservationAccessRights.ListenAndDelegate);

            // Add an URL reservation
            var urlReservation = HttpServer.RegisterUrlReservation("http://localhost:1664/", rule);

            urlReservation.Delete();

            urlReservation.Invoking(ur => ur.RemoveAccessRule(new HttpUrlReservationAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), HttpUrlReservationAccessRights.Delegate)))
                .ShouldThrow<HttpUrlReservationNotFoundException>()
                .Where(e => e.Message == "The 'http://localhost:1664/' URL to update has not been registered in the HTTP server.")
                .Where(e => e.NativeErrorCode == 2);
        }
    }
}
