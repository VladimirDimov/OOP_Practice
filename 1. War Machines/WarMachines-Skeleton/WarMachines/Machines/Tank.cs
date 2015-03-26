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
        private bool isDefenseMode;

        public bool DefenseMode
        {
            get;
        }

        public void ToggleDefenseMode()
        {
            this.isDefenseMode = !this.isDefenseMode;
        }
    }
}
