using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarMachines.Interfaces;

namespace WarMachines.Machines
{
    public class Machine : IMachine
    {
        private double attackPoints;
        private double defensePoints;
        private double healthPoints;
        private string name;
        private IPilot tankPilot;
        private IList<string> targets;
        
        public Machine(string name, double attackPoints, double defensePoints)
        {
            this.Name = name;
            this.attackPoints = attackPoints;
            this.defensePoints = defensePoints;
            targets = new List<string>();
        }
        
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public IPilot Pilot
        {
            get
            {
                return this.tankPilot;
            }
            set
            {
                this.tankPilot = value;
            }
        }

        public double HealthPoints
        {
            get
            {
                return this.healthPoints;
            }
            set
            {
                this.healthPoints = value;
            }
        }

        public double AttackPoints
        {
            get { return this.attackPoints; }
            protected set { this.attackPoints = value; }
        }

        public double DefensePoints
        {
            get 
            { 
                return this.defensePoints; 
            }
            protected set
            {
                this.defensePoints = value;
            }
        }

        public IList<string> Targets
        {
            get { return this.targets; }
        }

        public void Attack(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                throw new ArgumentException("Target cannot be null or empty");
            }

            this.targets.Add(target);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("- {0}\n", this.name);
            builder.AppendFormat("*Type: {0}\n", this.GetType().Name);
            builder.AppendFormat("*Health: {0}\n", this.HealthPoints);
            builder.AppendFormat("*Attack: {0}\n", this.AttackPoints);
            builder.AppendFormat("*Defense: {0}\n", this.DefensePoints);
            builder.AppendFormat("*Targets: {0}\n", this.Targets.Count == 0 ? "None" : string.Join(", ", this.Targets));

            if (this is Tank)
            {
                var asTank = this as Tank;
                builder.AppendFormat("*Defense: {0}", asTank.DefenseMode ? "On" : "OFF");
            }
            else if (this is Fighter)
            {
                var asTank = this as Fighter;
                builder.AppendFormat("*Stealth: {0}", asTank.StealthMode ? "On" : "OFF");
            }
            return builder.ToString();
        }
    }
}
