using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace TNRD.CodeGeneration
{
    public static class Utilities
    {
        [MenuItem("TNRD/Code Generation/Generate All", false, Int32.MaxValue)]
        private static void GenerateAll()
        {
            var typeDefinition = typeof(ICodeGenerator);
            
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeDefinition))
                .ToList();
            
            for (int i = 0; i < types.Count; i++)
            {
                var type = types[i];
                if (type.IsAbstract)
                    continue;

                var instance = (ICodeGenerator) Activator.CreateInstance(type);
                instance.Generate();
            }
        }

        public static string GetScreamName(string name)
        {
            var formattedName = "";

            name = FilterSpaces(name);

            for (int i = 0; i < name.Length; i++)
            {
                if (i == 0)
                {
                    formattedName += name[i].ToString();
                    continue;
                }

                var c = name[i];
                var pc = name[i - 1];
                if (char.IsUpper(c) && char.IsLower(pc))
                    formattedName += "_";

                formattedName += c.ToString();
            }

            return formattedName.ToUpper();
        }

        private static string FilterSpaces(string name)
        {
            var index = -1;

            while ((index = name.IndexOf(' ')) != -1)
            {
                if (index == name.Length - 1)
                {
                    name = name.Remove(index, 1);
                    return name;
                }

                var upperChar = char.ToUpper(name[index + 1]).ToString();
                name = name.Remove(index, 2);
                name = name.Insert(index, upperChar);
            }

            return name;
        }
    }
}
