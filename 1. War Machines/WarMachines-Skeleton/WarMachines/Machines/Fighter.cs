using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarMachines.Interfaces;
using WarMachines.Machines;

namespace WarMachines
{
    public class Fighter : Machine, IFighter
    {
        private const double FighterInitialHealth = 200;

        public Fighter(string name, double attackPoints, double defensePoints, bool stealthMode)
            :base(name, attackPoints, defensePoints)
        {
            this.isStealthMode = stealthMode;
        }

        private bool isStealthMode;

        public bool StealthMode
        {
            get { return this.isStealthMode; }
        }

        public void ToggleStealthMode()
        {
            this.isStealthMode = !this.isStealthMode;
        }
    }
}
