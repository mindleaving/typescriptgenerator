using System;

namespace TypescriptGenerator.Test.EndToEndTestTypes.Health
{
    public class Patient : Person, IAdmittable
    {
        public string Name { get; set; }
        public Person ContactPerson { get; set; }
        public DateTime AdmissionStart { get; set; }
        public DateTime AdmissionEnd { get; set; }
    }
}