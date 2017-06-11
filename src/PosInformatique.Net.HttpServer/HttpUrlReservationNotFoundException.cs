﻿//-----------------------------------------------------------------------
// <copyright file="HttpUrlReservationNotFoundException.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Occurs when an <see cref="HttpUrlReservation"/> deleted or updated has not been found.
    /// </summary>
    [Serializable]
    public class HttpUrlReservationNotFoundException : HttpServerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUrlReservationNotFoundException"/> class
        /// with the last Win32 error that occurred.
        /// </summary>
        public HttpUrlReservationNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUrlReservationNotFoundException"/> class
        /// with the specified <paramref name="error"/>.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        public HttpUrlReservationNotFoundException(int error)
            : base(error)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUrlReservationNotFoundException"/> class
        ///     with the specified detailed description.
        /// </summary>
        /// <param name="message">A detailed description of the error.</param>
        public HttpUrlReservationNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUrlReservationNotFoundException"/> class
        /// with the specified error and the specified detailed description.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        /// <param name="message">A detailed description of the error.</param>
        public HttpUrlReservationNotFoundException(int error, string message)
            : base(error, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUrlReservationNotFoundException"/> class
        /// with the specified <paramref name="error"/>.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        /// <param name="message">A detailed description of the error.</param>
        internal HttpUrlReservationNotFoundException(Win32Error error, string message)
            : this((int)error, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUrlReservationNotFoundException"/> class
        /// with the specified context and the serialization information.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> associated with this exception.</param>
        /// <param name="context">A <see cref="StreamingContext"/> that represents the context of
        /// this exception.</param>
        protected HttpUrlReservationNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
