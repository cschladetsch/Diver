namespace Pyro
{
    public interface IRef<T>
        : IConstRef<T>
        , IRefBase
    {
        new T Value { get; set; }
    }
}
