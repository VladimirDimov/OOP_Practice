using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FurnitureManufacturer.Interfaces;

    using System;
    using System.Collections.Generic;
    using FurnitureManufacturer.Engine;
    using FurnitureManufacturer.Interfaces;

namespace FurnitureManufacturer.Engine
{
    public class Command : ICommand
    {
        private const string NullOrEmptyNameErrorMessage = "Name cannot be null or empty";
        private const string NullCollectionOfParameters = "Collection of parameteres cannot be null";

        private const char SplitCommandSymbol = ' ';

        private string name;
        private IList<string> parameters;

        private Command(string input)
        {
            this.TranslateInput(input);
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
                    throw new ArgumentNullException(NullOrEmptyNameErrorMessage);
                }

                this.name = value;
            }
        }

        public IList<string> Parameters
        {
            get
            {
                return new List<string>(this.parameters);
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(NullCollectionOfParameters);
                }

                this.parameters = value;
            }
        }

        public static Command Parse(string input)
        {
            return new Command(input);
        }

        private void TranslateInput(string input)
        {
            var indexOfFirstSeparator = input.IndexOf(SplitCommandSymbol);

            this.Name = input.Substring(0, indexOfFirstSeparator);
            this.Parameters = input.Substring(indexOfFirstSeparator + 1).Split(new[] { SplitCommandSymbol }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
namespace FurnitureManufacturer.Engine
{
    using Interfaces;
    using Models;

    public class CompanyFactory : ICompanyFactory
    {
        public ICompany CreateCompany(string name, string registrationNumber)
        {
            return new Company(name, registrationNumber);
        }
    }
}
namespace FurnitureManufacturer.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using FurnitureManufacturer.Interfaces;

    public class ConsoleRenderer : IRenderer
    {
        public IEnumerable<string> Input()
        {
            var currentLine = Console.ReadLine();
            while (!string.IsNullOrEmpty(currentLine))
            {
                yield return currentLine;
                currentLine = Console.ReadLine();
            }
        }

        public void Output(IEnumerable<string> output)
        {
            var result = new StringBuilder();
            foreach (var line in output)
            {
                result.AppendLine(line);
            }

            Console.Write(result.ToString());
        }
    }
}
namespace FurnitureManufacturer.Engine
{
    internal static class EngineConstants
    {
        // Commands
        internal const string CreateCompanyCommand = "CreateCompany";
        internal const string AddFurnitureToCompanyCommand = "AddFurnitureToCompany";
        internal const string RemoveFurnitureFromCompanyCommand = "RemoveFurnitureFromCompany";
        internal const string FindFurnitureFromCompanyCommand = "FindFurnitureFromCompany";
        internal const string ShowCompanyCatalogCommand = "ShowCompanyCatalog";
        internal const string CreateTableCommand = "CreateTable";
        internal const string CreateChairCommand = "CreateChair";
        internal const string SetChairHeight = "SetChairHeight";
        internal const string ConvertChair = "ConvertChair";

        // Chair types
        internal const string NormalChairType = "Normal";
        internal const string AdjustableChairType = "Adjustable";
        internal const string ConvertibleChairType = "Convertible";

        // Error messages
        internal const string InvalidCommandErrorMessage = "Invalid command name: {0}";
        internal const string CompanyExistsErrorMessage = "Company {0} already exists";
        internal const string CompanyNotFoundErrorMessage = "Company {0} not found";
        internal const string FurnitureNotFoundErrorMessage = "Furniture {0} not found";
        internal const string FurnitureExistsErrorMessage = "Furniture {0} already exists";
        internal const string InvalidChairTypeErrorMessage = "Invalid chair type: {0}";
        internal const string FurnitureIsNotAdjustableChairErrorMessage = "{0} is not adjustable chair";
        internal const string FurnitureIsNotConvertibleChairErrorMessage = "{0} is not convertible chair";

        // Success messages
        internal const string CompanyCreatedSuccessMessage = "Company {0} created";
        internal const string FurnitureAddedSuccessMessage = "Furniture {0} added to company {1}";
        internal const string FurnitureRemovedSuccessMessage = "Furniture {0} removed from company {1}";
        internal const string TableCreatedSuccessMessage = "Table {0} created";
        internal const string ChairCreatedSuccessMessage = "Chair {0} created";
        internal const string ChairHeightAdjustedSuccessMessage = "Chair {0} adjusted to height {1}";
        internal const string ChairStateConvertedSuccessMessage = "Chair {0} converted";
    }
}
namespace FurnitureManufacturer.Engine.Factories
{
    using System;
    using Interfaces;
    using Models;

    public class FurnitureFactory : IFurnitureFactory
    {
        private const string Wooden = "wooden";
        private const string Leather = "leather";
        private const string Plastic = "plastic";
        private const string InvalidMaterialName = "Invalid material name: {0}";

        public ITable CreateTable(string model, string materialType, decimal price, decimal height, decimal length, decimal width)
        {
            return new Table(model, GetMaterialType(materialType).ToString(), price, height, length, width);
        }

        public IChair CreateChair(string model, string materialType, decimal price, decimal height, int numberOfLegs)
        {
            return new Chair(model, GetMaterialType(materialType).ToString(), price, height, numberOfLegs);
        }

        public IAdjustableChair CreateAdjustableChair(string model, string materialType, decimal price, decimal height, int numberOfLegs)
        {
            return new AdjustableChair(model, GetMaterialType(materialType).ToString(), price, height, numberOfLegs);

        }

        public IConvertibleChair CreateConvertibleChair(string model, string materialType, decimal price, decimal height, int numberOfLegs)
        {
            return new ConvertibleChair(model, GetMaterialType(materialType).ToString(), price, height, numberOfLegs);
        }

        private MaterialType GetMaterialType(string material)
        {
            switch (material)
            {
                case Wooden:
                    return MaterialType.Wooden;
                case Leather:
                    return MaterialType.Leather;
                case Plastic:
                    return MaterialType.Plastic;
                default:
                    throw new ArgumentException(string.Format(InvalidMaterialName, material));
            }
        }
    }
}
namespace FurnitureManufacturer.Engine
{
    using System.Collections.Generic;
    using Factories;
    using Interfaces;

    public sealed class FurnitureManufacturerEngine : IFurnitureManufacturerEngine
    {
        private static IFurnitureManufacturerEngine instance;

        private readonly ICompanyFactory companyFactory;
        private readonly IFurnitureFactory furnitureFactory;

        private readonly IDictionary<string, ICompany> companies;
        private readonly IDictionary<string, IFurniture> furnitures;

        private readonly IRenderer renderer;

        private FurnitureManufacturerEngine()
        {
            this.companyFactory = new CompanyFactory();
            this.furnitureFactory = new FurnitureFactory();
            this.companies = new Dictionary<string, ICompany>();
            this.furnitures = new Dictionary<string, IFurniture>();
            this.renderer = new ConsoleRenderer();
        }

        public static IFurnitureManufacturerEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FurnitureManufacturerEngine();
                }

                return instance;
            }
        }

        public void Start()
        {
            var commands = this.ReadCommands();
            var commandResults = this.ProcessCommands(commands);
            this.RenderCommandResults(commandResults);
        }

        private ICollection<ICommand> ReadCommands()
        {
            var commands = new List<ICommand>();
            foreach (var currentLine in this.renderer.Input())
            {
                var currentCommand = Command.Parse(currentLine);
                commands.Add(currentCommand);
            }

            return commands;
        }

        private IEnumerable<string> ProcessCommands(ICollection<ICommand> commands)
        {
            var commandResults = new List<string>();

            foreach (var command in commands)
            {
                string commandResult;

                switch (command.Name)
                {
                    case EngineConstants.CreateCompanyCommand:
                        var companyName = command.Parameters[0];
                        var companyRegistrationNumber = command.Parameters[1];
                        commandResult = this.CreateCompany(companyName, companyRegistrationNumber);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.AddFurnitureToCompanyCommand:
                        var companyToAddTo = command.Parameters[0];
                        var furnitureToBeAdded = command.Parameters[1];
                        commandResult = this.AddFurnitureToCompany(companyToAddTo, furnitureToBeAdded);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.RemoveFurnitureFromCompanyCommand:
                        var companyToRemoveFrom = command.Parameters[0];
                        var furnitureToBeRemoved = command.Parameters[1];
                        commandResult = this.RemoveFurnitureFromCompany(companyToRemoveFrom, furnitureToBeRemoved);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.FindFurnitureFromCompanyCommand:
                        var companyToFindFrom = command.Parameters[0];
                        var furnitureToBeFound = command.Parameters[1];
                        commandResult = this.FindFurnitureFromCompany(companyToFindFrom, furnitureToBeFound);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.ShowCompanyCatalogCommand:
                        var companyToShowCatalog = command.Parameters[0];
                        commandResult = this.ShowCompanyCatalog(companyToShowCatalog);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.CreateTableCommand:
                        var tableModel = command.Parameters[0];
                        var tableMaterial = command.Parameters[1];
                        var tablePrice = decimal.Parse(command.Parameters[2]);
                        var tableHeight = decimal.Parse(command.Parameters[3]);
                        var tableLength = decimal.Parse(command.Parameters[4]);
                        var tableWidth = decimal.Parse(command.Parameters[5]);
                        commandResult = this.CreateTable(tableModel, tableMaterial, tablePrice, tableHeight, tableLength, tableWidth);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.CreateChairCommand:
                        var chairModel = command.Parameters[0];
                        var chairMaterial = command.Parameters[1];
                        var chairPrice = decimal.Parse(command.Parameters[2]);
                        var chairHeight = decimal.Parse(command.Parameters[3]);
                        var chairLegs = int.Parse(command.Parameters[4]);
                        var chairType = command.Parameters[5];
                        commandResult = this.CreateChair(chairModel, chairMaterial, chairPrice, chairHeight, chairLegs, chairType);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.SetChairHeight:
                        var adjChairModel = command.Parameters[0];
                        var adjChairHeight = decimal.Parse(command.Parameters[1]);
                        commandResult = this.AdjustChairHeight(adjChairModel, adjChairHeight);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.ConvertChair:
                        var convertibleChairModel = command.Parameters[0];
                        commandResult = this.ConvertChair(convertibleChairModel);
                        commandResults.Add(commandResult);
                        break;
                    default:
                        commandResults.Add(string.Format(EngineConstants.InvalidCommandErrorMessage, command.Name));
                        break;
                }
            }

            return commandResults;
        }

        private void RenderCommandResults(IEnumerable<string> output)
        {
            this.renderer.Output(output);
        }

        private string CreateCompany(string name, string registrationNumber)
        {
            if (this.companies.ContainsKey(name))
            {
                return string.Format(EngineConstants.CompanyExistsErrorMessage, name);
            }

            var company = this.companyFactory.CreateCompany(name, registrationNumber);
            this.companies.Add(name, company);

            return string.Format(EngineConstants.CompanyCreatedSuccessMessage, name);
        }

        private string AddFurnitureToCompany(string companyName, string furnitureName)
        {
            if (!this.companies.ContainsKey(companyName))
            {
                return string.Format(EngineConstants.CompanyNotFoundErrorMessage, companyName);
            }

            if (!this.furnitures.ContainsKey(furnitureName))
            {
                return string.Format(EngineConstants.FurnitureNotFoundErrorMessage, furnitureName);
            }

            var company = this.companies[companyName];
            var furniture = this.furnitures[furnitureName];
            company.Add(furniture);

            return string.Format(EngineConstants.FurnitureAddedSuccessMessage, furnitureName, companyName);
        }

        private string RemoveFurnitureFromCompany(string companyName, string furnitureName)
        {
            if (!this.companies.ContainsKey(companyName))
            {
                return string.Format(EngineConstants.CompanyNotFoundErrorMessage, companyName);
            }

            if (!this.furnitures.ContainsKey(furnitureName))
            {
                return string.Format(EngineConstants.FurnitureNotFoundErrorMessage, furnitureName);
            }

            var company = this.companies[companyName];
            var furniture = this.furnitures[furnitureName];
            company.Remove(furniture);

            return string.Format(EngineConstants.FurnitureRemovedSuccessMessage, furnitureName, companyName);
        }

        private string FindFurnitureFromCompany(string companyName, string furnitureName)
        {
            if (!this.companies.ContainsKey(companyName))
            {
                return string.Format(EngineConstants.CompanyNotFoundErrorMessage, companyName);
            }

            var company = this.companies[companyName];
            var furniture = company.Find(furnitureName);
            if (furniture == null)
            {
                return string.Format(EngineConstants.FurnitureNotFoundErrorMessage, furnitureName);
            }

            return furniture.ToString();
        }

        private string ShowCompanyCatalog(string companyName)
        {
            if (!this.companies.ContainsKey(companyName))
            {
                return string.Format(EngineConstants.CompanyNotFoundErrorMessage, companyName);
            }

            return this.companies[companyName].Catalog();
        }

        private string CreateTable(string model, string material, decimal price, decimal height, decimal length, decimal width)
        {
            if (this.furnitures.ContainsKey(model))
            {
                return string.Format(EngineConstants.FurnitureExistsErrorMessage, model);
            }

            var table = this.furnitureFactory.CreateTable(model, material, price, height, length, width);
            this.furnitures.Add(model, table);

            return string.Format(EngineConstants.TableCreatedSuccessMessage, model);
        }

        private string CreateChair(string model, string material, decimal price, decimal height, int legs, string type)
        {
            if (this.furnitures.ContainsKey(model))
            {
                return string.Format(EngineConstants.FurnitureExistsErrorMessage, model);
            }

            IChair chair;
            switch (type)
            {
                case EngineConstants.NormalChairType:
                    chair = this.furnitureFactory.CreateChair(model, material, price, height, legs);
                    break;
                case EngineConstants.AdjustableChairType:
                    chair = this.furnitureFactory.CreateAdjustableChair(model, material, price, height, legs);
                    break;
                case EngineConstants.ConvertibleChairType:
                    chair = this.furnitureFactory.CreateConvertibleChair(model, material, price, height, legs);
                    break;
                default:
                    return string.Format(EngineConstants.InvalidChairTypeErrorMessage, type);
            }

            this.furnitures.Add(model, chair);

            return string.Format(EngineConstants.ChairCreatedSuccessMessage, model);
        }

        private string AdjustChairHeight(string model, decimal height)
        {
            if (!this.furnitures.ContainsKey(model))
            {
                return string.Format(EngineConstants.FurnitureNotFoundErrorMessage, model);
            }

            var adjChair = this.furnitures[model] as IAdjustableChair;
            if (adjChair == null)
            {
                return string.Format(EngineConstants.FurnitureIsNotAdjustableChairErrorMessage, model);
            }

            adjChair.SetHeight(height);

            return string.Format(EngineConstants.ChairHeightAdjustedSuccessMessage, model, height);
        }

        private string ConvertChair(string model)
        {
            if (!this.furnitures.ContainsKey(model))
            {
                return string.Format(EngineConstants.FurnitureNotFoundErrorMessage, model);
            }

            var convChair = this.furnitures[model] as IConvertibleChair;
            if (convChair == null)
            {
                return string.Format(EngineConstants.FurnitureIsNotConvertibleChairErrorMessage, model);
            }

            convChair.Convert();

            return string.Format(EngineConstants.ChairStateConvertedSuccessMessage, model);
        }
    }
}
namespace FurnitureManufacturer.Interfaces
{
    public interface IAdjustableChair : IChair
    {
        void SetHeight(decimal height);
    }
}
namespace FurnitureManufacturer.Interfaces
{
    public interface IChair : IFurniture
    {
        int NumberOfLegs { get; }
    }
}
namespace FurnitureManufacturer.Interfaces
{
    using System.Collections.Generic;

