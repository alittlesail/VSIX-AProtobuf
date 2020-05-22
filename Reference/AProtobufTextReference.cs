
using System.Collections.Generic;
using System.IO;

namespace ALittle
{
    public class AProtobufTextReference : AProtobufReferenceTemplate<AProtobufTextElement>
    {
        public AProtobufTextReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufText";
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var parent = m_element.GetParent();
            if (parent == null) return false;

            return parent.GetReference().QueryCompletion(offset, list);
        }

        public override ABnfElement GotoDefinition()
        {
            var project = m_file.GetProjectInfo() as AProtobufProjectInfo;
            if (project == null) return null;

            var node = m_element.GetParent();
            if (node is AProtobufImportElement)
            {
                var value = m_element.GetElementString();
                string full_path = project.GetProjectPath() + value;
                if (File.Exists(full_path)) return new ABnfPathElement(full_path);

                var file = project.FindImportFile(value);
                if (file == null) return null;
                full_path = file.GetFullPath();
                return new ABnfPathElement(full_path);
            }

            return null;
        }
    }
}
