namespace TypescriptGenerator.Objects
{
    public interface ITypescriptObject
    {
        string OriginalNamespace { get; }
        string TranslatedNamespace { get; }
    }
}