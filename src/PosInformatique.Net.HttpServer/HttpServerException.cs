//-----------------------------------------------------------------------
// <copyright file="HttpServerException.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Occurs when an error is occured with the <see cref="HttpServer"/>.
    /// </summary>
    [Serializable]
    public class HttpServerException : Win32Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerException"/> class
        ///  with the last Win32 error that occurred.
        /// </summary>
        public HttpServerException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerException"/> class
        /// with the specified <paramref name="error"/>.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        public HttpServerException(int error)
            : base(error)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerException"/> class
        ///     with the specified detailed description.
        /// </summary>
        /// <param name="message">A detailed description of the error.</param>
        public HttpServerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerException"/> class
        /// with the specified <paramref name="error"/> and the specified detailed description.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        /// <param name="message">A detailed description of the error.</param>
        public HttpServerException(int error, string message)
            : base(error, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerException"/> class
        /// with the specified <paramref name="error"/>.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        internal HttpServerException(Win32Error error)
            : this((int)error)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerException"/> class
        /// with the specified <paramref name="error"/> and the specified detailed description.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        /// <param name="message">A detailed description of the error.</param>
        internal HttpServerException(Win32Error error, string message)
            : this((int)error, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerException"/> class
        /// with the specified context and the serialization information.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> associated with this exception.</param>
        /// <param name="context">A <see cref="StreamingContext"/> that represents the context of
        /// this exception.</param>
        protected HttpServerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
