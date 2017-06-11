//-----------------------------------------------------------------------
// <copyright file="HttpServerApi.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

#pragma warning disable SA1310 // Field names must not contain underscore
    /// <summary>
    /// Contains external methods to use the Windows HTTP Server API.
    /// </summary>
    internal static class HttpServerApi
    {
        /// <summary>
        /// Perform initialization or releases resources for applications that use the HTTP configuration functions
        /// </summary>
        internal const uint HTTP_INITIALIZE_CONFIG = 0x00000002;

        /// <summary>
        /// Defines service configuration options.
        /// </summary>
        internal enum HTTP_SERVICE_CONFIG_ID
        {
            /// <summary>
            /// Specifies the IP Listen List used to register IP addresses on which to listen for SSL connections.
            /// </summary>
            HttpServiceConfigIPListenList,

            /// <summary>
            /// Specifies the SSL certificate store.
            /// </summary>
            HttpServiceConfigSSLCertInfo,

            /// <summary>
            /// Specifies the URL reservation store.
            /// </summary>
            HttpServiceConfigUrlAclInfo
        }

        /// <summary>
        /// The HTTP_SERVICE_CONFIG_QUERY_TYPE enumeration type defines various types of queries to make. It is used in the HTTP_SERVICE_CONFIG_SSL_QUERY and <see cref="HTTP_SERVICE_CONFIG_URLACL_QUERY"/> structures.
        /// </summary>
        internal enum HTTP_SERVICE_CONFIG_QUERY_TYPE
        {
            /// <summary>
            /// The query returns a single record that matches the specified key value.
            /// </summary>
            HttpServiceConfigQueryExact,

            /// <summary>
            /// The query iterates through the store and returns all records in sequence, using an index value that the calling process increments between query calls.
            /// </summary>
            HttpServiceConfigQueryNext
        }

        /// <summary>
        /// Deletes specified data, such as IP addresses or SSL Certificates, from the HTTP Server API configuration store, one record at a time.
        /// </summary>
        /// <param name="serviceHandle">This parameter is reserved and must be zero.</param>
        /// <param name="configId">Type of configuration</param>
        /// <param name="pConfigInformation">Pointer to a buffer that contains data required for the type of configuration specified in the <paramref name="configId"/> parameter.</param>
        /// <param name="configInformationLength">Size, in bytes, of the <paramref name="pConfigInformation"/> buffer.</param>
        /// <param name="pOverlapped">Reserved for future asynchronous operation. This parameter must be set to NULL.</param>
        /// <returns>If the function succeeds, the function returns <see cref="Win32Error.NO_ERROR"/>. If the function fails, it returns a <see cref="Win32Error"/>.</returns>
        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern Win32Error HttpDeleteServiceConfiguration(IntPtr serviceHandle, HTTP_SERVICE_CONFIG_ID configId, HTTP_SERVICE_CONFIG_URLACL_SET pConfigInformation, int configInformationLength, IntPtr pOverlapped);

        /// <summary>
        /// Creates and sets a configuration record for the HTTP Server API configuration store. The call fails if the specified record already exists. To change a given configuration record, delete it and then recreate it with a different value.
        /// </summary>
        /// <param name="serviceHandle">This parameter is reserved and must be zero.</param>
        /// <param name="configId">Type of configuration</param>
        /// <param name="pConfigInformation">Pointer to a buffer that contains data required for the type of configuration specified in the <paramref name="configId"/> parameter.</param>
        /// <param name="configInformationLength">Size, in bytes, of the <paramref name="pConfigInformation"/> buffer.</param>
        /// <param name="pOverlapped">Reserved for future asynchronous operation. This parameter must be set to NULL.</param>
        /// <returns>If the function succeeds, the function returns <see cref="Win32Error.NO_ERROR"/>. If the function fails, it returns a <see cref="Win32Error"/>.</returns>
        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern Win32Error HttpSetServiceConfiguration(IntPtr serviceHandle, HTTP_SERVICE_CONFIG_ID configId, HTTP_SERVICE_CONFIG_URLACL_SET pConfigInformation, int configInformationLength, IntPtr pOverlapped);

        /// <summary>
        /// Initializes the HTTP Server API driver, starts it, if it has not already been started, and allocates data structures for the calling application to support response-queue creation and other operations. Call this function before calling any other functions in the HTTP Server API.
        /// </summary>
        /// <param name="version">HTTP version. This parameter is an <see cref="HTTPAPI_VERSION"/> structure. For the current version, declare an instance of the structure and set it to the pre-defined value HTTPAPI_VERSION_1 before passing it to <see cref="HttpInitialize"/>.</param>
        /// <param name="flags">Initialization options</param>
        /// <param name="pReserved">This parameter is reserved and must be NULL.</param>
        /// <returns>If the function succeeds, the return value is <see cref="Win32Error.NO_ERROR"/>. if the function fails, the return value is a <see cref="Win32Error"/>.</returns>
        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern Win32Error HttpInitialize(HTTPAPI_VERSION version, uint flags, IntPtr pReserved);

        /// <summary>
        /// Cleans up resources used by the HTTP Server API to process calls by an application. An application should call HttpTerminate once for every time it called HttpInitialize, with matching flag settings.
        /// </summary>
        /// <param name="flags">Termination options</param>
        /// <param name="pReserved">This parameter is reserved and must be NULL.</param>
        /// <returns>If the function succeeds, the return value is <see cref="Win32Error.NO_ERROR"/>.</returns>
        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern Win32Error HttpTerminate(uint flags, IntPtr pReserved);

        /// <summary>
        /// Retrieves one or more HTTP Server API configuration records.
        /// </summary>
        /// <param name="serviceHandle">Reserved. Must be zero.</param>
        /// <param name="configId">The configuration record query type. This parameter is one of the following values from the HTTP_SERVICE_CONFIG_ID enumeration.</param>
        /// <param name="pInputConfigInfo">A pointer to a structure whose contents further define the query and of the type that correlates with ConfigId in the following table.</param>
        /// <param name="inputConfigInfoLength">Size, in bytes, of the pInputConfigInfo buffer.</param>
        /// <param name="pOutputConfigInfo">A pointer to a buffer in which the query results are returned. The type of this buffer correlates with ConfigId.</param>
        /// <param name="outputConfigInfoLength">Size, in bytes, of the pOutputConfigInfo buffer.</param>
        /// <param name="pReturnLength">A pointer to a variable that receives the number of bytes to be written in the output buffer. If the output buffer is too small, the call fails with a return value of ERROR_INSUFFICIENT_BUFFER. The value pointed to by pReturnLength can be used to determine the minimum length the buffer requires for the call to succeed.</param>
        /// <param name="pOverlapped">Reserved for asynchronous operation and must be set to NULL.</param>
        /// <returns>If the function succeeds, the return value is NO_ERROR.</returns>
        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern Win32Error HttpQueryServiceConfiguration(IntPtr serviceHandle, HTTP_SERVICE_CONFIG_ID configId, HTTP_SERVICE_CONFIG_URLACL_QUERY pInputConfigInfo, int inputConfigInfoLength, IntPtr pOutputConfigInfo, int outputConfigInfoLength, out int pReturnLength, IntPtr pOverlapped);

        /// <summary>
        /// The HTTP_SERVICE_CONFIG_URLACL_QUERY structure is used to specify a particular reservation record to query in the URL namespace reservation store.
        /// It is passed to the <see cref="HttpQueryServiceConfiguration"/> function using the pInputConfigInfo parameter when the ConfigId parameter is equal to HttpServiceConfigUrlAclInfo.
        /// </summary>
        internal struct HTTP_SERVICE_CONFIG_URLACL_QUERY
        {
            /// <summary>
            /// One of the following values from the <see cref="HTTP_SERVICE_CONFIG_QUERY_TYPE"/> enumeration (<see cref="HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryExact"/> or HttpServiceConfigQueryNext"/>.
            /// </summary>
            public HTTP_SERVICE_CONFIG_QUERY_TYPE QueryDesc;

            /// <summary>
            /// <para>
            /// If the <see cref="QueryDesc"/> parameter is equal to HttpServiceConfigQueryExact, then KeyDesc should contain an <see cref="HTTP_SERVICE_CONFIG_URLACL_KEY"/> structure that identifies the reservation record queried.
            /// </para>
            /// <para>
            /// If the <see cref="QueryDesc"/> parameter is equal to HttpServiceConfigQueryNext, KeyDesc is ignored.
            /// </para>
            /// </summary>
            public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;

            /// <summary>
            /// <para>
            /// If the <see cref="QueryDesc"/> parameter is equal to HttpServiceConfigQueryNext, then <see cref="Token"/> must be equal to zero on the first call to the <see cref="HttpQueryServiceConfiguration"/> function, one on the second call, two on the third call, and so forth until all reservation records are returned, at which point HttpQueryServiceConfiguration returns ERROR_NO_MORE_ITEMS.
            /// </para>
            /// <para>
            /// If the <see cref="QueryDesc"/> parameter is equal to HttpServiceConfigQueryExact, then <see cref="Token"/> is ignored.
            /// </para>
            /// </summary>
            public uint Token;
        }

        /// <summary>
        /// The <see cref="HTTP_SERVICE_CONFIG_URLACL_KEY"/> structure is used to specify a particular reservation record in the URL namespace reservation store. It is a member of the <see cref="HTTP_SERVICE_CONFIG_URLACL_SET"/> and <see cref="HTTP_SERVICE_CONFIG_URLACL_QUERY"/> structures.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct HTTP_SERVICE_CONFIG_URLACL_KEY
        {
            /// <summary>
            /// A pointer to the UrlPrefix string that defines the portion of the URL namespace to which this reservation pertains.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string UrlPrefix;

            /// <summary>
            /// Initializes a new instance of the <see cref="HTTP_SERVICE_CONFIG_URLACL_KEY"/> struct
            /// with the specified <paramref name="urlPrefix"/>.
            /// </summary>
            /// <param name="urlPrefix">A pointer to the UrlPrefix string that defines the portion of the URL namespace to which this reservation pertains.</param>
            public HTTP_SERVICE_CONFIG_URLACL_KEY(string urlPrefix)
            {
                this.UrlPrefix = urlPrefix;
            }
        }

        /// <summary>
        /// The <see cref="HTTP_SERVICE_CONFIG_URLACL_SET"/> structure is used to add a new record to the URL reservation store or retrieve an existing record from it.
        /// An instance of the structure is used to pass data in through the pConfigInformation parameter of the HTTPSetServiceConfiguration function,
        /// or to retrieve data through the pOutputConfigInformation parameter of the <see cref="HttpQueryServiceConfiguration(IntPtr, HTTP_SERVICE_CONFIG_ID, HTTP_SERVICE_CONFIG_URLACL_QUERY, int, IntPtr, int, out int, IntPtr)"/> function when the ConfigId parameter
        /// of either function is equal to HTTPServiceConfigUrlAclInfo.
        /// </summary>
        internal struct HTTP_SERVICE_CONFIG_URLACL_SET
        {
            /// <summary>
            /// An <see cref="HTTP_SERVICE_CONFIG_URLACL_KEY"/> structure that identifies the URL reservation record.
            /// </summary>
            public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;

            /// <summary>
            /// An <see cref="HTTP_SERVICE_CONFIG_URLACL_PARAM"/> structure that holds the contents of the specified URL reservation record.
            /// </summary>
            public HTTP_SERVICE_CONFIG_URLACL_PARAM ParamDesc;

            /// <summary>
            /// Initializes a new instance of the <see cref="HTTP_SERVICE_CONFIG_URLACL_SET"/> struct
            /// with the specified <paramref name="urlPrefix"/> and <paramref name="stringSecurityDescriptor"/>.
            /// </summary>
            /// <param name="urlPrefix">A pointer to the UrlPrefix string that defines the portion of the URL namespace to which this reservation pertains.</param>
            /// <param name="stringSecurityDescriptor">A pointer to a Security Descriptor Definition Language (SDDL) string that contains the permissions associated with this URL namespace reservation record.</param>
            public HTTP_SERVICE_CONFIG_URLACL_SET(string urlPrefix, string stringSecurityDescriptor)
            {
                this.KeyDesc = new HTTP_SERVICE_CONFIG_URLACL_KEY(urlPrefix);
                this.ParamDesc = new HTTP_SERVICE_CONFIG_URLACL_PARAM(stringSecurityDescriptor);
            }
        }

        /// <summary>
        /// The <see cref="HTTP_SERVICE_CONFIG_URLACL_PARAM"/> structure is used to specify the permissions associated with a particular record in the URL namespace reservation store. It is a member of the <see cref="HTTP_SERVICE_CONFIG_URLACL_SET"/> structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct HTTP_SERVICE_CONFIG_URLACL_PARAM
        {
            /// <summary>
            /// A pointer to a Security Descriptor Definition Language (SDDL) string that contains the permissions associated with this URL namespace reservation record.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string StringSecurityDescriptor;

            /// <summary>
            /// Initializes a new instance of the <see cref="HTTP_SERVICE_CONFIG_URLACL_PARAM"/> struct
            /// with the specified <paramref name="stringSecurityDescriptor"/>.
            /// </summary>
            /// <param name="stringSecurityDescriptor">A pointer to a Security Descriptor Definition Language (SDDL) string that contains the permissions associated with this URL namespace reservation record.</param>
            public HTTP_SERVICE_CONFIG_URLACL_PARAM(string stringSecurityDescriptor)
            {
                this.StringSecurityDescriptor = stringSecurityDescriptor;
            }
        }

        /// <summary>
        /// The HTTPAPI_VERSION structure defines the version of the HTTP Server API. This is not to be confused with the version of the HTTP protocol used, which is stored in an HTTP_VERSION structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal struct HTTPAPI_VERSION
        {
            /// <summary>
            /// Major version of the HTTP Server API.
            /// </summary>
            public ushort HttpApiMajorVersion;

            /// <summary>
            /// Minor version of the HTTP Server API.
            /// </summary>
            public ushort HttpApiMinorVersion;

            /// <summary>
            /// Initializes a new instance of the <see cref="HTTPAPI_VERSION"/> struct
            /// with the specified <paramref name="minorVersion"/> and <paramref name="minorVersion"/>.
            /// </summary>
            /// <param name="majorVersion">Major version of the HTTP Server API.</param>
            /// <param name="minorVersion">Minor version of the HTTP Server API.</param>
            public HTTPAPI_VERSION(ushort majorVersion, ushort minorVersion)
            {
                this.HttpApiMajorVersion = majorVersion;
                this.HttpApiMinorVersion = minorVersion;
            }
        }
    }
#pragma warning restore SA1310 // Field names must not contain underscore
}
