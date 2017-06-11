//-----------------------------------------------------------------------
// <copyright file="Contracts.cs" company="P.O.S Informatique">
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
    /// Contains contracts method to test arguments of methods.
    /// </summary>
    internal static class Contracts
    {
        /// <summary>
        /// Checks if the specified <paramref name="value"/> is not null.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="value"/> to test.</typeparam>
        /// <param name="value">Value to check.</param>
        /// <param name="argumentName">Argument name to check (used for <see cref="ArgumentException.ParamName"/>).</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="value"/> is null.</exception>
        public static void IsNotNull<T>(T value, string argumentName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