    public interface ICommand
    {
        string Name { get; }

        IList<string> Parameters { get; }
    }
}

namespace FurnitureManufacturer.Interfaces
{
    using System.Collections.Generic;

    public interface ICompany
    {
        string Name { get; }

        string RegistrationNumber { get; }

        ICollection<IFurniture> Furnitures { get; }

        void Add(IFurniture furniture);

        void Remove(IFurniture furniture);

        IFurniture Find(string model);

        string Catalog();
    }
}
namespace FurnitureManufacturer.Interfaces
{
    public interface ICompanyFactory
    {
        ICompany CreateCompany(string name, string registrationNumber);
    }
}
namespace FurnitureManufacturer.Interfaces
{
    public interface IConvertibleChair : IChair
    {
        bool IsConverted { get; }

        void Convert();
    }
}
namespace FurnitureManufacturer.Interfaces
{
    public interface IFurniture
    {
        string Model { get; }

        string Material { get; }

        decimal Price { get; set; }

        decimal Height { get; }
    }
}
namespace FurnitureManufacturer.Interfaces
{
    public interface IFurnitureFactory
    {
        ITable CreateTable(string model, string materialType, decimal price, decimal height, decimal length, decimal width);

        IChair CreateChair(string model, string materialType, decimal price, decimal height, int numberOfLegs);

