using HMUI;
using SiraUtil.Tools;
using TMPro;
using UnityEngine;
using Zenject;

namespace UITweaks.Services
{
    public class PositionColorer : IInitializable, ITickable
    {
        //..ctor stuff
        MultiplayerPositionHUDController _pos;
        PluginConfig.PositionConfig _config;
        SiraLog _log;

        //GameObjects
        TextMeshProUGUI[] _positionGOs;
        CurvedTextMeshPro _firstAnimGO;

        public PositionColorer(MultiplayerPositionHUDController pos, PluginConfig.PositionConfig config, SiraLog log)
        {
            _config = config;
            _log = log;
            _pos = pos;
        }

        public void Initialize()
        {
            _positionGOs = _pos.transform.GetComponentsInChildren<TextMeshProUGUI>();
            _log.Logger.Debug("Position Texts Received!");

            _firstAnimGO = _pos.transform.Find("DynamicPanel/1stPosition").gameObject.GetComponent<CurvedTextMeshPro>();
            _log.Logger.Debug("1st Place Animation Receieved!");

            if (_config.HideFirstPlaceAnimation) _firstAnimGO.enabled = false;
        }

        public void Tick()
        {
            switch (_positionGOs[1].text)
            {
                case "5":
                    _positionGOs[1].color = _config.Fifth;
                    break;
                case "4":
                    _positionGOs[1].color = _config.Fourth;
                    break;
                case "3":
                    _positionGOs[1].color = _config.Third;
                    break;
                case "2":
                    _positionGOs[1].color = _config.Second;
                    break;
                case "1":
                    _positionGOs[1].color = _config.First;
                    _firstAnimGO.color = _config.First;
                    break;
            }

            _positionGOs[0].color = _positionGOs[1].color.ColorWithAlpha(0.25f);
        }
    }
}
