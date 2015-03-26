using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarMachines
{
    public class Pilot : WarMachines.Interfaces.IPilot
    {
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public void AddMachine(Interfaces.IMachine machine)
        {
            throw new NotImplementedException();
        }

        public string Report()
        {
            throw new NotImplementedException();
        }
    }
}
