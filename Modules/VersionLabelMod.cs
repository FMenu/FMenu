using System;
using ExtensionMethods;
using FMenu.UI;
using FMenu.Utils;
using UnityEngine;
using Logger = FMenu.Utils.Logger;
using Object = UnityEngine.Object;

namespace FMenu.Modules
{
    public class VersionLabelMod
    {

        private VersionLabel _version { get; set; }
        private TMPro.TextMeshProUGUI _versionLabel { get; set; }
        private Color _color { get; set; }
        private Timer _timer { get; set; }
        private float _hue { get; set; }

        public bool RainbowEnabled { get; set; }

        public VersionLabelMod()
        {
            this._version = null;
            this._versionLabel = null;
            this._color = new Color(0xff, 0xff, 0xff);
            this._timer = new Timer(0.2f);
            this._hue = 0f;
            this.RainbowEnabled = false;
        }

        public void Update(MenuSettings settings)
        {

            if (_timer.isCompleted() && this.RainbowEnabled)
            {
                _hue += 1f*Time.deltaTime;
                if (_hue > 1f) _hue -= 1f;
                _color = Color.HSVToRGB(_hue, 0.5f, 0.5f);
            }

            if (!this.RainbowEnabled) _color = Color.HSVToRGB(1f, 0f, 1f);

            if (_version == null) _version = Object.FindObjectOfType<VersionLabel>();
            if (_version != null && _versionLabel == null) _versionLabel = _version.GetComponent<TMPro.TextMeshProUGUI>();
            _versionLabel.text = $"FMenu v{settings.MenuVersion} | v{settings.GameVersion}";
            _versionLabel.color = _color;
        }
    }
}