//-----------------------------------------------------------------------
// <copyright file="HttpUrlReservationAccessRule.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an access rule for a <see cref="HttpUrlReservation"/>.
    /// </summary>
    public sealed class HttpUrlReservationAccessRule : AccessRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUrlReservationAccessRule"/> class.
        /// </summary>
        /// <param name="identity">The identity to which the access rule applies. This parameter must be an object that can be cast as a <see cref="SecurityIdentifier"/>.</param>
        /// <param name="rights">The <see cref="HttpUrlReservation"/> operation to which this access rule controls access.</param>
        public HttpUrlReservationAccessRule(IdentityReference identity, HttpUrlReservationAccessRights rights)
            : base(identity, (int)rights, false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
        {
        }

        /// <summary>
        /// Gets the <see cref="HttpUrlReservation"/> operation to which this access rule controls access.
        /// </summary>
        public HttpUrlReservationAccessRights HttpUrlReservationAccessRights
        {
            get
            {
                return (HttpUrlReservationAccessRights)this.AccessMask;
            }
        }
    }
}
