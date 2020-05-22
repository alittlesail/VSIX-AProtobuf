
using System.Collections.Generic;

namespace ALittle
{
    public class AProtobufEnumVarNameReference : AProtobufReferenceTemplate<AProtobufEnumVarNameElement>
    {
        public AProtobufEnumVarNameReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW)
            {
                var parent = m_element.GetParent();
                if (parent == null) return null;
                parent = parent.GetParent();
                if (parent == null) return null;
                parent = parent.GetParent();
                if (parent == null) return null;

                var enum_dec = parent as AProtobufEnumElement;
                if (enum_dec == null) return null;

                var enum_name_dec = enum_dec.GetEnumName();
                if (enum_name_dec == null) return null;

                var name = m_element.GetElementText();

                if (enum_name_dec.GetElementText() == "EMsgTypes")
                {
                    if (!name.StartsWith("_"))
                        return new ABnfGuessError(m_element, "EMsgTypes中的枚举项名请使用_开头");
                }
                else
                {
                    if (!name.StartsWith("e"))
                        return new ABnfGuessError(m_element, "枚举项名请使用e开头");
                }
            }
            return null;
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufEnumVarName";
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            AProtobufFile file = m_file as AProtobufFile;
            var name_set = file.GetNameSet();
            var value = m_element.GetElementText();

            foreach (var name in name_set)
            {
                if (value != name && name.StartsWith(value))
                    list.Add(new ALanguageCompletionInfo(name, null));
            }

            return true;
        }
    }
}

