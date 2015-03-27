using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureManufacturer.Models
{
    public class ConvertibleChair : Chair, FurnitureManufacturer.Interfaces.IConvertibleChair
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
    }
}
