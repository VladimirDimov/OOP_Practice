using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestaurantManager.Interfaces;

namespace RestaurantManager.Models
{
    public class Restaurant : IRestaurant
    {
        private string name;
        private string location;
        private IList<IRecipe> recipes;

        public Restaurant(string name, string location)
        {
            this.Name = name;
            this.Location = location;
            this.recipes = new List<IRecipe>();
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

        public string Location
        {
            get
            {
                return this.location;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Restaurant location cannot be null or empty");
                }

                this.location = value;
            }
        }

        public IList<IRecipe> Recipes
        {
            get
            {
                return this.recipes;
            }
        }

        public void AddRecipe(IRecipe recipe)
        {
            this.Recipes.Add(recipe);
        }

        public void RemoveRecipe(IRecipe recipe)
        {
            if (this.Recipes.Contains(recipe))
            {
                this.Recipes.Remove(recipe);
            }
        }

        public string PrintMenu()
        {
            // ***** <name> - <location> *****
            // <recipes>

            // ~~~~~ DRINKS ~~~~~
            // ~~~~~ SALADS ~~~~~
            // ~~~~~ MAIN COURSES ~~~~~
            // ~~~~~ DESSERTS ~~~~~
            var menu = new List<string>();

            menu.Add(string.Format("***** {0} - {1} *****", this.Name, this.Location));

            if (this.recipes.Count == 0)
            {
                menu.Add("No recipes... yet");
            }
            else
            {
                var drinks = this.Recipes
                    .Where(x => x is IDrink)
                    .OrderBy(x => x.Name).ToList();

                if (drinks.Count > 0)
                {
                    menu.Add("~~~~~ DRINKS ~~~~~");
                    foreach (var item in drinks)
                    {
                        menu.Add(item.ToString());
                    }
                }

                var salads = this.recipes
                    .Where(x => x is ISalad)
                    .OrderBy(x => x.Name).ToList();

                if (salads.Count > 0)
                {
                    menu.Add("~~~~~~~ SALADS ~~~~~");
                    foreach (var item in salads)
                    {
                        menu.Add(item.ToString());
                    }
                }

                var mainCourses = this.recipes
                    .Where(x => x is MainCourse)
                    .OrderBy(x => x.Name).ToList();

                if (mainCourses.Count > 0)
                {
                    menu.Add("~~~~~ MAIN COURSES ~~~~~");
                    foreach (var item in mainCourses)
                    {
                        menu.Add(item.ToString());
                    }
                }

                var dessesrts = this.recipes
                    .Where(x => x is IDessert)
                    .OrderBy(x => x.Name).ToList();

                if (mainCourses.Count > 0)
                {
                    menu.Add("~~~~~ DESSERTS ~~~~~");
                    foreach (var item in dessesrts)
                    {
                        menu.Add(item.ToString());
                    }
                }
            }
            
            return string.Join(Environment.NewLine, menu);
        }
    }
}
