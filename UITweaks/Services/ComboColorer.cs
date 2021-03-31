using HMUI;
using IPA.Utilities;
using Zenject;

namespace UITweaks.Services
{
    public class ComboColorer : IInitializable
    {
        PluginConfig.ComboConfig _config;

        ImageView[] _fcLines;
        ComboUIController _comboUIController;

        public ComboColorer(PluginConfig.ComboConfig config, ComboUIController comboUIController) 
        {
            _config = config;
            _comboUIController = comboUIController;
        }

        public void Initialize()
        { 
            var comboFCLines = _comboUIController.GetComponentsInChildren<ImageView>();
            _fcLines = comboFCLines;

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
