using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestaurantManager.Interfaces;

namespace RestaurantManager.Models
{
    public class MainCourse : Meal, IMainCourse
    {
        private MainCourseType type;

        public MainCourse(string name, decimal price, int calories, int quantityPerServing,
            int timeToPrepare, bool isVegan, string courseType)
            : base(name, price, calories, quantityPerServing, timeToPrepare, isVegan)
        {
            if (Enum.IsDefined(typeof(MainCourseType), courseType))
            {
                this.Type = (MainCourseType)Enum.Parse(typeof(MainCourseType), courseType);
            }
            else
            {
                throw new ArgumentException("Invalid main course type");
            }
        }

        public MainCourseType Type
        {
            get
            {
                return this.type;
            }
            private set
            {
                this.type = value;
            }
        }

        public override string ToString()
        {
            // <[VEGAN] >==  <name> == $<price>
            // Per serving: <quantity> <unit>, <calories> kcal
            // Ready in <time_to_prepare> minutes
            // Type: <type>

            var result = new List<string>();

            result.Add(string.Format("<[{0}] >==  {1} == ${2}", 
                this.IsVegan ? "VEGAN" : "NO VEGAN" , this.Name, this.Price));
            result.Add(string.Format("Per serving: {0} {1}, {2} kcal", 
                this.QuantityPerServing, this.Unit, this.Calories));
            result.Add(string.Format("Ready in {0} minutes", this.TimeToPrepare));
            result.Add(string.Format("Type: {0}", this.Type));

            return string.Join(Environment.NewLine, result);

        }
    }
}
