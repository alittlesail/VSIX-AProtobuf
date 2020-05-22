
namespace ALittle
{
    public class AProtobufPrimitiveTypeReference : AProtobufReferenceTemplate<AProtobufPrimitiveTypeElement>
    {
        public AProtobufPrimitiveTypeReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW)
            {
                var text = m_element.GetElementText();
                if (text == "uint32" || text == "uint64")
                    return new ABnfGuessError(m_element, "不能使用无符号类型");
            }

            return null;
        }
    }
}

