using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

public class Command : ICommand
{
    private const char CommandNameSeparator = '(';
    private const char CommandParameterSeparator = ';';
    private const char CommandValueSeparator = '=';

    private string name;
    private IDictionary<string, string> parameters = new Dictionary<string, string>();

    public Command(string input)
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
                throw new ArgumentNullException("The command name is required.");
            }

            this.name = value;
        }
    }

    public IDictionary<string, string> Parameters
    {
        get
        {
            return this.parameters;
        }

        private set
        {
            if (value == null)
            {
                throw new ArgumentNullException("The command parameters are required.");
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
        int parametersBeginning = input.IndexOf(CommandNameSeparator);

        this.Name = input.Substring(0, parametersBeginning);
        var parametersKeysAndValues = input.Substring(parametersBeginning + 1, input.Length - parametersBeginning - 2)
            .Split(new[] { CommandParameterSeparator }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var parameter in parametersKeysAndValues)
        {
            var split = parameter.Split(new[] { CommandValueSeparator }, StringSplitOptions.RemoveEmptyEntries);
            this.Parameters.Add(split[0], split[1]);
        }
    }
}


public class ConsoleInterface : IUserInterface
{
    public IEnumerable<string> Input()
    {
        string currentLine = Console.ReadLine();
        while (currentLine != "End")
        {
            yield return currentLine;
            currentLine = Console.ReadLine();
        }
    }

    public void Output(IEnumerable<string> output)
    {
        var result = new StringBuilder();
        foreach (string line in output)
        {
            result.AppendLine(line);
        }

        Console.Write(result.ToString());
    }
}


public class Dessert : Meal, IDessert
{
    private bool withSugar;

    public Dessert(string name, decimal price, int calories,
        int quantityPerServing, int timeToPrepare, bool isVegan)
        : base(name, price, calories, quantityPerServing, timeToPrepare, isVegan)
    {
        this.WithSugar = true;
    }

    public bool WithSugar
    {
        get
        {
            return this.withSugar;
        }
        private set
        {
            this.withSugar = value;
        }
    }

    public void ToggleSugar()
    {
        this.WithSugar = !this.WithSugar;
    }
}


class Drink : Recipe, IDrink
{
    private bool isCarbonated;

    public Drink(string name, decimal price, int calories, int quantityPerServing,
        int timeToPrepare, bool isCarbonated)
        : base(name, price, calories, quantityPerServing, timeToPrepare)
    {
        this.IsCarbonated = isCarbonated;

        if (calories > 100)
        {
            throw new ArgumentOutOfRangeException("Drink calories must not be greater than 100");
        }

        if (timeToPrepare > 20)
        {
            throw new ArgumentOutOfRangeException("Drink time to prepare must not be greater than 20");
        }

        this.Unit = MetricUnit.Milliliters;
    }

    public bool IsCarbonated
    {
        get
        {
            return this.isCarbonated;
        }
        private set
        {
            this.isCarbonated = value;
        }
    }
}

internal static class EngineConstants
{
    #region Commands
    internal const string CreateRestaurantCommand = "CreateRestaurant";
    internal const string CreateDrinkCommand = "CreateDrink";
    internal const string CreateSaladCommand = "CreateSalad";
    internal const string CreateMainCourseCommand = "CreateMainCourse";
    internal const string CreateDessertCommand = "CreateDessert";
    internal const string ToggleSugarCommand = "ToggleSugar";
    internal const string ToggleVeganCommand = "ToggleVegan";
    internal const string AddRecipeToRestaurantCommand = "AddRecipeToRestaurant";
    internal const string RemoveRecipeFromRestaurantCommand = "RemoveRecipeFromRestaurant";
    internal const string PrintRestaurantMenuCommand = "PrintRestaurantMenu";

    #endregion

    #region Error messages
    internal const string InvalidCommandMessage = "Invalid command name: {0}";
    internal const string RestaurantExistsMessage = "The restaurant {0} already exists";
    internal const string RecipeExistsMessage = "The recipe {0} already exists";
    internal const string RestaurantDoesNotExistMessage = "The restaurant {0} does not exist";
    internal const string RecipeDoesNotExistMessage = "The recipe {0} does not exist";
    internal const string InapplicableCommandMessage = "The command {0} is not applicable to recipe {1}";
    #endregion

    #region Success messages
    internal const string RestaurantCreatedMessage = "Restaurant {0} created";
    internal const string RecipeCreatedMessage = "Recipe {0} created";
    internal const string CommandSuccessfulMessage = "Command {0} executed successfully. New value: {1}";
    internal const string RecipeAddedMessage = "Recipe {0} successfully added to restaurant {1}";
    internal const string RecipeRemovedMessage = "Recipe {0} successfully removed from restaurant {1}";
    #endregion
}


public interface ICommand
{
    string Name { get; }

    IDictionary<string, string> Parameters { get; }
}

public interface IDessert : IMeal
{
    bool WithSugar { get; }

    void ToggleSugar(); // Turns "with sugar" to "without sugar" and vice versa
}

public interface IDrink : IRecipe
{
    bool IsCarbonated { get; }
}


public interface IMainCourse : IMeal
{
    MainCourseType Type { get; }
}

public interface IMeal : IRecipe
{
    bool IsVegan { get; }

    void ToggleVegan(); // Turns "vegan" to "not vegan" and vice versa
}


public interface IRecipe
{
    string Name { get; }

    decimal Price { get; }

    int Calories { get; }

    int QuantityPerServing { get; }

    MetricUnit Unit { get; }

    int TimeToPrepare { get; }
}

public interface IRecipeFactory
{
    IDrink CreateDrink(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isCarbonated);

    ISalad CreateSalad(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool containsPasta);

    IMainCourse CreateMainCourse(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isVegan, string type);

    IDessert CreateDessert(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isVegan);
}


public interface IRestaurant
{
    string Name { get; }

    string Location { get; }

    IList<IRecipe> Recipes { get; }

    void AddRecipe(IRecipe recipe);

    void RemoveRecipe(IRecipe recipe);

    string PrintMenu();
}

public interface IRestaurantFactory
{
    IRestaurant CreateRestaurant(string name, string location);
}

public interface IRestaurantManagerEngine
{
    void Start();
}

public interface ISalad : IMeal
{
    bool ContainsPasta { get; }
}


public interface IUserInterface
{
    IEnumerable<string> Input();

    void Output(IEnumerable<string> output);
}


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
}

