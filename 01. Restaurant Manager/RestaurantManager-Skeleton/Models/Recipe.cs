using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Interfaces;

namespace RestaurantManager.Models
{
    public class Recipe : IRecipe
    {
        private string name;
        private decimal price;
        private int calories;
        private int quantityPerServing;
        private int timeToPrepare;
        private MetricUnit unit;

        public Recipe(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare)
        {
            this.Name = name;
            this.Price = price;
            this.Calories = calories;
            this.QuantityPerServing = quantityPerServing;
            this.TimeToPrepare = timeToPrepare;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Restaurant name cannot be null or empty");
                }

                this.name = value;
            }
        }

        public decimal Price
        {
            get
            {
                return this.price;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Receipe price must be positive");
                }

                this.price = value;
            }
        }

        public int Calories
        {
            get
            {
                return this.calories;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Receipe calories must be positive");
                }

                this.calories = value;
            }
        }

        public int QuantityPerServing
        {
            get
            {
                return this.quantityPerServing;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Receipe quantity per serving must be positive");
                }

                this.quantityPerServing = value;
            }
        }

        public MetricUnit Unit
        {
            get
            {
                return this.unit;
            }
            protected set
            {
                if (!Enum.IsDefined(typeof(MetricUnit), value))
                {
                    throw new ArgumentException("Invalid receipe");
                }

                this.unit = value;
            }
        }

        public int TimeToPrepare
        {
            get
            {
                return this.timeToPrepare;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Receipe time to prepare must be positive");
                }

                this.timeToPrepare = value;
            }
        }
    }
}
