using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTunesShop
{
    class Band : Performer, IBand
    {
        private IList<string> members;
        private PerformerType performerType;

        public Band(string name) : base(name)
        {
            this.performerType = PerformerType.Band;
            this.members = new List<string>();
        }

        public override PerformerType Type
        {
            get
            {
                return this.performerType;
            }
        }

        public IList<string> Members
        {
            get
            {
                return this.members;
            }
        }

        public void AddMember(string member)
        {
            this.Members.Add(member);
        }
    }
}
