using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarMachines.Interfaces;
using WarMachines.Machines;

namespace WarMachines
{
    public class Tank : Machine, ITank
    {
        private const int DefensePointsIncrement = 30;
        private const int AttackPointsIncrement = 40;

        private bool isDefenseMode;
        private double initialDefensePoints;
        private double initialAttackPoints;

        public Tank(string name, double attackPoints, double defensePoints) 
            : base(name, attackPoints, defensePoints)
        {
            this.HealthPoints = 100;
            this.initialAttackPoints = attackPoints;
            this.initialDefensePoints = defensePoints;
            this.isDefenseMode = true;
            UpadteAttackDeffencePoints();
        }

        public bool DefenseMode
        {
            get 
            { 
                return isDefenseMode; 
            }
            private set 
            { 
                isDefenseMode = value; 
            }
        }

        public void ToggleDefenseMode()
        {
            this.DefenseMode = !this.DefenseMode;
            UpadteAttackDeffencePoints();
        }

        private void UpadteAttackDeffencePoints()
        {
            if (this.isDefenseMode)
            {
                this.DefensePoints = this.initialDefensePoints + DefensePointsIncrement;
                this.AttackPoints = this.initialAttackPoints - AttackPointsIncrement;
            }
            else
            {
                this.DefensePoints = this.initialDefensePoints;
                this.AttackPoints = this.initialAttackPoints;
            }
        }
    }
}
