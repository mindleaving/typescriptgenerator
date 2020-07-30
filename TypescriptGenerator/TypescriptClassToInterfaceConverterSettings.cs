namespace TypescriptGenerator
{
    public class TypescriptClassToInterfaceConverterSettings
    {
        public bool AddExportModifier { get; set; } = false;
        public TypescriptPropertyConverterSettings PropertySettings { get; } = new TypescriptPropertyConverterSettings();
    }
}