using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class Queen : Infest
    {
        private const int BaseHealth = 30;
        private const int BasePower = 1;
        private const int BaseAgression = 1;

        public Queen(string id)
            : base(id, UnitClassification.Psionic, BaseHealth, BasePower, BaseAgression)
        {
        }
    }
}
