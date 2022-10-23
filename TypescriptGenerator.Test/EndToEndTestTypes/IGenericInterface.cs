namespace TypescriptGenerator.Test.EndToEndTestTypes
{
    public interface IExternalGenericInterface<S>
    {
        S EntryReference { get; }
    }
    public interface IGenericInterface<T>
    {
        T Item { get; }
    }
}