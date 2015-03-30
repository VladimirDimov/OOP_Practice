using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infestation
{
    class ExtendedHoldingPen : HoldingPen
    {
        protected override void ExecuteAddSupplementCommand(string[] commandWords)
        {
            switch (commandWords[1])
            {
                case "PowerCatalyst":
                    var powerCatalyst = new PowerCatalyst();
                    GetUnit(commandWords[2]).AddSupplement(powerCatalyst);
                    break;
                case "HealthCatalyst":
                    var healthCatalyst = new HealthCatalyst();
                    GetUnit(commandWords[2]).AddSupplement(healthCatalyst);
                    break;
                case "AggressionCatalyst":
                    var agressionCatalyst = new AggressionCatalyst();
                    GetUnit(commandWords[2]).AddSupplement(agressionCatalyst);
                    break;
                case "Weapon":
                    var weapon = new Weapon();
                    GetUnit(commandWords[2]).AddSupplement(weapon);
                    break;
                default:
                    base.ExecuteAddSupplementCommand(commandWords);
                    break;
            }
        }

        protected override void ExecuteInsertUnitCommand(string[] commandWords)
        {
            switch (commandWords[1])
            {
                case "Tank":
                    var tank = new Tank(commandWords[2]);
                    this.InsertUnit(tank);
                    break;
                case "Marine":
                    var marine = new Marine(commandWords[2]);
                    this.InsertUnit(marine);
                    break;
                case "Parasite":
                    var parasite = new Parasite(commandWords[2]);
                    this.InsertUnit(parasite);
                    break;
                default:
                    base.ExecuteInsertUnitCommand(commandWords);
                    break;
            }
        }

        protected override void ProcessSingleInteraction(Interaction interaction)
        {
            switch (interaction.InteractionType)
            {
                case InteractionType.Infest:                    
                    Unit targetUnit = this.GetUnit(interaction.TargetUnit);
                    targetUnit.AddSupplement(new InfestationSpores());
                    break;
                default: 
                    base.ProcessSingleInteraction(interaction);
                    break;
            }
        }
    }
}
