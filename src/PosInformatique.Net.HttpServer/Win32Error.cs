//-----------------------------------------------------------------------
// <copyright file="Win32Error.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a Win32 API error code.
    /// </summary>
    internal enum Win32Error : uint
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        NO_ERROR = 0,

        /// <summary>
        /// The system cannot find the file specified.
        /// </summary>
        ERROR_FILE_NOT_FOUND = 2,

        /// <summary>
        /// The data area passed to a system call is too small.
        /// </summary>
        ERROR_INSUFFICIENT_BUFFER = 122,

        /// <summary>
        /// Cannot create a file when that file already exists.
        /// </summary>
        ERROR_ALREADY_EXISTS = 183,

        /// <summary>
        /// No more data is available.
        /// </summary>
        ERROR_NO_MORE_ITEMS = 259
    }
}
