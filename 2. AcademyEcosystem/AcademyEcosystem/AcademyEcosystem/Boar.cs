using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyEcosystem
{
    class Boar : Animal, ICarnivore, IHerbivore
    {
        private const int BoarSize = 4;
        private const int BoarBiteSize = 2;

        public Boar(string name, Point location)
            : base(name, location, BoarSize)
        {
        }

        public int TryEatAnimal(Animal animal)
        {
            if (animal != null)
            {
                if (this.Size >= animal.Size)
                {
                   return animal.GetMeatFromKillQuantity();
                }
            }
            
            return 0;
        }

        public int EatPlant(Plant plant)
        {
            if (plant != null)
            {
                this.Size++;
                return plant.GetEatenQuantity(BoarBiteSize);
            }

            return 0;
        }
    }
}
