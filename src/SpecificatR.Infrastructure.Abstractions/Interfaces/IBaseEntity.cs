//-----------------------------------------------------------------------
// <copyright file="IBaseEntity.cs">
//     Copyright (c) 2019-2020 David Vanderheyden All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
//-----------------------------------------------------------------------

namespace SpecificatR.Abstractions
{
    /// <summary>
    /// Defines the <see cref="IBaseEntity{TIdentifier}"/>.
    /// </summary>
    /// <typeparam name="TIdentifier">The Type identifier <see cref="TIdentifier"/>.</typeparam>
    public interface IBaseEntity<TIdentifier>
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        TIdentifier Id { get; set; }
    }
}