        IAdjustableChair CreateAdjustableChair(string model, string materialType, decimal price, decimal height, int numberOfLegs);

        IConvertibleChair CreateConvertibleChair(string model, string materialType, decimal price, decimal height, int numberOfLegs);
    }
}
namespace FurnitureManufacturer.Interfaces
{
    public interface IFurnitureManufacturerEngine
    {
        void Start();
    }
}
namespace FurnitureManufacturer.Interfaces
{
    using System.Collections.Generic;

    public interface IRenderer
    {
        IEnumerable<string> Input();

        void Output(IEnumerable<string> output);
    }
}
namespace FurnitureManufacturer.Interfaces
{
    public interface ITable : IFurniture
    {
        decimal Length { get; }

        decimal Width { get; }

        decimal Area { get; }
    }
}
namespace FurnitureManufacturer.Models
{

    public class AdjustableChair : Chair, IAdjustableChair
    {
        public AdjustableChair(string model, string materialType, decimal price, decimal height, int numberOfLegs)
            : base(model, materialType, price, height, numberOfLegs)
        {
        }

        public void SetHeight(decimal height)
        {
            this.Height = height;  // Validated in Furniture.cs
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Model: {1}, Material: {2}, Price: {3}, Height: {4}, Legs: {5}", 
                this.GetType().Name, this.Model, this.Material, this.Price, this.Height, this.NumberOfLegs);
        }
    }
}
namespace FurnitureManufacturer.Models
{

