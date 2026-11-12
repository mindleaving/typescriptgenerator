using TypescriptGenerator.Formatter;

namespace TypescriptGenerator.Settings
{
    public class GeneralFormatterSettings
    {
        public string NewLine { get; set; } = "\n";
        public string IndentString { get; set; } = "    ";
        public EnumNameSource EnumNameSource { get; set; } = EnumNameSource.CSharpName;
        public EnumNameSource StringValueSource { get; set; } = EnumNameSource.EnumMemberAttribute;
    }
}
