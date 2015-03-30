using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class Marine : Human
    {
        public Marine(string id)
            : base(id)
        {
            this.AddSupplement(new WeaponrySkill());
        }
        
        protected override UnitInfo GetOptimalAttackableUnit(IEnumerable<UnitInfo> attackableUnits)
        {
            //This method finds the unit with the highest Health and attacks it
            var optimalAttackableUnit = new UnitInfo();

            foreach (var unit in attackableUnits)
            {
                if (unit.Health > int.MinValue)
                {
                    optimalAttackableUnit = unit;
                }
            }

            return optimalAttackableUnit;
        }
    }
}
