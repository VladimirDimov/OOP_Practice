namespace AcademyEcosystem
{
    using System;

    class Wolf : Animal, ICarnivore
    {
        public Wolf(string name, Point location)
            : base(name, location, 4)
        {
        }

        public int TryEatAnimal(Animal animal)
        {
            if (animal == null)
            {
                return 0;
            }
            if (animal.State == AnimalState.Sleeping || animal.Size <= this.Size)
            {
                return animal.GetMeatFromKillQuantity();
            }
            else return 0;
        }
    }
}
