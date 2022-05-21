using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    /// <summary>
    /// Determine if this is a package of the Unity Editor since Unity has no API to determine this
    /// </summary>
    public static class AssetPath
    {
        // TODO: Fix asset paths to support all platforms
        private const string PathProject = "Assets/FluidBehaviorTree";
        private const string PathPackage = "Packages/com.fluid.behavior-tree";

        private static string _basePath;

        public static string BasePath => GetBasePath();

        private static string GetBasePath()
        {
            if (_basePath != null)
            {
                return _basePath;
            }

            if (AssetDatabase.IsValidFolder(PathPackage))
            {
                _basePath = PathPackage;
                return _basePath;
            }

            if (AssetDatabase.IsValidFolder(PathProject))
            {
                _basePath = PathProject;
                return _basePath;
            }

            Debug.LogError("Asset root could not be found");

            return null;
        }
    }
}
