namespace FurnitureManufacturer.Models
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FurnitureManufacturer.Interfaces;
using System.Threading.Tasks;

    public class ConvertibleChair : Chair, IConvertibleChair
    {
        private const decimal ConvertedHeight = 0.10m;

        private bool isConverted;
        private decimal initialHeight;

        public ConvertibleChair(string model, string materialType, decimal price, decimal height, int numberOfLegs)
            : base(model, materialType, price, height, numberOfLegs)
        {
            this.IsConverted = false;
            this.initialHeight = height;
        }

        public bool IsConverted
        {
            get
            {
                return this.isConverted;
            }
            set
            {
                this.isConverted = value;
            }
        }

        public void Convert()
        {
            this.IsConverted = !this.IsConverted;
            UpdateAfterConvertion();
        }

        private void UpdateAfterConvertion()
        {
            if (this.isConverted)
            {
                this.Height = ConvertedHeight;
            }
            else
            {
                this.Height = this.initialHeight;
            }
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Model: {1}, Material: {2}, Price: {3}, Height: {4}, Legs: {5}, State: {6}", 
                this.GetType().Name, this.Model, this.Material, this.Price, this.Height, this.NumberOfLegs, 
                this.IsConverted ? "Converted" : "Normal");
        }
    }
}
