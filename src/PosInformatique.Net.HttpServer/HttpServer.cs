//-----------------------------------------------------------------------
// <copyright file="HttpServer.cs" company="P.O.S Informatique">
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
    /// Allows to use and manage the configuration of Windows HTTP Server.
    /// </summary>
    public static class HttpServer
    {
        /// <summary>
        /// Register an URL reservation in the Windows HTTP Server.
        /// </summary>
        /// <param name="url">URL to register.</param>
        /// <param name="rule">Rule to apply to the URL to register.</param>
        /// <returns>An instance of <see cref="HttpUrlReservation"/> which allows to manage the URL reservation.</returns>
        public static HttpUrlReservation RegisterUrlReservation(string url, HttpUrlReservationAccessRule rule)
        {
            Contracts.IsNotNull(url, nameof(url));
            Contracts.IsNotNull(rule, nameof(rule));

            Initialize();

            try
            {
                // Create the ACL access
                var acl = new DiscretionaryAcl(false, false, 1);
                acl.AddAccess(rule.AccessControlType, (SecurityIdentifier)rule.IdentityReference, (int)rule.HttpUrlReservationAccessRights, rule.InheritanceFlags, rule.PropagationFlags);

                // Create the security descriptor and convert it to SDDL.
                var securityDescriptor = new CommonSecurityDescriptor(false, false, ControlFlags.DiscretionaryAclPresent, (SecurityIdentifier)rule.IdentityReference, null, null, acl);
                var ssdl = securityDescriptor.GetSddlForm(AccessControlSections.Access);

                var configInformation = new HttpServerApi.HTTP_SERVICE_CONFIG_URLACL_SET(url, ssdl);

                var result = HttpServerApi.HttpSetServiceConfiguration(IntPtr.Zero, HttpServerApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, configInformation, Marshal.SizeOf(configInformation), IntPtr.Zero);

                if (result == Win32Error.ERROR_ALREADY_EXISTS)
                {
                    throw new HttpUrlReservationAlreadyExistsException(result, string.Format(CultureInfo.CurrentUICulture, HttpServerResources.UrlReservationAlreadyExists, url));
                }

                if (result != Win32Error.NO_ERROR)
                {
                    throw new HttpServerException(result);
                }

                return new HttpUrlReservation(url, new[] { rule });
            }
            finally
            {
                Terminate();
            }
        }

        /// <summary>
        /// Gets the URL reservations registered in the Windows HTTP server.
        /// </summary>
        /// <returns>List of URL reservations registered in the Windows HTTP server.</returns>
        public static IReadOnlyList<HttpUrlReservation> GetUrlReservations()
        {
            Initialize();

            try
            {
                var urlReservations = new List<HttpUrlReservation>();
                var record = 0u;

                while (true)
                {
                    // Create the HTTP_SERVICE_CONFIG_URLACL_QUERY structure to query all the URL reservations
                    HttpServerApi.HTTP_SERVICE_CONFIG_URLACL_QUERY inputConfigInfoQuery = new HttpServerApi.HTTP_SERVICE_CONFIG_URLACL_QUERY
                    {
                        QueryDesc = HttpServerApi.HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryNext,
                        Token = record,
                    };

                    // Create an unmanaged memory which will be use to retrieve the output of the HttpQueryServiceConfiguration() call.
                    using (UnmanagedMemoryBlock pOutputConfigInfo = new UnmanagedMemoryBlock())
                    {
                        int returnLength;

                        var result = HttpServerApi.HttpQueryServiceConfiguration(
                            IntPtr.Zero,
                            HttpServerApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                            inputConfigInfoQuery,
                            Marshal.SizeOf(inputConfigInfoQuery),
                            pOutputConfigInfo,
                            pOutputConfigInfo.Length,
                            out returnLength,
                            IntPtr.Zero);

                        // If the API returns ERROR_INSUFFICIENT_BUFFER, increase the pOutputConfigInfo memory block
                        // and call the HttpQueryServiceConfiguration() function again.
                        if (result == Win32Error.ERROR_INSUFFICIENT_BUFFER)
                        {
                            pOutputConfigInfo.Extends(returnLength);

                            result = HttpServerApi.HttpQueryServiceConfiguration(
                                IntPtr.Zero,
                                HttpServerApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                                inputConfigInfoQuery,
                                Marshal.SizeOf(inputConfigInfoQuery),
                                pOutputConfigInfo,
                                pOutputConfigInfo.Length,
                                out returnLength,
                                IntPtr.Zero);
                        }
                        else if (result == Win32Error.ERROR_NO_MORE_ITEMS)
                        {
                            // Exit the loop (there is no more record)
                            break;
                        }

                        // If the HttpQueryServiceConfiguration() function does not return NO_ERROR, throw a Win32Exception.
                        if (result != Win32Error.NO_ERROR)
                        {
                            throw new Win32Exception((int)result);
                        }

                        var urlAcl = pOutputConfigInfo.Unwrap<HttpServerApi.HTTP_SERVICE_CONFIG_URLACL_SET>();

                        urlReservations.Add(ConvertUrlAclToUrlReservation(urlAcl));
                    }

                    record++;
                }

                return urlReservations;
            }
            finally
            {
                Initialize();
            }
        }

        /// <summary>
        /// Calls the HttpInitialize() function of the HTTP Server API.
        /// </summary>
        internal static void Initialize()
        {
            var version = new HttpServerApi.HTTPAPI_VERSION(1, 0);
            var result = HttpServerApi.HttpInitialize(version, HttpServerApi.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);

            if (result != Win32Error.NO_ERROR)
            {
                throw new HttpServerException(result);
            }
        }

        /// <summary>
        /// Calls the HttpTerminate function of the HTTP Server API.
        /// </summary>
        internal static void Terminate()
        {
            HttpServerApi.HttpTerminate(HttpServerApi.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
        }

        /// <summary>
        /// Converts the specified <paramref name="urlAcl"/> structure to the <see cref="HttpUrlReservation"/> instance.
        /// </summary>
        /// <param name="urlAcl"><see cref="HttpServerApi.HTTP_SERVICE_CONFIG_URLACL_SET"/> to convert into <see cref="HttpUrlReservation"/>.</param>
        /// <returns>The <see cref="HttpUrlReservation"/> converted.</returns>
        private static HttpUrlReservation ConvertUrlAclToUrlReservation(HttpServerApi.HTTP_SERVICE_CONFIG_URLACL_SET urlAcl)
        {
            var accessRules = new List<HttpUrlReservationAccessRule>();

            // Parse the security descriptor
            var securityDescriptor = new CommonSecurityDescriptor(false, false, urlAcl.ParamDesc.StringSecurityDescriptor);

            // For each ACE rule create a HttpUrlReservationAccessRule
            foreach (CommonAce rule in securityDescriptor.DiscretionaryAcl)
            {
                // Translate the security identifier to real login name (instead of the SID)
                var id = (IdentityReference)rule.SecurityIdentifier;

                if (id.IsValidTargetType(typeof(NTAccount)))
                {
                    id = id.Translate(typeof(NTAccount));
                }

                accessRules.Add(new HttpUrlReservationAccessRule(id, (HttpUrlReservationAccessRights)rule.AccessMask));
            }

            return new HttpUrlReservation(urlAcl.KeyDesc.UrlPrefix, accessRules);
        }
    }
}
