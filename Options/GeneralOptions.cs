using Microsoft.VisualStudio.Settings;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ALittle
{
    internal class GeneralOptions : BaseOptionModel<GeneralOptions>
    {
        [Category("定制功能")]
        [DisplayName("项目组")]
        [Description("选择您所在的项目组，插件可以根据不同的项目组开启不同的功能")]
        [DefaultValue(ProjectTeamTypes.None)]
        [TypeConverter(typeof(EnumConverter))]
        public ProjectTeamTypes ProjectTeam { get; set; }

        protected override void LoadProperty(SettingsStore store)
        {
            var value = store.GetString(CollectionName, nameof(ProjectTeam), "None");
            if (value == "LW") ProjectTeam = ProjectTeamTypes.LW;
            else ProjectTeam = ProjectTeamTypes.None;
        }
        protected override void SaveProperty(WritableSettingsStore store)
        {
            if (ProjectTeam == ProjectTeamTypes.LW)
                store.SetString(CollectionName, nameof(ProjectTeam), "LW");
            else
                store.SetString(CollectionName, nameof(ProjectTeam), "None");
        }
    }

    public enum ProjectTeamTypes
    {
        None,
        LW
    }
}
