using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class Weapon : ISupplement
    {
        private const int PowerEffectValue = 10;
        private const int HealthEffectValue = 0;
        private const int AggressionEffectValue = 3;

        public Weapon()
        {

        }

        public void ReactTo(ISupplement otherSupplement)
        {
            throw new NotImplementedException();
        }

        public int PowerEffect
        {
            get 
            {
                return PowerEffectValue; 
            }
        }

        public int HealthEffect
        {
            get 
            { 
                return HealthEffectValue; 
            }
        }

        public int AggressionEffect
        {
            get 
            { 
                return AggressionEffectValue; 
            }
        }
    }
}
