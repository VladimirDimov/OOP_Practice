using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Interfaces;

namespace RestaurantManager.Models
{
    public class Meal : Recipe , IMeal
    {
        private bool isVegan;

        public Meal(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, 
            bool isVegan)
            : base(name, price, calories, quantityPerServing, timeToPrepare)
        {
            this.IsVegan = isVegan;
            this.Unit = MetricUnit.Grams;
        }

        public bool IsVegan
        {
            get
            {
                return this.isVegan;
            }
            private set
            {
                this.isVegan = value;
            }
        }

        public void ToggleVegan()
        {
            this.IsVegan = !this.IsVegan;
        }
    }
}
