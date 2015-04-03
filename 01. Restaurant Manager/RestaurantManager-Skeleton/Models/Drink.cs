using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Interfaces;

namespace RestaurantManager.Models
{
    class Drink : Recipe, IDrink
    {
        private bool isCarbonated;

        public Drink(string name, decimal price, int calories, int quantityPerServing,
            int timeToPrepare, bool isCarbonated)
            : base(name, price, calories, quantityPerServing, timeToPrepare)
        {
            this.IsCarbonated = isCarbonated;

            if (calories > 100)
            {
                throw new ArgumentOutOfRangeException("Drink calories must not be greater than 100");
            }

            if (timeToPrepare > 20)
            {
                throw new ArgumentOutOfRangeException("Drink time to prepare must not be greater than 20");
            }

            this.Unit = MetricUnit.Milliliters;
        }

        public bool IsCarbonated
        {
            get
            {
                return this.isCarbonated;
            }
            private set
            {
                this.isCarbonated = value;
            }
        }

        public override string ToString()
        {
            var result = new List<string>();

            result.Add(string.Format("==  {0} == ${1}", this.Name, this.Price));
            result.Add(string.Format("Per serving: {0} {1}, {2} kcal", this.QuantityPerServing, this.Unit, this.Calories));
            result.Add(string.Format("Ready in {0} minutes", this.TimeToPrepare));
            result.Add(string.Format("Carbonated: {0}", this.IsCarbonated ? "yes" : "no"));

            return string.Join(Environment.NewLine, result);
        }
    }
}
