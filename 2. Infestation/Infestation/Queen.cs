using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class Queen : Unit
    {
        public Queen(string id)
            : base(id, UnitClassification.Psionic, 30, 1 , 1)
        {

        }
    }
}
