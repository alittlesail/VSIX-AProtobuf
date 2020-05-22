
using System.Collections.Generic;

namespace ALittle
{
    public class AProtobufMessageVarNameReference : AProtobufReferenceTemplate<AProtobufMessageVarNameElement>
    {
        public AProtobufMessageVarNameReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufMessageVarName";
        }

        public override ABnfGuessError CheckError()
        {
            if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW)
            {
                var parent = m_element.GetParent() as AProtobufMessageVarElement;
                if (parent == null) return null;

                var all_type = parent.GetAllType();
                if (all_type == null) return null;

                var custom_type = all_type.GetCustomType();
                if (custom_type == null) return null;
                var id_list = custom_type.GetIdList();
                if (id_list.Count == 1 && id_list[0].GetElementText() == "EMsgErrorCode")
                {
                    var text = m_element.GetElementText();
                    if (text != "ret_code")
                        return new ABnfGuessError(m_element, "EMsgErrorCode对应的名称，请使用ret_code");
                }
            }

            return null;
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

