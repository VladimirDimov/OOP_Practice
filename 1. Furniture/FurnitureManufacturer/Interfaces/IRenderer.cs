namespace FurnitureManufacturer.Interfaces
{
    using System.Collections.Generic;

    public interface IRenderer
    {
        IEnumerable<string> Input();

        void Output(IEnumerable<string> output);
    }
}
