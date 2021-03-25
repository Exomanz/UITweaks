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

        public MultiplierColorer(PluginConfig.MultiplierConfig config,
            ScoreMultiplierUIController ui)
        {
            _config = config;
            _ui = ui;
        }

        public void Initialize()
        {
            var textGOs = _ui.transform.GetComponentsInChildren<CurvedTextMeshPro>();
            var bg = _ui.transform.Find("BGCircle").GetComponent<ImageView>();
            var fg = _ui.transform.Find("FGCircle").GetComponent<ImageView>();

            _mainTextGO = textGOs;
            _bg = bg;
            _fg = fg;
        }

        public void Tick()
        {
            switch (_mainTextGO[1].text)
            {
                case "1":
                    _bg.color = new Color(_config.Color1.r, _config.Color1.g, _config.Color1.b, .25f);
                    _fg.color = _config.Color1;
                    break;
                case "2":
                    _bg.color = new Color(_config.Color2.r, _config.Color2.g, _config.Color2.b, .25f);
                    _fg.color = _config.Color2;
                    break;
                case "4":
                    _bg.color = new Color(_config.Color4.r, _config.Color4.g, _config.Color4.b, .25f);
                    _fg.color = _config.Color4;
                    break;
                case "8":
                    if (_config.RainbowAnimOn8)
                        _bg.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * .5f, 1), 1, 1));

                    else _bg.color = new Color(_config.Color8.r, _config.Color8.g, _config.Color8.b, .25f);
                    //_fg.color = _config.Color8; <= Doesn't ever actually show in-game
                    break;
            }
        }
    }
}
