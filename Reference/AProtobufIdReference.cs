
using System.Collections.Generic;

namespace ALittle
{
    public class AProtobufIdReference : AProtobufReferenceTemplate<AProtobufIdElement>
    {
        public AProtobufIdReference(ABnfElement element) : base(element) { }
        
        public override string QueryQuickInfo()
        {
            var custom_type = m_element.GetParent() as AProtobufCustomTypeElement;
            if (custom_type != null)
            {
                return custom_type.GetReference().QueryQuickInfo();
            }

            return null;
        }

        public override bool PeekHighlightWord()
        {
            return m_element.GetParent() is AProtobufCustomTypeElement;
        }

        public override void QueryHighlightWordTag(List<ALanguageHighlightWordInfo> list)
        {
            var custom_type = m_element.GetParent() as AProtobufCustomTypeElement;
            if (custom_type != null)
            {
                custom_type.GetReference().QueryHighlightWordTag(list);
                return;
            }
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var package_split_name = m_element.GetParent() as AProtobufPackageSplitNameElement;
            if (package_split_name != null)
			{
                var package_name = package_split_name.GetParent() as AProtobufPackageNameElement;
                if (package_name != null)
                    return package_name.GetReference().QueryCompletion(offset, list);
            }   

            var message_name = m_element.GetParent() as AProtobufMessageNameElement;
            if (message_name != null)
                return message_name.GetReference().QueryCompletion(offset, list);

            var enum_name = m_element.GetParent() as AProtobufEnumNameElement;
            if (enum_name != null)
                return enum_name.GetReference().QueryCompletion(offset, list);

            var custom_type = m_element.GetParent() as AProtobufCustomTypeElement;
            if (custom_type != null)
                return custom_type.GetReference().QueryCompletion(offset, list);

            var message_var_name = m_element.GetParent() as AProtobufMessageVarNameElement;
            if (message_var_name != null)
                return message_var_name.GetReference().QueryCompletion(offset, list);

            var enum_var_name = m_element.GetParent() as AProtobufEnumVarNameElement;
            if (enum_var_name != null)
                return enum_var_name.GetReference().QueryCompletion(offset, list);

            return true;
        }

        public override ABnfElement GotoDefinition()
        {
            var custom_type = m_element.GetParent() as AProtobufCustomTypeElement;
            if (custom_type != null)
                return custom_type.GetReference().GotoDefinition();

            return null;
        }
    }
}

