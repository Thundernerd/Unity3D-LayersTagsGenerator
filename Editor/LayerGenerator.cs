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
            var generator = new LayerGenerator();
            generator.Generate();
        }

        public void Generate()
        {
            var layers = InternalEditorUtility.layers;

            var generator = new Generator();
            var @class = new Class("Layers");

            for (int i = 0; i < layers.Length; i++)
            {
                var layerName = Utilities.GetScreamName(layers[i]);
                var maskName = layerName + "_MASK";

                @class.AddField(
                    new Field(layerName, i, typeof(int))
                    {
                        IsConst = true,
                    });

                @class.AddField(
                    new Field(maskName, string.Format("1 << {0}", i), typeof(int))
                    {
                        IsConst = true,
                    });
            }

            generator.AddClass(@class);
            generator.SaveToFile(Application.dataPath + "/Generated/Layers.cs");

            AssetDatabase.Refresh();
        }
    }
}