public enum MainCourseType
{
    Soup,
    Entree,
    Pasta,
    Side,
    Meat,
    Other
}


public class Meal : Recipe, IMeal
{
    private bool isVegan;

    public Meal(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare,
        bool isVegan)
        : base(name, price, calories, quantityPerServing, timeToPrepare)
    {
        this.IsVegan = isVegan;
        this.Unit = MetricUnit.Grams;
    }

    public bool IsVegan
    {
        get
        {
            return this.isVegan;
        }
        private set
        {
            this.isVegan = value;
        }
    }

    public void ToggleVegan()
    {
        this.IsVegan = !this.IsVegan;
    }
}

public enum MetricUnit
{
    Grams,
    Milliliters
}


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

public class RecipeFactory : IRecipeFactory
{
    public IDrink CreateDrink(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isCarbonated)
    {
        return new Drink(name, price, calories, quantityPerServing, timeToPrepare, isCarbonated);
    }

    public ISalad CreateSalad(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool containsPasta)
    {
        return new Salad(name, price, calories, quantityPerServing, timeToPrepare, containsPasta);
    }

    public IMainCourse CreateMainCourse(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isVegan, string type)
    {
        return new MainCourse(name, price, calories, quantityPerServing, timeToPrepare, isVegan, type);
    }

