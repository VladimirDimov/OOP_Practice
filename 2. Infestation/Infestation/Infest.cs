using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    public class Infest : Unit
    {
        public Infest(string id, UnitClassification unitType, int health, int power, int agression)
            : base(id, unitType, health, power, agression)
        {
        }

        public override Interaction DecideInteraction(IEnumerable<UnitInfo> units)
        {
            IEnumerable<UnitInfo> attackableUnits = units.Where((unit) => this.CanAttackUnit(unit));

            UnitInfo optimalAttackableUnit = GetOptimalAttackableUnit(attackableUnits);

            if (optimalAttackableUnit.Id != null)
            {
                return new Interaction(new UnitInfo(this), optimalAttackableUnit, InteractionType.Infest);
            }

            return Interaction.PassiveInteraction;
        }

        protected override bool CanAttackUnit(UnitInfo unit)
        {
            bool attackUnit = false;
            if (this.Id != unit.Id && 
                this.UnitClassification == InfestationRequirements.RequiredClassificationToInfest(unit.UnitClassification))
            {
                attackUnit = true;
            }

            return attackUnit;
        }

        protected override UnitInfo GetOptimalAttackableUnit(IEnumerable<UnitInfo> attackableUnits)
        {
            //This method finds the unit with the least health and attacks it
            var optimalAttackableUnit = new UnitInfo();

            foreach (var unit in attackableUnits)
            {
                if (unit.Health < int.MaxValue)
                {
                    optimalAttackableUnit = unit;
                }
            }

            return optimalAttackableUnit;
        }
    }
}
