using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class ColorFader
    {
        private const float FadeDuration = 0.8f;

        public Color CurrentColor { get; private set; }

        private float _fadeTime;
        private readonly Color _startColor;
        private readonly Color _endColor;

        public ColorFader(Color startColor, Color endColor)
        {
            _startColor = startColor;
            _endColor = endColor;
        }

        public void Update(bool isReset)
        {
            if (isReset)
            {
                _fadeTime = FadeDuration;
            }
            else
            {
                _fadeTime -= Time.deltaTime;
                _fadeTime = Mathf.Max(_fadeTime, 0);
            }

            CurrentColor = Color.Lerp(
                _startColor,
                _endColor,
                _fadeTime / FadeDuration
            );
        }
    }
}
