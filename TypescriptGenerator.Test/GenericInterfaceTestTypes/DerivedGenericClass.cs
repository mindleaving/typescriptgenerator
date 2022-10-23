using TypescriptGenerator.Test.EndToEndTestTypes;

namespace TypescriptGenerator.Test.GenericInterfaceTestTypes
{
    /// <summary>
    /// Generic class that uses one interface with a generic parameter
    /// </summary>
    public class DerivedGenericClass<T> : IGenericInterface<T>
    {
        public string Id { get; set; }
        public T Item { get; set; }
    }
}