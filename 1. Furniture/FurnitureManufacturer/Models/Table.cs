namespace FurnitureManufacturer.Models
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FurnitureManufacturer.Interfaces;
using System.Threading.Tasks;

    public class Table : Furniture, ITable
    {
        private decimal length;
        private decimal width;

        public Table(string model, string materialType, decimal price, decimal heigth, decimal length, decimal width)
            : base(model, materialType, price, heigth)
        {
            this.Length = length;
            this.Width = width;
        }

        public decimal Length
        {
            get
            {
                return this.length;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Length must be greater than zero");
                }

                this.length = value;
            }
        }

        public decimal Width
        {
            get
            {
                return this.width;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Width must be greater than zero");
                }

                this.width = value;
            }
        }

        public decimal Area
        {
            get { return this.Length * this.Width; }
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Model: {1}, Material: {2}, Price: {3}, Height: {4}, Length: {5}, Width: {6}, Area: {7}", 
                this.GetType().Name, this.Model, this.Material, this.Price, this.Height, this.Length, this.Width, this.Area);
        }
    }
}
