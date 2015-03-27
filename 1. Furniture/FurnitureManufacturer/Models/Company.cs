using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureManufacturer.Models
{
    class Company : FurnitureManufacturer.Interfaces.ICompany
    {
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string RegistrationNumber
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection<Interfaces.IFurniture> Furnitures
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(Interfaces.IFurniture furniture)
        {
            throw new NotImplementedException();
        }

        public void Remove(Interfaces.IFurniture furniture)
        {
            throw new NotImplementedException();
        }

        public Interfaces.IFurniture Find(string model)
        {
            throw new NotImplementedException();
        }

        public string Catalog()
        {
            throw new NotImplementedException();
        }
    }
}
