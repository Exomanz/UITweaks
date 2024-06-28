namespace UITweaks.Utilities.SettableSettings
{
#if HECK
    using Heck.SettingsSetter;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UITweaks.Models;

    // Shoutout to Caeden117's Counters+ for providing a good example on how to register SettableSettings to Heck
    internal class UITweaksSettableSettings : IDisposable
    {
        public static bool hasRunBefore { get; private set; } = false;

        private const string groupIdentifier = "_uiTweaks";

        private readonly BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        private static List<ISettableSetting> settableSettings = new List<ISettableSetting>();

        public UITweaksSettableSettings(List<ConfigBase> configs)
        {
            if (hasRunBefore) return;
            hasRunBefore = true;

            Type settableSettingsType = typeof(UITweaksSettingsWrapper);

            // Remove MiscConfig since it's Enabled property is ignored.
            configs.Remove(configs.Last());

            foreach (var conf in configs)
            {
                var configType = conf.GetType();
                var normalizedTypeName = configType.BaseType.Name;
                var enabledProperty = configType.GetProperty("Enabled", bindingFlags);

                ISettableSetting setting = Activator.CreateInstance(settableSettingsType,
                    $"UITweaks - {normalizedTypeName}", enabledProperty.Name, enabledProperty, conf)
                    as ISettableSetting;

                settableSettings.Add(setting);

                int indexOfConfig = normalizedTypeName.IndexOf("Config");
                string heckFieldName = $"_{normalizedTypeName.Remove(indexOfConfig).ToLowerInvariant()}{enabledProperty.Name}";
                SettingSetterSettableSettingsManager.RegisterSettableSetting(groupIdentifier, heckFieldName, setting);
            }
        }

        public void Dispose()
        {
            settableSettings.ForEach(x => x.SetTemporary(null));
        }
    }
#endif
}