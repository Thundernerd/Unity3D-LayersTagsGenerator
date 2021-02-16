using UnityEditor;
using UnityEngine;

namespace TNRD.CodeGeneration.Layers
{
    public class SortingLayerGenerator : ICodeGenerator
    {
        [MenuItem("TNRD/Code Generation/Sorting Layers")]
        private static void Execute()
        {
            SortingLayerGenerator generator = new SortingLayerGenerator();
            generator.Generate();
        }

        public void Generate()
        {
            SortingLayer[] layers = SortingLayer.layers;

            Generator generator = new Generator();
            Class @class = new Class("SortingLayers");

            for (int i = 0; i < layers.Length; i++)
            {
                SortingLayer layer = layers[i];
                string layerName = Utilities.GetScreamName(layer.name);
                string layerIdName = layerName + "_ID";

                @class.AddField(
                    new Field(layerName, layer.name, typeof(string))
                    {
                        IsConst = true
                    });

                @class.AddField(
                    new Field(layerIdName, layer.id, typeof(int))
                    {
                        IsConst = true
                    });
            }

            generator.AddClass(@class);
            generator.SaveToFile(Application.dataPath + "/Generated/SortingLayers.cs");

            AssetDatabase.Refresh();
        }
    }
}
