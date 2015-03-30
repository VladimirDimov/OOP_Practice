using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class Weapon : ISupplement
    {
        private const int PowerEffectValue = 0;
        private const int HealthEffectValue = 0;
        private const int AggressionEffectValue = 0;

        private int powerEffect;
        private int healthEffect;
        private int agressionEffect;

        public Weapon()
        {

        }

        public void ReactTo(ISupplement otherSupplement)
        {
            if (otherSupplement.GetType().Name == "WeaponrySkill")
            {
                this.PowerEffect = 10;
                this.AggressionEffect = 3;
            }
        }

        public int PowerEffect
        {
            get 
            {
                return PowerEffectValue; 
            }
            private set
            {
                this.powerEffect = value;
            }
        }

        public int HealthEffect
        {
            get 
            { 
                return HealthEffectValue; 
            }
            private set
            {
                this.healthEffect = value;
            }
        }

        public int AggressionEffect
        {
            get 
            { 
                return AggressionEffectValue; 
            }
            private set
            {
                this.agressionEffect = value;
            }
        }
    }
}
