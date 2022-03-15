using System;

namespace TypescriptGenerator.Test.EndToEndTestTypes.Health
{
    public interface IAdmittable : INamedEntity, IHasContact
    {
        DateTime AdmissionStart { get; set; }
        DateTime AdmissionEnd { get; set; }
    }
}