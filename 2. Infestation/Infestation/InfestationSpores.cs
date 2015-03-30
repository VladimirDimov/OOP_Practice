using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class InfestationSpores : ISupplement
    {
        private const int PowerEffectValue = -1;
        private const int HealthEffectValue = 0;
        private const int AggressionEffectValue = 20;

        private int powerEffect = -1;
        private int healthEffect = 0;
        private int aggressionEffecte = 20;

        public InfestationSpores()
        {
            this.PowerEffect = PowerEffectValue;
            this.HealthEffect = HealthEffectValue;
            this.AggressionEffect = AggressionEffectValue;
        }

        public void ReactTo(ISupplement otherSupplement)
        {
            if (otherSupplement.GetType().Name == "InfestationSpores")
            {
                this.PowerEffect = 0;
                this.HealthEffect = 0;
                this.AggressionEffect = 0;
            }
        }

        public int PowerEffect
        {
            get { return PowerEffectValue; }
            private set { this.powerEffect = value; }
        }

        public int HealthEffect
        {
            get { return HealthEffectValue; }
            private set { this.healthEffect = value; }
        }

        public int AggressionEffect
        {
            get { return AggressionEffectValue; }
            private set { this.aggressionEffecte = value; }
        }
    }
}
