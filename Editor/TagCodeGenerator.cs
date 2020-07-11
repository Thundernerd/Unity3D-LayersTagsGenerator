using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TNRD.CodeGeneration.Tags
{
    public class TagCodeGenerator : ICodeGenerator
    {
        [MenuItem("TNRD/Code Generation/Tags")]
        private static void Execute()
        {
            var generator = new TagCodeGenerator();
            generator.Generate();
        }

        public void Generate()
        {
            var tags = InternalEditorUtility.tags;

            var generator = new Generator();
            var tagsClass = new Class("Tags");

            foreach (var tag in tags)
            {
                tagsClass.AddField(
                    new Field(Utilities.GetScreamName(tag), tag, typeof(string))
                {
                    IsConst = true,
                });
            }

            generator.AddClass(tagsClass);
            generator.SaveToFile(Application.dataPath + "/Generated/Tags.cs");

            AssetDatabase.Refresh();
        }
    }
}
