using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FurnitureManufacturer.Interfaces;

namespace FurnitureManufacturer.Models
{
    class Company : FurnitureManufacturer.Interfaces.ICompany
    {
        private string name;
        private string registrationNumber;
        ICollection<IFurniture> furnitures;

        public Company(string name, string registrationNumber)
        {
            this.Name = name;
            this.RegistrationNumber = registrationNumber;
            furnitures = new List<IFurniture>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 5)
                {
                    throw new ArgumentException("Name length must be at least");
                }

                this.name = value;
            }
        }

        public string RegistrationNumber
        {
            get
            {
                return this.registrationNumber;
            }
            set
            {
                if (value.Length != 10 || !value.All(x => char.IsDigit(x)))
                {
                    throw new ArgumentException("Invalid registration number");
                }

                this.registrationNumber = value;
            }
        }

        public ICollection<Interfaces.IFurniture> Furnitures
        {
            get
            {
                return this.furnitures;
            }
        }

        public void Add(Interfaces.IFurniture furniture)
        {
            this.furnitures.Add(furniture);
        }

        public void Remove(Interfaces.IFurniture furniture)
        {
            furnitures.Remove(furniture);
        }

        public Interfaces.IFurniture Find(string model)
        {
            return this.furnitures.FirstOrDefault(x => x.Model.ToLower() == model.ToLower());
        }

        public string Catalog()
        {
            var builder = new List<string>();
            builder.Add(string.Format("{0} - {1} - {2}", this.name, this.RegistrationNumber,  GetNumberOfFurnituresAsString()));
            foreach (var item in this.furnitures)
            {
                builder.Add(item.ToString());
            }

            return string.Join(Environment.NewLine, builder);
        }

        private string GetNumberOfFurnituresAsString()
        {
            int number = furnitures.Count;

            if (number == 0)
            {
                return "no furnitures";
            }
            else if (number == 1)
            {
                return "1 furniture";
            }
            else
            {
                return number + " furnitures";
            }
        }
    }
}
