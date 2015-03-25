
namespace AcademyRPG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Knight : Character, IFighter
    {
        public Knight(string name, Point coordinates, int owner)
            : base(name, coordinates, owner)
        {
            this.HitPoints = 100;
        }

        public int AttackPoints
        {
            get { return 100; }
        }

        public int DefensePoints
        {
            get { return 100; }
        }

        public int GetTargetIndex(List<WorldObject> availableTargets)
        {
            WorldObject firstAvailable = availableTargets.FirstOrDefault(x => (x.Owner != 0 && x.Owner != this.Owner));
            return availableTargets.IndexOf(firstAvailable);
        }
    }
}
