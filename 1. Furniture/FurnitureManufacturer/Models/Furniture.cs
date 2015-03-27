using System;
using System.Collections.Generic;
using System.Linq;
namespace FurnitureManufacturer.Models
{
using System.Text;
using FurnitureManufacturer.Interfaces;
using System.Threading.Tasks;

    public class Furniture : IFurniture
    {
        #region Fields
        private string model;
        private string material;
        private decimal price;
        private decimal height;
        #endregion

        #region Constructors
        public Furniture(string model, string materialType, decimal price, decimal height)
        {
            this.Model = model;
            this.Material = materialType;
            this.Price = price;
            this.Height = height;
        }
        #endregion

        #region Properties
        public string Model
        {
            get
            {
                return this.model;
            }
            private set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                {
                    throw new ArgumentException("Model name must be at least 3 symbols");
                }

                this.model = value;
            }
        }

        public string Material
        {
            get
            {
                return this.material;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Material type cannot be null or empty");
                }

                this.material = value;
            }
        }

        public decimal Price
        {
            get
            {
                return this.price;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price cannot be negative");
                }

                this.price = value;
            }
        }

        public decimal Height
        {
            get
            {
                return this.height;
            }
            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Height must be greater than zero");
                }

                this.height = value;
            }
        }
        #endregion
        //public virtual string ToString()
        //{
        //    return null;
        //}
    }
}
