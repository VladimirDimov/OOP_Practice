using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarMachines.Interfaces;

namespace WarMachines
{
    public class Pilot : WarMachines.Interfaces.IPilot
    {
        private string name;
        private List<IMachine> machines;

        public Pilot(string name)           
        {
            this.name = name;
            machines = new List<IMachine>();
        }

        public string Name
        {
            get { return this.name; }
        }

        public void AddMachine(IMachine machine)
        {
            machines.Add(machine);
        }

        public string Report()
        {
            var builder = new StringBuilder();            
            string countMachinesAsString = null;

            if (this.machines.Count == 0)
            {
                countMachinesAsString = "no machines";
            }
            else if (this.machines.Count == 1)
            {
                countMachinesAsString = "1 machine";                
            }
            else if (this.machines.Count > 1)
            {
                countMachinesAsString = machines.Count + " machines";
            }
            builder.AppendFormat("{0} - {1}", this.Name, countMachinesAsString);
            foreach (var machine  in this.machines)
            {
                builder.Append("\n" + machine.ToString());
            }
            return builder.ToString();
        }
    }
}
