using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.BuilderIO.Core;

public static class ModuleConstants
{
    public static class Security
    {
        public static class Permissions
        {
            public const string Access = "builderio:access";

            public static string[] AllPermissions { get; } =
            {
                Access,
            };
        }
    }

    public static class Settings
    {
        public static class General
        {
            public static SettingDescriptor Enable { get; } = new()
            {
                Name = "BuilderIO.Enable",
                GroupName = "BuilderIO",
                ValueType = SettingValueType.Boolean,
                IsPublic = true,
                DefaultValue = false,
            };

            public static SettingDescriptor PublicApiKey { get; } = new()
            {
                Name = "BuilderIO.PublicApiKey",
                GroupName = "BuilderIO",
                ValueType = SettingValueType.ShortText,
                IsPublic = true,
                DefaultValue = string.Empty,
            };

            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return Enable;
                    yield return PublicApiKey;
                }
            }
        }

        public static IEnumerable<SettingDescriptor> AllSettings
        {
            get
            {
                return General.AllGeneralSettings;
            }
        }

        public static IEnumerable<SettingDescriptor> StoreLevelSettings
        {
            get
            {
                yield return General.Enable;
                yield return General.PublicApiKey;
            }
        }
    }
}
