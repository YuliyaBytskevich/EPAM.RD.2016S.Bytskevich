using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public interface IIdentifiersGenerator
    {
        int GenerateNewNumber();

        void ResetGenerator();
    }
}
