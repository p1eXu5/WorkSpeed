using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import.Models
{
    public struct ImportedProduct : IEquatable<ImportedProduct>
    {
        public readonly int Id;
        public readonly string Name;
        public readonly int? Parent;
        public readonly bool IsGroup;

        public ImportedProduct (int id, string name, int parent, bool isGroup)
        {
            Id = id;
            Name = name;
            Parent = parent;
            IsGroup = isGroup;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public bool Equals (ImportedProduct other)
        {
            return Id == other.Id;
        }

        public override bool Equals (object obj)
        {
            if (ReferenceEquals (null, obj)) return false;
            return obj is ImportedProduct other && Equals (other);
        }

        public static bool operator == (ImportedProduct productA, ImportedProduct productB)
        {
            return productA.Equals (productB);
        }

        public static bool operator != (ImportedProduct productA, ImportedProduct productB)
        {
            return !productA.Equals(productB);
        }
    }

}
