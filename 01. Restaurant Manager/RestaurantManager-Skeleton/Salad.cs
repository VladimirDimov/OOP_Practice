using RestaurantManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantManager.Models
{
    public class Salad : Meal , ISalad
    {
        private const bool IsSaladVegan = true;

        private bool containsPasta;

        public Salad(string name, decimal price, int calories, int quantityPerServing, 
            int timeToPrepare, bool containsPasta)
            : base(name, price, calories, quantityPerServing, timeToPrepare, IsSaladVegan)
        {
            this.ContainsPasta = containsPasta;
        }

        public bool ContainsPasta
        {
            get
            {
                return this.containsPasta;
            }
            private set
            {
                this.containsPasta = value;
            }
        }
    }
}
