using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class Tank : Unit
    {
        private const int BasePowerValue = 25;
        private const int BaseHealthValue = 20;
        private const int BaseAgressionValue = 25;

        public Tank(string id)
            : base(id, UnitClassification.Mechanical, BaseHealthValue, BasePowerValue, BaseAgressionValue)
        {
        }
    }
}
