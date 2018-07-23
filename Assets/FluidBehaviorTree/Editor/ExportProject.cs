using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Adnc.FluidBT.Editor {
    public class ExportProject {
        [MenuItem("Export/Fluid Behavior Tree")]
        static void BuildProject () {
            var core = GetCoreFiles();
            var examples = GetExampleFiles();
            var path = EditorUtility.OpenFolderPanel("Select Output Location", "", "");
            
            AssetDatabase.ExportPackage(core.ToArray(), $"{path}/Fluid Behavior Tree.unitypackage");
            AssetDatabase.ExportPackage(examples.ToArray(), $"{path}/Fluid Behavior Tree - Examples.unitypackage");
        }

        private static List<string> GetCoreFiles () {
            return AssetDatabase
                .FindAssets("", new[] {"Assets/FluidBehaviorTree"}).ToList()
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(file => {
                    var invalidFolder = file.Contains("Assets/FluidBehaviorTree/Editor") ||
                                        file.Contains("Assets/FluidBehaviorTree/Examples");

                    if (invalidFolder) return false;

                    return !file.Contains("Test.cs");
                }).ToList();
        }

        private static List<string> GetExampleFiles () {
            return AssetDatabase
                .FindAssets("", new[] {"Assets/FluidBehaviorTree/Examples"}).ToList()
                .Select(AssetDatabase.GUIDToAssetPath).ToList();
        }
    }
}