

namespace ALittle
{
    public class AProtobufEnumNameReference : AProtobufReferenceTemplate<AProtobufEnumNameElement>
    {
        public AProtobufEnumNameReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW)
            {
                var name = m_element.GetElementText();
                if (!name.StartsWith("E"))
                    return new ABnfGuessError(m_element, "枚举类型请使用E开头");
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

