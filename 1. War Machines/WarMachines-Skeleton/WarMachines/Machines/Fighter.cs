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
        private bool isStealthMode;

        public bool StealthMode
        {
            get;
        }

        public void ToggleStealthMode()
        {
            this.isStealthMode = !this.isStealthMode;
        }
    }
}
