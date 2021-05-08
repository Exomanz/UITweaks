using HMUI;
using IPA.Utilities;
using SiraUtil.Tools;
using System.Threading;
using System.Threading.Tasks;
using Zenject;

namespace UITweaks.Services
{
    public class ComboColorer : IInitializable
    {
        //..ctor
        PluginConfig.ComboConfig _config;
        ComboUIController _comboUIController;
        SiraLog _log;

        //GameObjects
        ImageView[] _fcLines;

        public ComboColorer(PluginConfig.ComboConfig config, ComboUIController comboUIController, SiraLog log) 
        {
            _config = config;
            _comboUIController = comboUIController;
            _log = log;
        }

        public async void Initialize()
        {
            await Task.Run(() => Thread.Sleep(50));

            if (_comboUIController.isActiveAndEnabled)
            {
                _log.Logger.Debug("Combo Panel Present");
                _fcLines = _comboUIController.GetComponentsInChildren<ImageView>();
                _log.Logger.Debug("Got FC Lines");

                if (_config.GradientLines)
                {
                    ReflectionUtil.SetField(_fcLines[0], "_gradient", true);
                    _fcLines[0].color0 = _config.T_GradientColor0;
                    _fcLines[0].color1 = _config.T_GradientColor1;

                    ReflectionUtil.SetField(_fcLines[1], "_gradient", true);
                    if (!_config.SeparateLineColors)
                    {
                        ReflectionUtil.SetField(_fcLines[1], "_flipGradientColors", true);
                        _fcLines[1].color0 = _config.T_GradientColor0;
                        _fcLines[1].color1 = _config.T_GradientColor1;
                    }
                    else
                    {
                        _fcLines[1].color0 = _config.B_GradientColor0;
                        _fcLines[1].color1 = _config.B_GradientColor1;
                    }
                }
                else
                {
                    _fcLines[0].color = _config.T_Color;
                    _fcLines[1].color = _config.B_Color;
                }
            }
        }
    }
}
