using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TNRD.CodeGeneration.Tags
{
    public class TagGenerator : ICodeGenerator
    {
        [MenuItem("TNRD/Code Generation/Tags")]
        private static void Execute()
        {
            TagGenerator generator = new TagGenerator();
            generator.Generate();
        }

        public void Generate()
        {
            string[] tags = InternalEditorUtility.tags;

            Generator generator = new Generator();
            Class tagsClass = new Class("Tags");

            foreach (string tag in tags)
            {
                tagsClass.AddField(
                    new Field(Utilities.GetScreamName(tag), tag, typeof(string))
                    {
                        IsConst = true
                    });
            }

            generator.AddClass(tagsClass);
            generator.SaveToFile(Application.dataPath + "/Generated/Tags.cs");

            AssetDatabase.Refresh();
        }
    }
}