    public IDessert CreateDessert(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isVegan)
    {
        return new Dessert(name, price, calories, quantityPerServing, timeToPrepare, isVegan);
    }
}


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
                .Select(x => x.Name)
                .OrderBy(x => x).ToList();

            if (drinks.Count > 0)
            {
                menu.Add("~~~~~ DRINKS ~~~~~");
                foreach (var item in drinks)
                {
                    menu.Add(item);
                }
            }

            var salads = this.recipes
                .Where(x => x is ISalad)
                .Select(x => x.Name)
                .OrderBy(x => x).ToList();

            if (salads.Count > 0)
            {
                menu.Add("~~~~~~~ SALADS ~~~~~");
                foreach (var item in salads)
                {
                    menu.Add(item);
                }
            }

            var mainCourses = this.recipes
                .Where(x => x is MainCourse)
                .Select(x => x.Name)
                .OrderBy(x => x).ToList();

            if (mainCourses.Count > 0)
            {
                menu.Add("~~~~~ MAIN COURSES ~~~~~");
                foreach (var item in mainCourses)
                {
                    menu.Add(item);
                }
            }

            var dessesrts = this.recipes
                .Where(x => x is IDessert)
                .Select(x => x.Name)
                .OrderBy(x => x).ToList();

            if (mainCourses.Count > 0)
            {
                menu.Add("~~~~~ DESSERTS ~~~~~");
                foreach (var item in dessesrts)
                {
                    menu.Add(item);
                }
            }
        }

        return string.Join(Environment.NewLine, menu);
    }
}


public class RestaurantFactory : IRestaurantFactory
{
    public IRestaurant CreateRestaurant(string name, string location)
    {
        return new Restaurant(name, location);
    }
}


public sealed class RestaurantManagerEngine : IRestaurantManagerEngine
{
    private static IRestaurantManagerEngine instance;

    private readonly IRestaurantFactory restaurantFactory;
    private readonly IRecipeFactory recipeFactory;

    private readonly IDictionary<string, IRestaurant> restaurants;
    private readonly IDictionary<string, IRecipe> recipes;

    private readonly IUserInterface userInterface;

    private RestaurantManagerEngine()
    {
        this.restaurantFactory = new RestaurantFactory();
        this.recipeFactory = new RecipeFactory();
        this.restaurants = new Dictionary<string, IRestaurant>();
        this.recipes = new Dictionary<string, IRecipe>();
        this.userInterface = new ConsoleInterface();
    }

