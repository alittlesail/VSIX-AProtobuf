

namespace ALittle
{
    public class AProtobufMessageNameReference : AProtobufReferenceTemplate<AProtobufMessageNameElement>
    {
        public AProtobufMessageNameReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            var project = m_project;
            if (project == null) return null;

            if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW)
            {
                var name = m_element.GetElementText();
                if (name.StartsWith("MSG_"))
                {
                    var element_info = project.FindEnumElementInfo("", "EMsgTypes");
                    if (element_info == null)
                        return new ABnfGuessError(m_element, "没有在全局定义enum EMsgTypes");

                    var enum_var_name = "_" + name;
                    if (!element_info.name_map.ContainsKey(enum_var_name))
                        return new ABnfGuessError(m_element, "在EMsgTypes中没有定义" + enum_var_name);
                }
            }

            return null;
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufCustomName";
        }
    }
}

