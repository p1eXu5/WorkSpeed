using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Helpers
{
    public static class EasyTypeBuilder
    {
        private static byte _next = Byte.MinValue;

        public static Type CreateType(string typeName, string assemblyName, string moduleName, EasyProperty property)
        {
            var typeBuilder = GetTypeBuilder(assemblyName, moduleName, typeName);

            typeBuilder.CreateProperty (property);
            ++_next;

            return typeBuilder.CreateType();
        }

        public static Type CreateType(string typeName, EasyProperty property)
        {
            var typeBuilder = GetTypeBuilder($"Assembly{_next}", $"Module{_next}", typeName);

            typeBuilder.CreateProperty (property);
            ++_next;

            return typeBuilder.CreateType();
        }

        public static ICollection GetEmptyCollection(Type type)
        {
            Type t = typeof(List<>);
            var constr = t.MakeGenericType(type);
            return (ICollection)Activator.CreateInstance(constr);
        }

        /// <summary>
        /// Creates <see cref="TypeBuilder"/>
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="moduleName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static TypeBuilder GetTypeBuilder (string assemblyName, string moduleName, string typeName)
        {
            if (String.IsNullOrWhiteSpace(assemblyName)) throw new ArgumentException("Assembly name must be setted.", nameof(assemblyName));
            if (String.IsNullOrWhiteSpace(moduleName)) throw new ArgumentException("Module name must be setted.", nameof(moduleName));
            if (String.IsNullOrWhiteSpace(typeName)) throw new ArgumentException("Type name must be setted.", nameof(typeName));

            AppDomain domain = AppDomain.CurrentDomain;

            AssemblyName assemblyNameClass = new AssemblyName (assemblyName);
            AssemblyBuilder assemblyBuilder =
                domain.DefineDynamicAssembly (assemblyNameClass, AssemblyBuilderAccess.Run);

            ModuleBuilder module = assemblyBuilder.DefineDynamicModule (moduleName);

            return module.DefineType (typeName, TypeAttributes.Public);
        }

        private static PropertyBuilder CreateProperty (this TypeBuilder typeBuilder, EasyProperty property)
        {
            if (typeBuilder == null) throw new ArgumentNullException(nameof(typeBuilder), "Type builder can't be null.");

            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var fieldName = "_" + property.Name.Substring (0, 1).ToLowerInvariant() + property.Name.Substring (1);

            FieldBuilder fieldBuilder = typeBuilder.DefineField (fieldName, property.Type, FieldAttributes.Private);
            PropertyBuilder propertyBuilder =
                typeBuilder.DefineProperty (property.Name, PropertyAttributes.HasDefault, property.Type, null);

            MethodBuilder getterBuilder =
                typeBuilder.DefineMethod ($"get_{property.Name}", getSetAttr, property.Type, Type.EmptyTypes);
            LoadIlToGetter (getterBuilder, fieldBuilder);

            MethodBuilder setterBuilder =
                typeBuilder.DefineMethod ($"set_{property.Name}", getSetAttr, null, new[] {property.Type});
            LoadIlToSetter (setterBuilder, fieldBuilder);

            propertyBuilder.SetGetMethod (getterBuilder);
            propertyBuilder.SetSetMethod (setterBuilder);

            if (property.AttributeType != null) {

                var attrBuilder = GetAttributeBuilder (property.AttributeType, property.AttributeCtorParams);
                propertyBuilder.SetCustomAttribute (attrBuilder);
            }

            return propertyBuilder;

            #region Local Functions

            void LoadIlToGetter (MethodBuilder getterBldr, FieldBuilder fieldBldr)
            {
                ILGenerator getterGenerator = getterBldr.GetILGenerator();
                getterGenerator.Emit (OpCodes.Ldarg_0);
                getterGenerator.Emit (OpCodes.Ldfld, fieldBldr);
                getterGenerator.Emit (OpCodes.Ret);
            }

            void LoadIlToSetter (MethodBuilder setterBldr, FieldBuilder fieldBldr)
            {
                ILGenerator setterGenerator = setterBldr.GetILGenerator();
                setterGenerator.Emit (OpCodes.Ldarg_0);
                setterGenerator.Emit (OpCodes.Ldarg_1);
                setterGenerator.Emit (OpCodes.Stfld, fieldBldr);
                setterGenerator.Emit (OpCodes.Ret);
            }

            #endregion
        }


        private static CustomAttributeBuilder GetAttributeBuilder (Type attributeType, Dictionary<Type, object> attributeCtorParams)
        {
            ConstructorInfo ci = attributeType.GetConstructor (attributeCtorParams.Keys.ToArray()) 
                                 ?? throw new ArgumentException("Attribute type has no ctor consisted passed types.", nameof(attributeType));

            return new CustomAttributeBuilder (ci, attributeCtorParams.Values.ToArray());
        }

    }
}
