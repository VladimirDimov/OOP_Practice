using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAndTravel
{
    class ExtendedInteractionManager : InteractionManager
    {
        protected override Item CreateItem(string itemTypeString, string itemNameString, Location itemLocation, Item item)
        {
            switch (itemTypeString)
            {
                case "armor":
                    return base.CreateItem(itemTypeString, itemNameString, itemLocation, item);
                case "weapon":
                    {
                        item = new Weapon(itemNameString, itemLocation);
                        break;
                    }
                case "wood":
                    {
                        item = new Wood(itemNameString, itemLocation);
                        break;
                    }
                case "iron":
                    {
                        item = new Iron(itemNameString, itemLocation);
                        break;
                    }
                default:
                    break;
            }

            return item;
        }

        protected override Location CreateLocation(string locationTypeString, string locationName)
        {
            Location location = null;
            switch (locationTypeString)
            {
                case "town":
                    return base.CreateLocation(locationTypeString, locationName);
                case "mine":
                    {
                        location = new Mine(locationName);
                        break;
                    }
                case "forest":
                    {
                        location = new Forest(locationName);
                        break;
                    }
                default:
                    break;
            }

            return location;
        }

        protected override void HandlePersonCommand(string[] commandWords, Person actor)
        {
            switch (commandWords[1])
            {
                case "drop":
                case "pickup":
                case "sell":
                case "buy":
                case "inventory":
                case "money":
                case "travel":
                    base.HandlePersonCommand(commandWords, actor);
                    break;
                case "gather":
                    {
                        HandleGatherCommand(actor, commandWords[2]);
                        break;
                    }
                case "craft":
                    {
                        HandlePersonCraftCommand(actor, commandWords[2], commandWords[3]);
                        break;
                    }
                default:
                    break;
            }
        }

        private void HandlePersonCraftCommand(Person actor, string type, string itemName)
        {
            if (type == "weapon")
            {
                if (actor.ListInventory().FirstOrDefault(x => x.ItemType == ItemType.Wood) != null)
                {
                    actor.AddToInventory(new Weapon(itemName, actor.Location));
                }
            }
            else if (type == "armor")
            {
                if (actor.ListInventory().FirstOrDefault(x => x.ItemType == ItemType.Iron) != null)
                {
                    actor.AddToInventory(new Armor(itemName, actor.Location));
                }
            }
        }

        private void HandleGatherCommand(Person actor, string itemName)
        {
            Location location = actor.Location;
            if (location is Forest)
            {
                if (actor.ListInventory().Any(x => x.ItemType == ItemType.Weapon))
                {
                    this.AddToPerson(actor, new Wood(itemName, null));
                }
            }
            else if (location is Mine)
            {
                if (actor.ListInventory().Any(x => x.ItemType == ItemType.Armor))
                {
                    this.AddToPerson(actor, new Iron(itemName, null));
                }
            }
        }

        protected override Person CreatePerson(string personTypeString, string personNameString, Location personLocation)
        {
            Person person = null;
            switch (personTypeString)
            {
                case "shopkeeper":
                case "traveller":
                    return base.CreatePerson(personTypeString, personNameString, personLocation);

                case "merchant":
                    {
                        person = new Merchant(personNameString, personLocation);
                        break;
                    }
                default:
                    break;
            }
            return person;
        }

    }
}
