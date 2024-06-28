namespace UITweaks.Utilities.SettableSettings
{
#if HECK
    using Heck.SettingsSetter;
    using System;
    using System.Reflection;
    using UnityEngine;
    public class UITweaksSettingsWrapper : ISettableSetting
    {
        private readonly PropertyInfo settingsProperty;
        private readonly object settingsInstance;

        private object originalValue;

        public string GroupName { get; }
        public string FieldName { get; }

        public UITweaksSettingsWrapper(string groupName, string fieldName, PropertyInfo settingsProperty, object settingsInstance)
        {
            GroupName = groupName;
            FieldName = fieldName;

            this.settingsProperty = settingsProperty;
            this.settingsInstance = settingsInstance;
        }

        public object TrueValue => settingsProperty.GetValue(settingsInstance);

        public void SetTemporary(object tempValue)
        {
            if (tempValue != null)
            {
                originalValue = settingsProperty.GetValue(settingsInstance);
                settingsProperty.SetValue(settingsInstance, tempValue);
            }

            else if (originalValue != null)
            {
                settingsProperty.SetValue(settingsInstance, originalValue);
                originalValue = null;
            }
        }
    }
#endif
}