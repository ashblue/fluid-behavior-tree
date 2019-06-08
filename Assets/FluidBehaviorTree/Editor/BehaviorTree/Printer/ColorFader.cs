using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class ColorFader {
        const float FADE_DURATION = 0.5f;

        private float _activeColorFadeTime;
        private readonly Color _activeColor = new Color(0.38f, 1f, 0.4f);
        private readonly Color _defaultColor = Color.white;

        public Color CurrentColor { get; private set; }

        public void Update (bool reset) {
            if (reset) {
                _activeColorFadeTime = FADE_DURATION;
            } else {
                _activeColorFadeTime -= Time.deltaTime;
                _activeColorFadeTime = Mathf.Max(_activeColorFadeTime, 0);
            }

            CurrentColor = Color.Lerp(
                _defaultColor, 
                _activeColor, 
                _activeColorFadeTime / FADE_DURATION);
        }
    }
}