using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using UnityEditor;

namespace TNRD.CodeGeneration
{
    public static class Utilities
    {
        [MenuItem("TNRD/Code Generation/Generate All", false, int.MaxValue)]
        private static void GenerateAll()
        {
            Type typeDefinition = typeof(ICodeGenerator);

            Assembly assembly = Assembly.GetExecutingAssembly();
            List<Type> types = assembly.GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeDefinition))
                .ToList();

            for (int i = 0; i < types.Count; i++)
            {
                Type type = types[i];
                if (type.IsAbstract)
                    continue;

                ICodeGenerator instance = (ICodeGenerator) Activator.CreateInstance(type);
                instance.Generate();
            }
        }

        public static string GetScreamName(string name)
        {
            string formattedName = "";

            name = FilterSpaces(name);

            for (int i = 0; i < name.Length; i++)
            {
                if (i == 0)
                {
                    formattedName += name[i].ToString();
                    continue;
                }

                char c = name[i];
                char pc = name[i - 1];
                if (char.IsUpper(c) && char.IsLower(pc))
                    formattedName += "_";

                formattedName += c.ToString();
            }

            return formattedName.ToUpper();
        }

        private static string FilterSpaces(string name)
        {
            int index = -1;

            while ((index = name.IndexOf(' ')) != -1)
            {
                if (index == name.Length - 1)
                {
                    name = name.Remove(index, 1);
                    return name;
                }

                string upperChar = char.ToUpper(name[index + 1]).ToString();
                name = name.Remove(index, 2);
                name = name.Insert(index, upperChar);
            }

            return name;
        }

        public static void GenerateToFile(CodeCompileUnit unit, string directory, string filename)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };

            StringWriter writer = new StringWriter();
            codeProvider.GenerateCodeFromCompileUnit(unit, writer, options);
            writer.Flush();
            string output = writer.ToString();

            string directoryPath = directory;
            string filePath = directoryPath + "/" + filename;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            File.WriteAllText(filePath, output);
            AssetDatabase.Refresh();
        }
    }
}
