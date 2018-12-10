using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.ActionModels
{
    public struct ImportedOperation
    {
        public readonly byte Id;
        public readonly string Name;

        public ImportedOperation (byte id, string name)
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
