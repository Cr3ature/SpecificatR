namespace SpecificatR.Infrastructure.Abstractions
{
    public interface IBaseEntity<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}
