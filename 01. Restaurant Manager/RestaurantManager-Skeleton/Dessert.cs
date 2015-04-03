using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestaurantManager.Interfaces;

namespace RestaurantManager.Models
{
    public class Dessert : Meal, IDessert
    {
        private bool withSugar;
        
        public Dessert(string name, decimal price, int calories, 
            int quantityPerServing, int timeToPrepare, bool isVegan)
            : base(name, price, calories, quantityPerServing, timeToPrepare, isVegan)
        {
            this.WithSugar = true;
        }

        public bool WithSugar
        {
            get
            {
                return this.withSugar;
            }
            private set
            {
                this.withSugar = value;
            }
        }

        public void ToggleSugar()
        {
            this.WithSugar = !this.WithSugar;
        }
    }
}
