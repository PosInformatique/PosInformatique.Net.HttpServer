//-----------------------------------------------------------------------
// <copyright file="HttpUrlReservation.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an URL reservation for the Windows HTTP Server.
    /// </summary>
    public sealed class HttpUrlReservation : CommonObjectSecurity
    {
        /// <summary>
        /// URL of the reservation.
        /// </summary>
        private readonly string url;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUrlReservation"/> class.
        /// </summary>
        /// <param name="url">URL of the reservation.</param>
        /// <param name="accessRules">Access rules of the URL reservation.</param>
        internal HttpUrlReservation(string url, IReadOnlyCollection<HttpUrlReservationAccessRule> accessRules)
            : base(false)
        {
            this.url = url;

            foreach (var rule in accessRules)
            {
                base.AddAccessRule(rule);
            }
        }

        /// <summary>
        /// Gets the URL of the reservation.
        /// </summary>
        public string Url
        {
            get
            {
                return this.url;
            }
        }

        /// <inheritdoc />
        public override Type AccessRightType
        {
            get { return typeof(HttpUrlReservationAccessRights); }
        }

        /// <inheritdoc />
        public override Type AccessRuleType
        {
            get { return typeof(HttpUrlReservationAccessRule); }
        }

        /// <inheritdoc />
        public override Type AuditRuleType
        {
            get { throw new NotSupportedException(HttpServerResources.UrlReservationDoesNotSupportAuditRules);  }
        }

        /// <inheritdoc />
        public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
        {
            return new HttpUrlReservationAccessRule(identityReference, (HttpUrlReservationAccessRights)accessMask);
        }

        /// <inheritdoc />
        public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
        {
            throw new NotSupportedException(HttpServerResources.UrlReservationDoesNotSupportAuditRules);
        }

        /// <summary>
        /// Deletes the current <see cref="HttpUrlReservation"/>.
        /// </summary>
        /// <exception cref="HttpUrlReservationNotFoundException">If the current <see cref="HttpUrlReservation"/> does not still exist.</exception>
        /// <exception cref="HttpServerException">If an internal error has been occured (use the <see cref="Win32Exception.NativeErrorCode"/> for more information).</exception>
        public void Delete()
        {
            HttpServer.Initialize();

            try
            {
                var configInformation = new HttpServerApi.HTTP_SERVICE_CONFIG_URLACL_SET(this.url, null);

                var result = HttpServerApi.HttpDeleteServiceConfiguration(IntPtr.Zero, HttpServerApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, configInformation, Marshal.SizeOf(configInformation), IntPtr.Zero);

                if (result == Win32Error.ERROR_FILE_NOT_FOUND)
                {
                    throw new HttpUrlReservationNotFoundException(result, string.Format(CultureInfo.CurrentUICulture, HttpServerResources.UrlReservationToDeleteNotFound, this.url));
                }

                if (result != Win32Error.NO_ERROR)
                {
                    throw new HttpServerException(result);
                }
            }
            finally
            {
                HttpServer.Terminate();
            }
        }

        /// <summary>
        /// Adds the specified access rule to the Discretionary Access Control List(DACL) associated with this <see cref="HttpUrlReservation"/> object.
        /// </summary>
        /// <param name="rule">The access rule to add.</param>
        /// <exception cref="HttpUrlReservationNotFoundException">If the current <see cref="HttpUrlReservation"/> does not still exist.</exception>
        /// <exception cref="HttpServerException">If an internal error has been occured (use the <see cref="Win32Exception.NativeErrorCode"/> for more information).</exception>
        public void AddAccessRule(HttpUrlReservationAccessRule rule)
        {
            base.AddAccessRule(rule);
            this.Update();
        }

        /// <summary>
        /// Removes access rules that contain the same security identifier and access mask
        /// as the specified access rule from the Discretionary Access Control List (DACL)
        /// associated with this <see cref="HttpUrlReservation"/> object.
        /// </summary>
        /// <param name="rule">The access rule to remove.</param>
        /// <returns>true if the access rule was successfully removed; otherwise, false.</returns>
        /// <exception cref="HttpUrlReservationNotFoundException">If the current <see cref="HttpUrlReservation"/> does not still exist.</exception>
        /// <exception cref="HttpServerException">If an internal error has been occured (use the <see cref="Win32Exception.NativeErrorCode"/> for more information).</exception>
        public bool RemoveAccessRule(HttpUrlReservationAccessRule rule)
        {
            var result = base.RemoveAccessRule(rule);

            this.Update();

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Url;
        }

        /// <summary>
        /// Update the <see cref="HttpUrlReservationAccessRule"/> of the current <see cref="HttpUrlReservation"/>.
        /// </summary>
        /// <exception cref="HttpUrlReservationNotFoundException">If the current <see cref="HttpUrlReservation"/> does not still exist.</exception>
        /// <exception cref="HttpServerException">If an internal error has been occured (use the <see cref="Win32Exception.NativeErrorCode"/> for more information).</exception>
        private void Update()
        {
            HttpServer.Initialize();

            try
            {
                var ssdl = this.GetSecurityDescriptorSddlForm(AccessControlSections.Access);

                var configInformation = new HttpServerApi.HTTP_SERVICE_CONFIG_URLACL_SET(this.url, ssdl);

                // Delete the URL reservation record
                var result = HttpServerApi.HttpDeleteServiceConfiguration(IntPtr.Zero, HttpServerApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, configInformation, Marshal.SizeOf(configInformation), IntPtr.Zero);

                if (result == Win32Error.ERROR_FILE_NOT_FOUND)
                {
                    throw new HttpUrlReservationNotFoundException(result, string.Format(CultureInfo.CurrentUICulture, HttpServerResources.UrlReservationToUpdateNotFound, this.url));
                }

                // Add the URL reservation record with the new SDDL.
                result = HttpServerApi.HttpSetServiceConfiguration(IntPtr.Zero, HttpServerApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, configInformation, Marshal.SizeOf(configInformation), IntPtr.Zero);

                if (result != Win32Error.NO_ERROR)
                {
                    throw new HttpServerException(result);
                }
            }
            finally
            {
                HttpServer.Terminate();
            }
        }
    }
}