    public static IRestaurantManagerEngine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new RestaurantManagerEngine();
            }

            return instance;
        }
    }

    public void Start()
    {
        var commands = this.ReadCommands();
        var commandResults = this.ProcessCommands(commands);
        this.userInterface.Output(commandResults);
    }

    private ICollection<ICommand> ReadCommands()
    {
        var commands = new List<ICommand>();
        foreach (var line in this.userInterface.Input())
        {
            commands.Add(Command.Parse(line));
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
                case EngineConstants.CreateRestaurantCommand:
                    commandResult = this.CreateRestaurant(
                        command.Parameters["name"],
                        command.Parameters["location"]);
                    break;
                case EngineConstants.CreateDrinkCommand:
                    commandResult = this.CreateDrink(
                        command.Parameters["name"],
                        decimal.Parse(command.Parameters["price"]),
                        int.Parse(command.Parameters["calories"]),
                        int.Parse(command.Parameters["quantity"]),
                        int.Parse(command.Parameters["time"]),
                        this.ParseBoolean(command.Parameters["carbonated"]));
                    break;
                case EngineConstants.CreateSaladCommand:
                    commandResult = this.CreateSalad(
                        command.Parameters["name"],
                        decimal.Parse(command.Parameters["price"]),
                        int.Parse(command.Parameters["calories"]),
                        int.Parse(command.Parameters["quantity"]),
                        int.Parse(command.Parameters["time"]),
                        this.ParseBoolean(command.Parameters["pasta"]));
                    break;
                case EngineConstants.CreateMainCourseCommand:
                    commandResult = this.CreateMainCourse(
                        command.Parameters["name"],
                        decimal.Parse(command.Parameters["price"]),
                        int.Parse(command.Parameters["calories"]),
                        int.Parse(command.Parameters["quantity"]),
                        int.Parse(command.Parameters["time"]),
                        this.ParseBoolean(command.Parameters["vegan"]),
                        command.Parameters["type"]);
                    break;
                case EngineConstants.CreateDessertCommand:
                    commandResult = this.CreateDessert(
                        command.Parameters["name"],
                        decimal.Parse(command.Parameters["price"]),
                        int.Parse(command.Parameters["calories"]),
                        int.Parse(command.Parameters["quantity"]),
                        int.Parse(command.Parameters["time"]),
                        this.ParseBoolean(command.Parameters["vegan"]));
                    break;
                case EngineConstants.ToggleVeganCommand:
                    commandResult = this.ToggleVegan(command.Parameters["name"]);
                    break;
                case EngineConstants.ToggleSugarCommand:
                    commandResult = this.ToggleSugar(command.Parameters["name"]);
                    break;
                case EngineConstants.AddRecipeToRestaurantCommand:
                    commandResult = this.AddRecipeToRestaurant(command.Parameters["restaurant"], command.Parameters["recipe"]);
                    break;
                case EngineConstants.RemoveRecipeFromRestaurantCommand:
                    commandResult = this.RemoveRecipeFromRestaurant(command.Parameters["restaurant"], command.Parameters["recipe"]);
                    break;
                case EngineConstants.PrintRestaurantMenuCommand:
                    commandResult = this.PrintRestaurantMenu(command.Parameters["name"]);
                    break;
                default:
                    commandResult = string.Format(EngineConstants.InvalidCommandMessage, command.Name);
                    break;
            }

            commandResults.Add(commandResult);
        }

        return commandResults;
    }

    private bool ParseBoolean(string boolValue)
    {
        if (boolValue == "yes")
        {
            return true;
        }
        else if (boolValue == "no")
        {
            return false;
        }
        else
        {
            throw new ArgumentException("Invalid boolean value provided: " + boolValue);
        }
    }

    private string CreateRestaurant(string name, string location)
    {
        if (this.restaurants.ContainsKey(name))
        {
            return string.Format(EngineConstants.RestaurantExistsMessage, name);
        }

        var restaurant = this.restaurantFactory.CreateRestaurant(name, location);
        this.restaurants.Add(name, restaurant);
        return string.Format(EngineConstants.RestaurantCreatedMessage, name);
    }

    private string CreateDrink(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isCarbonated)
    {
        if (this.recipes.ContainsKey(name))
        {
            return string.Format(EngineConstants.RecipeExistsMessage, name);
        }

        var drink = this.recipeFactory.CreateDrink(name, price, calories, quantityPerServing, timeToPrepare, isCarbonated);
        this.recipes.Add(name, drink);
        return string.Format(EngineConstants.RecipeCreatedMessage, name);
    }

    private string CreateSalad(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool containsPasta)
    {
        if (this.recipes.ContainsKey(name))
        {
            return string.Format(EngineConstants.RecipeExistsMessage, name);
        }

        var salad = this.recipeFactory.CreateSalad(name, price, calories, quantityPerServing, timeToPrepare, containsPasta);
        this.recipes.Add(name, salad);
        return string.Format(EngineConstants.RecipeCreatedMessage, name);
    }

    private string CreateMainCourse(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isVegan, string type)
    {
        if (this.recipes.ContainsKey(name))
        {
            return string.Format(EngineConstants.RecipeExistsMessage, name);
        }

        var mainCourse = this.recipeFactory.CreateMainCourse(name, price, calories, quantityPerServing, timeToPrepare, isVegan, type);
        this.recipes.Add(name, mainCourse);
        return string.Format(EngineConstants.RecipeCreatedMessage, name);
    }

    private string CreateDessert(string name, decimal price, int calories, int quantityPerServing, int timeToPrepare, bool isVegan)
    {
        if (this.recipes.ContainsKey(name))
        {
            return string.Format(EngineConstants.RecipeExistsMessage, name);
        }

        var dessert = this.recipeFactory.CreateDessert(name, price, calories, quantityPerServing, timeToPrepare, isVegan);
        this.recipes.Add(name, dessert);
        return string.Format(EngineConstants.RecipeCreatedMessage, name);
    }

    private string ToggleVegan(string name)
    {
        if (!this.recipes.ContainsKey(name))
        {
            return string.Format(EngineConstants.RecipeDoesNotExistMessage, name);
        }

        var recipe = this.recipes[name];
        if (recipe is IMeal)
        {
            var meal = recipe as IMeal;
            try
            {
                meal.ToggleVegan();
            }
            catch (ArgumentException)
            {
                return string.Format(EngineConstants.InapplicableCommandMessage, "ToggleVegan", name);
            }

            return string.Format(EngineConstants.CommandSuccessfulMessage, "ToggleVegan", meal.IsVegan.ToString().ToLower());
        }
        else
        {
            return string.Format(EngineConstants.InapplicableCommandMessage, "ToggleVegan", name);
        }
    }

    private string ToggleSugar(string name)
    {
        if (!this.recipes.ContainsKey(name))
        {
            return string.Format(EngineConstants.RecipeDoesNotExistMessage, name);
        }

        var recipe = this.recipes[name];
        if (recipe is IDessert)
        {
            var dessert = recipe as IDessert;
            dessert.ToggleSugar();
            return string.Format(EngineConstants.CommandSuccessfulMessage, "ToggleSugar", dessert.WithSugar.ToString().ToLower());
        }
        else
        {
            return string.Format(EngineConstants.InapplicableCommandMessage, "ToggleSugar", name);
        }
    }

    private string AddRecipeToRestaurant(string restaurantName, string recipeName)
    {
        if (!this.restaurants.ContainsKey(restaurantName))
        {
            return string.Format(EngineConstants.RestaurantDoesNotExistMessage, restaurantName);
        }

        if (!this.recipes.ContainsKey(recipeName))
        {
            return string.Format(EngineConstants.RecipeDoesNotExistMessage, recipeName);
        }

        this.restaurants[restaurantName].AddRecipe(this.recipes[recipeName]);
        return string.Format(EngineConstants.RecipeAddedMessage, recipeName, restaurantName);
    }

    private string RemoveRecipeFromRestaurant(string restaurantName, string recipeName)
    {
        if (!this.restaurants.ContainsKey(restaurantName))
        {
            return string.Format(EngineConstants.RestaurantDoesNotExistMessage, restaurantName);
        }

        if (!this.recipes.ContainsKey(recipeName))
        {
            return string.Format(EngineConstants.RecipeDoesNotExistMessage, recipeName);
        }

        this.restaurants[restaurantName].RemoveRecipe(this.recipes[recipeName]);
        return string.Format(EngineConstants.RecipeRemovedMessage, recipeName, restaurantName);
    }

    private string PrintRestaurantMenu(string name)
    {
        if (!this.restaurants.ContainsKey(name))
        {
            return string.Format(EngineConstants.RestaurantDoesNotExistMessage, name);
        }

        return this.restaurants[name].PrintMenu();
    }
}


public class RestaurantManagementProgram
{
    public static void Main()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        RestaurantManagerEngine.Instance.Start();
    }
}


public class Salad : Meal, ISalad
{
    private const bool IsSaladVegan = true;

    private bool containsPasta;

    public Salad(string name, decimal price, int calories, int quantityPerServing,
        int timeToPrepare, bool containsPasta)
        : base(name, price, calories, quantityPerServing, timeToPrepare, IsSaladVegan)
    {
        this.ContainsPasta = containsPasta;
    }

    public bool ContainsPasta
    {
        get
        {
            return this.containsPasta;
        }
        private set
        {
            this.containsPasta = value;
        }
    }
}