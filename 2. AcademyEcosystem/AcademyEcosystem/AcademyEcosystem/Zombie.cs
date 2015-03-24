using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyEcosystem
{   
    class Zombie : Animal
    {
        private const int ZombieSize = -1;
        private const int ZombieMeat = 10;

        public Zombie(string name, Point location)
            : base(name, location, ZombieSize)
        {
        }

        public override int GetMeatFromKillQuantity()
        {
            return ZombieMeat;
        }
    }
}
