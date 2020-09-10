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
