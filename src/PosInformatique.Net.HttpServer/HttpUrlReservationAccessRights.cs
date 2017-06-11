//-----------------------------------------------------------------------
// <copyright file="HttpUrlReservationAccessRights.cs" company="P.O.S Informatique">
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
    /// Defines the access rights for the <see cref="HttpUrlReservation"/>.
    /// </summary>
    public enum HttpUrlReservationAccessRights
    {
        /// <summary>
        /// Allows the designated user to register to receive requests from the <see cref="HttpUrlReservation.Url"/>, but does not allow the user to delegate sub-tree reservations to others.
        /// </summary>
        Listen = 0x20000000,

        /// <summary>
        /// Allows the designated user to reserve (delegate) a subtree of the <see cref="HttpUrlReservation.Url"/> for another user, but does not allow the user to register to receive requests from the URL.
        /// </summary>
        Delegate = 0x40000000,

        /// <summary>
        /// Allows the designated user to have the <see cref="Listen"/> and <see cref="Delegate"/> rights.
        /// </summary>
        ListenAndDelegate = Listen | Delegate
    }
}
