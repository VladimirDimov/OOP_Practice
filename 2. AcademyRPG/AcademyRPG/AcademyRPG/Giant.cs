namespace AcademyRPG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class Giant : Character, IFighter, IGatherer
    {
        private bool isEnhanced = false;
        private int attackPoints = 150;

        public Giant(string name, Point position)
            : base(name, position, 0)
        {
            this.HitPoints = 200;
        }

        public int AttackPoints
        {
            get { return this.attackPoints; }
        }

        public int DefensePoints
        {
            get { return 80; }
        }

        public int GetTargetIndex(List<WorldObject> availableTargets)
        {
            WorldObject firstAvailable = availableTargets.FirstOrDefault(x => (x.Owner != 0 && x.Owner != this.Owner));
            return availableTargets.IndexOf(firstAvailable);
        }

        public bool TryGather(IResource resource)
        {
            if (resource.Type == ResourceType.Stone)
            {
                if (!isEnhanced)
                {
                    this.attackPoints += 100;
                }
                return true;
            }

            return false;

        }
    }
}
