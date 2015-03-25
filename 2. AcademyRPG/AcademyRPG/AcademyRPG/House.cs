namespace AcademyRPG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class House : StaticObject
    {
        public House(Point position)
            :base(position)
        {
            this.HitPoints = 500;
        }

        public House(Point position, int owner)
            : base(position, owner)
        {
            this.HitPoints = 500;
        }
    }
}
