namespace HTMLRenderer.Models
{
    using HTMLRenderer.Interfaces;
    using System.Collections.Generic;
    using System.Text;

    abstract class Element : IElement
    {
        private string name;
        private string textContent;
        private ICollection<IElement> childElements;

        public Element(string name)
        {           
            this.Name = name;
            this.childElements = new List<IElement>();
        }

        public Element(string name, string text)
            : this(name)
        {
            this.TextContent = text;
        }
                
        public string Name
        {
            get { return this.name; }
            private set { this.name = value; }
        }

        public string TextContent
        {
            get
            {
                return this.textContent;
            }
            set
            {
                this.textContent = value;
            }
        }

        public IEnumerable<IElement> ChildElements
        {
            get { return this.childElements; }
        }

        public void AddElement(IElement element)
        {
            this.childElements.Add(element);
        }

        public abstract void Render(StringBuilder output);

        public override string ToString()
        {
            var builder = new StringBuilder();
            Render(builder);
            return builder.ToString();
        }

    }
}
