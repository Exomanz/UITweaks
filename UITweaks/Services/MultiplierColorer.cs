using HMUI;
using UnityEngine;
using Zenject;

namespace UITweaks.Services
{
    public class MultiplierColorer : IInitializable, ITickable
    {
        PluginConfig.MultiplierConfig _config;
        ScoreMultiplierUIController _ui;
        CurvedTextMeshPro[] _mainTextGO;
        ImageView _bg;
        ImageView _fg;

        public MultiplierColorer(PluginConfig.MultiplierConfig config, ScoreMultiplierUIController scoreMultiplierUIController)
        {
            _ui = scoreMultiplierUIController;
            _config = config;
        }

        public void Initialize()
        {
            var text = _ui.transform.Find("TextPanel").GetComponentsInChildren<CurvedTextMeshPro>();
            var bg = _ui.transform.Find("BGCircle").GetComponent<ImageView>();
            var fg = _ui.transform.Find("FGCircle").GetComponent<ImageView>();

            _mainTextGO = text;
            _bg = bg;
            _fg = fg;
        }

        public void Tick()
        {
            switch (_mainTextGO[1].text)
            {
                case "1":
                    _bg.color = new Color(_config.IColor.r, _config.IColor.g, _config.IColor.b, .25f);
                    _fg.color = _config.IColor;
                    break;
                case "2":
                    _bg.color = new Color(_config.IIColor.r, _config.IIColor.g, _config.IIColor.b, .25f);
                    _fg.color = _config.IIColor;
                    break;
                case "4":
                    _bg.color = new Color(_config.IVColor.r, _config.IVColor.g, _config.IVColor.b, .25f);
                    _fg.color = _config.IVColor;
                    break;
                case "8":
                    if (_config.RainbowAnimOnIIX)
                        _bg.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * .5f, 1), 1, 1));

                    else _bg.color = new Color(_config.IIXColor.r, _config.IIXColor.g, _config.IIXColor.b, .25f);
                    _fg.color = _config.IIXColor;
                    break;
            }
        }
    }
}
