using System.Linq;
using UnityEngine;

/// <summary>
/// Display settings of the behaviour tree window
/// </summary>
public class DisplaySettings {
    public static Color lightModeActive { get; } = new Color(0.39f, 0.78f, 0.39f);
    public static Color lightModeInactive { get; } = new Color(0.65f, 0.65f, 0.65f);
    public static Color lightModeActiveText { get; } = Color.white;
    public static Color lightModeInactiveText { get; } = Color.black;
    public static Color lightModeActiveMainIcon { get; } = new Color(1, 1, 1, 1f);
    public static Color lightModeInactiveMainIcon { get; } = new Color(1, 1, 1, 0.3f);

    public static Color darkModeActive { get; } = Color.white;
    public static Color darkModeInactive { get; } = new Color(0f, 0f, 0f, 1f);
    public static Color darkModeActiveText { get; } = Color.white;
    public static Color darkModeInactiveText { get; } = Color.grey;
    public static Color darkModeActiveMainIcon { get; } = new Color(1, 1, 1, 1f);
    public static Color darkModeInactiveMainIcon { get; } = new Color(1, 1, 1, 0.3f);

    public static GUIStyle taskFontFormat { get; } = new GUIStyle(GUI.skin.label) {
        fontSize = 9,
        alignment = TextAnchor.LowerCenter,
        wordWrap = true,
        padding = new RectOffset(3, 3, 3, 3),
    };

    public static GUIStyle lightModeBoxStyle = new GUIStyle(GUI.skin.box) {
        // Perhaps make it better one day
    };

    public static GUIStyle darkModeBoxStyle = new GUIStyle(GUI.skin.box) {
        // Perhaps make it better one day
    };
}