    public class Chair : Furniture, IChair
    {
        private int numberOfLegs;

        public Chair(string model, string materialType, decimal price, decimal height, int numberOfLegs)
            : base(model, materialType, price, height)
        {
            this.NumberOfLegs = numberOfLegs;
        }

        public int NumberOfLegs
        {
            get
            {
                return this.numberOfLegs;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Number of legs must be at least 1");
                }

                this.numberOfLegs = value;
            }
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Model: {1}, Material: {2}, Price: {3}, Height: {4}, Legs: {5}", 
                this.GetType().Name, this.Model, this.Material, this.Price, this.Height, this.NumberOfLegs);
        }
    }
}
namespace FurnitureManufacturer.Models
{

    class Company : ICompany
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

        public ICollection<IFurniture> Furnitures
        {
            get
            {
                return this.furnitures;
            }
        }

        public void Add(IFurniture furniture)
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
            var sortedFurnitures = this.furnitures.OrderBy(x => x.Price).ThenBy(x => x.Model);

            foreach (var item in sortedFurnitures)
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
namespace FurnitureManufacturer.Models
{

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
namespace FurnitureManufacturer.Models
{
    public class DeleteMe
    {
        // TODO: Write all needed classes by implementing the interfaces in this namespace. You may delete this class
    }
}
namespace FurnitureManufacturer.Models
{

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
namespace FurnitureManufacturer.Models
{
    public enum MaterialType
    {
        Wooden,
        Leather,
        Plastic
    }
}
namespace FurnitureManufacturer.Models
{

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
namespace FurnitureManufacturer
{
    using Engine;

    public class FurnitureProgram
    {
        public static void Main()
        {
            FurnitureManufacturerEngine.Instance.Start();
        }
    }
}
