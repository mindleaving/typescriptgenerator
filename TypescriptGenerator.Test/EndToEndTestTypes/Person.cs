using System;
using System.Collections.Generic;

namespace TypescriptGenerator.Test.EndToEndTestTypes
{
    public class Person : IId
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
