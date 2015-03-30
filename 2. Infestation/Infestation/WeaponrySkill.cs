using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class WeaponrySkill : ISupplement
    {
        public void ReactTo(ISupplement otherSupplement)
        {
            throw new NotImplementedException();
        }

        public int PowerEffect
        {
            get { return 0; }
        }

        public int HealthEffect
        {
            get { return 0; }
        }

        public int AggressionEffect
        {
            get { return 0; }
        }
    }
}
