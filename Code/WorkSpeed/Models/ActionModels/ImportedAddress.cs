using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.ActionModels
{
    public struct ImportedAddress
    {
        public readonly ushort Id;
        public readonly string Name;

        public ImportedAddress (ushort id, string name)
        {
            Id = id;
            Name = name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
