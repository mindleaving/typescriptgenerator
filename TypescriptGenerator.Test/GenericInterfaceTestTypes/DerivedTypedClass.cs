using TypescriptGenerator.Test.EndToEndTestTypes;

namespace TypescriptGenerator.Test.GenericInterfaceTestTypes
{
    /// <summary>
    /// Class with two generic interfaces,
    /// one that is also used in it's generic form by <see cref="DerivedGenericClass{T}"/>
    /// and one that is only used by this class in its specific type, i.e. IExternalGenericInterface{Person}
    /// </summary>
    public class DerivedTypedClass : IGenericInterface<Address>, IExternalGenericInterface<Person> 
    {
        public Address Item { get; set; }
        public Person EntryReference { get; set; }
    }
}