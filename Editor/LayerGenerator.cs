using System.CodeDom;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TNRD.CodeGeneration.Layers
{
    public class LayerGenerator : ICodeGenerator
    {
        [MenuItem("TNRD/Code Generation/Layers")]
        private static void Execute()
        {
            LayerGenerator generator = new LayerGenerator();
            generator.Generate();
        }

        public void Generate()
        {
            string[] layers = InternalEditorUtility.layers
                .OrderBy(x => x)
                .ToArray();

            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace();
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("Layers")
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            for (int i = 0; i < layers.Length; i++)
            {
                string layer = layers[i];
                string layerName = Utilities.GetScreamName(layer);
                string maskName = layerName + "_MASK";
                int layerValue = LayerMask.NameToLayer(layer);

                CodeMemberField layerField = new CodeMemberField(typeof(int), layerName)
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Const,
                    InitExpression = new CodePrimitiveExpression(layerValue)
                };

                CodeMemberField maskField = new CodeMemberField(typeof(int), maskName)
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Const,
                    InitExpression = new CodePrimitiveExpression(1 << layerValue)
                };

                classDeclaration.Members.Add(layerField);
                classDeclaration.Members.Add(maskField);
            }

            codeNamespace.Types.Add(classDeclaration);
            codeCompileUnit.Namespaces.Add(codeNamespace);

            Utilities.GenerateToFile(codeCompileUnit, Application.dataPath + "/Generated", "Layers.cs");
        }
    }
}
