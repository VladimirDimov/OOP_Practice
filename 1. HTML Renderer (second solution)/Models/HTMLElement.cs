namespace HTMLRenderer.Models
{
    using HTMLRenderer.Interfaces;
    using System.Text;

    class HtmlElement : Element
    {
        public HtmlElement(string name)
            : base(name)
        {
            this.TextContent = "";
        }

        public HtmlElement(string name, string text)
            : base(name, text)
        {
        }

        public override void Render(StringBuilder output)
        {
            // <(name)>(text_content)(child_content)</(name)>
            if (this.Name != null)
            {
                output.AppendFormat("<{0}>", this.Name);
                if (this.TextContent != null)
                {
                    output.AppendFormat(this.TextContent);
                }
                foreach (var childElement in this.ChildElements)
                {
                    output.Append(childElement.ToString());
                }

                output.AppendFormat("</{0}>", this.Name);
            }
        }

    }
}
