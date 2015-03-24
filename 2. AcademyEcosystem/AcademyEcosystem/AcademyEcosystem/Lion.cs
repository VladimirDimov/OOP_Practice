using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyEcosystem
{
    class Lion : Animal, ICarnivore
    {
        private const int LionSize = 6;

        public Lion(string name, Point location)
            : base(name, location, LionSize)
        {
        }

        public int TryEatAnimal(Animal animal)
        {
            if (animal != null)
            {
                if (2 * this.Size >= animal.Size)
                {
                    this.Size++;
                    return animal.GetMeatFromKillQuantity();
                }
            }

            return 0;
        }
    }
}
