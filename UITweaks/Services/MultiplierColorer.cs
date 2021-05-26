using HMUI;
using SiraUtil.Tools;
using UnityEngine;
using Zenject;

namespace UITweaks.Services
{
    public class MultiplierColorer : IInitializable, ITickable
    {
        PluginConfig.MultiplierConfig _config;
        ScoreMultiplierUIController _ui;
        SiraLog _log;
        
        //GameObjects
        CurvedTextMeshPro[] _mainTextGOs;
        ImageView _bg;
        ImageView _fg;

        public MultiplierColorer(PluginConfig.MultiplierConfig config,
            ScoreMultiplierUIController ui, SiraLog log)
        {
            _config = config;
            _ui = ui;
            _log = log;
        }

        public void Initialize()
        {
            _mainTextGOs = _ui.transform.GetComponentsInChildren<CurvedTextMeshPro>();
            _bg = _ui.transform.Find("BGCircle").GetComponent<ImageView>();
            _fg = _ui.transform.Find("FGCircle").GetComponent<ImageView>();

            _log.Logger.Debug("Multiplier Ring Image And Text Got");
        }

        public void Tick()
        {
            if (_ui.isActiveAndEnabled) //Counters+ Hide Multiplier Condition :KEKW:
                switch (_mainTextGOs[1].text)
                {
                    case "1":
                        _bg.color = _config.Color1.ColorWithAlpha(0.25f);
                        _fg.color = _config.Color1;
                        break;
                    case "2":
                        _bg.color = _config.Color2.ColorWithAlpha(0.25f);
                        _fg.color = _config.Color2;
                        break;
                    case "4":
                        _bg.color = _config.Color4.ColorWithAlpha(0.25f);
                        _fg.color = _config.Color4;
                        break;
                    case "8":
                        if (_config.RainbowAnim)
                            _bg.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong
                                (Time.time * .5f, 1), 1, 1));

                        else _bg.color = _config.Color8.ColorWithAlpha(0.25f);
                        //_fg.color = _config.Color8; <= Doesn't ever actually show in-game
                        break;
                }
        }
    }
}
