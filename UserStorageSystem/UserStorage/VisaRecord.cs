using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public struct VisaRecord
    {
        public string Country { get; }
        public DateTime DateOfStarting { get; }
        public DateTime DateOfEnding { get; }

        public VisaRecord(string country, DateTime dateOfStarting, DateTime dateOfEnding)
        {
            Country = country;
            DateOfStarting = dateOfStarting;
            DateOfEnding = dateOfEnding;
        }        
    }
}
