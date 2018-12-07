using System;
using System.Collections.Generic;

namespace Helpers
{
    public struct EasyProperty
    {
        public readonly Type Type;
        public readonly string Name;

        public readonly Type AttributeType;
        public readonly Dictionary<Type, object> AttributeCtorParams;

        public EasyProperty (string name)
        {
            if (String.IsNullOrWhiteSpace (name)) throw new ArgumentException();

            Name = name.ToProperty();
            Type = typeof (object);

            AttributeType = null;
            AttributeCtorParams = new Dictionary<Type, object>();
        }

        public EasyProperty(string name, string typeName)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException();

            Name = name.ToProperty();
            Type = typeName.GetType();

            AttributeType = null;
            AttributeCtorParams = new Dictionary<Type, object>();
        }

        public EasyProperty(string name, string typeName, Type attributeType, Dictionary<Type, object> attributeCtorParams)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException();

            Name = name.ToProperty();
            Type = typeName.GetType();

            AttributeType = attributeType;
            AttributeCtorParams = attributeCtorParams ?? new Dictionary<Type, object>();
        }
    }
}
