using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    /// <summary>
    /// A centralized location that determines the visual theme of the behavior tree. Mostly light and dark mode.
    /// </summary>
    public static class BehaviorTreeDisplayTheme {
        static bool IsDarkMode => EditorGUIUtility.isProSkin;

        static Color lightModeActive { get; } = new(0.39f, 0.78f, 0.39f);
        static Color lightModeInactive { get; } = new(0.65f, 0.65f, 0.65f);
        static Color lightModeActiveText { get; } = Color.white;
        static Color lightModeInactiveText { get; } = Color.black;
        static Color lightModeActiveMainIcon { get; } = new(1, 1, 1, 1f);
        static Color lightModeInactiveMainIcon { get; } = new(1, 1, 1, 0.3f);
        static Color _lightBoxActiveBorder { get; } = Color.gray;
        static Color _lightBoxActiveBackground { get; } = Color.white;
        static Color _lightBoxInactiveBorder { get; } = new(0.6f, 0.6f, 0.6f);
        static Color _lightBoxInactiveBackground { get; } = new(0.8f, 0.8f, 0.8f);

        static Color _darkColorFadeActive { get; } = new (0.3f, 0.6f, 0.3f);
        static Color _darkColorFadeInactive { get; } = new(0.65f, 0.65f, 0.65f);
        static Color _darkColorFadeActiveText { get; } = Color.white;
        static Color _darkColorFadeInactiveText { get; } = Color.black;
        static Color _darkColorFadeActiveMainIcon { get; } = new(1, 1, 1, 1f);
        static Color _darkColorFadeInactiveMainIcon { get; } = new(1, 1, 1, 0.3f);
        static Color _darkBoxActiveBorder { get; } = Color.gray;
        static Color _darkBoxActiveBackground { get; } = Color.white;
        static Color _darkBoxInactiveBorder { get; } = new(0.6f, 0.6f, 0.6f);
        static Color _darkBoxInactiveBackground { get; } = new(0.8f, 0.8f, 0.8f);

        public static NodeBoxStyle GetBoxActive () {
            var border = IsDarkMode ? _darkBoxActiveBorder : _lightBoxActiveBorder;
            var background = IsDarkMode ? _darkBoxActiveBackground : _lightBoxActiveBackground;

            return new NodeBoxStyle(border, background);
        }

        public static NodeBoxStyle GetBoxInactive () {
            var border = IsDarkMode ? _darkBoxInactiveBorder : _lightBoxInactiveBorder;
            var background = IsDarkMode ? _darkBoxInactiveBackground : _lightBoxInactiveBackground;

            return new NodeBoxStyle(border, background);
        }

        public static ColorFader GetBackgroundFader () {
            var inactive = IsDarkMode ? _darkColorFadeInactive : lightModeInactive;
            var active = IsDarkMode ? _darkColorFadeActive : lightModeActive;

            return new ColorFader(inactive, active);
        }

        public static ColorFader GetTextFader () {
            var inactive = IsDarkMode ? _darkColorFadeInactiveText : lightModeInactiveText;
            var active = IsDarkMode ? _darkColorFadeActiveText : lightModeActiveText;

            return new ColorFader(inactive, active);
        }

        public static ColorFader GetMainIconFader () {
            var inactive = IsDarkMode ? _darkColorFadeInactiveMainIcon : lightModeInactiveMainIcon;
            var active = IsDarkMode ? _darkColorFadeActiveMainIcon : lightModeActiveMainIcon;

            return new ColorFader(inactive, active);
        }
    }
}
