using UnityEngine;
using UnityEditor;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class NodeFaders {
        public ColorFader BackgroundFader { get; }

        public ColorFader TextFader { get; }

        public ColorFader MainIconFader { get; }

        public NodeFaders() {
            // If in dark mode choose dark mode
            if (EditorGUIUtility.isProSkin){
                BackgroundFader = new ColorFader(
                    DisplaySettings.darkModeInactive,
                    DisplaySettings.darkModeActive);
                TextFader = new ColorFader(
                    DisplaySettings.darkModeInactiveText,
                    DisplaySettings.darkModeActiveText);
                MainIconFader = new ColorFader(
                    DisplaySettings.darkModeInactiveMainIcon,
                    DisplaySettings.darkModeActiveMainIcon);
            }else {
                BackgroundFader = new ColorFader(
                    DisplaySettings.lightModeInactive,
                    DisplaySettings.lightModeActive);
                TextFader = new ColorFader(
                    DisplaySettings.lightModeInactiveText,
                    DisplaySettings.lightModeActiveText);
                MainIconFader = new ColorFader(
                    DisplaySettings.lightModeInactiveMainIcon,
                    DisplaySettings.lightModeActiveMainIcon);
            }
        }

        public void Update (bool active) {
            BackgroundFader.Update(active);
            TextFader.Update(active);
            MainIconFader.Update(active);
        }
    }
}
