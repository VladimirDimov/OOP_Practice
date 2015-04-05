namespace HTMLRenderer.Models
{
    using HTMLRenderer.Interfaces;
    using System;
    using System.Text;

    class HtmlTable : Element, ITable
    {
        private const string TableName = "table";
        private const string TableTextContent = null;
        private const string oRowTag = "<tr>";
        private const string cRowTag = "</tr>";
        private const string oColTag = "<td>";
        private const string cColTag = "</td>";

        private int rows;
        private int cols;
        private IElement[,] elements;

        public HtmlTable(int rows, int columns)
            : base(TableName, TableTextContent)
        {
            this.Rows = rows;
            this.Cols = columns;
            elements = new IElement[Rows, Cols];
        }

        public int Rows
        {
            get
            {
                return this.rows;
            }

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Rows number must be greater than zero");
                }

                this.rows = value;
            }
        }

        public int Cols
        {
            get
            {
                return this.cols;
            }

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Columns number must be greater than zero");
                }

                this.cols = value;
            }
        }

        public IElement this[int row, int col]
        {
            get
            {
                return this.elements[row, col];
            }
            set
            {
                this.elements[row, col] = value;
            }
        }

        public override void Render(StringBuilder output)
        {
            // <table><tr><td>(cell_0_0)</td><td>(cell_0_1)</td>…</tr><tr><td>(cell_1_0)</td><td>(cell_1_1)</td>…</tr>…</table>
            output.AppendFormat("<{0}>", this.Name);
            for (int row = 0; row < this.Rows; row++)
            {
                output.AppendFormat(oRowTag);
                for (int col = 0; col < this.Cols; col++)
                {
                    output.Append(oColTag);
                    output.Append(this.elements[row, col].ToString());
                    output.Append(cColTag);
                }

                output.AppendFormat(cRowTag);
            }

            output.AppendFormat("</{0}>", this.Name);
        }

    }
}
