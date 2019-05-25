//-----------------------------------------------------------------------
// <copyright file="IBaseEntity.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:47</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Abstractions
{
    /// <summary>
    /// Defines the <see cref="IBaseEntity{TIdentifier}" />
    /// </summary>
    /// <typeparam name="TIdentifier"></typeparam>
    public interface IBaseEntity<TIdentifier>
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        TIdentifier Id { get; set; }
    }
}
