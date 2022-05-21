using System.IO;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    /// <summary>
    /// Determine if this is a package of the Unity Editor since Unity has no API to determine this
    /// </summary>
    public static class AssetPath
    {
        private static readonly string InProjectLocation = $"Assets{Path.DirectorySeparatorChar}FluidBehaviorTree";
        private static readonly string PackageLocation = $"Packages{Path.DirectorySeparatorChar}com.fluid.behavior-tree";
        private static readonly string WorkspaceLocation = $"Assets{Path.DirectorySeparatorChar}com.fluid.behavior-tree";

        private static string _basePath;

        public static string BasePath => GetBasePath();

        private static string GetBasePath()
        {
            if (_basePath != null)
            {
                return _basePath;
            }

            if (AssetDatabase.IsValidFolder(PackageLocation))
            {
                _basePath = PackageLocation;
                return _basePath;
            }

            if (AssetDatabase.IsValidFolder(InProjectLocation))
            {
                _basePath = InProjectLocation;
                return _basePath;
            }

            if (AssetDatabase.IsValidFolder(WorkspaceLocation))
            {
                _basePath = WorkspaceLocation;
                return _basePath;
            }

            Debug.LogError("Asset root could not be found");

            return null;
        }
    }
}
