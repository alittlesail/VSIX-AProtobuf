
using System.Collections.Generic;
using System.IO;

namespace ALittle
{
    public class AProtobufImportReference : AProtobufReferenceTemplate<AProtobufImportElement>
    {
        public AProtobufImportReference(ABnfElement element) : base(element) { }
        
        public override ABnfGuessError CheckError()
        {
            ABnfElement child = m_element.GetText();
            if (child == null) return null;

            var project = m_file.GetProjectInfo() as AProtobufProjectInfo;
            if (project == null) return null;

            var value = child.GetElementString();

            var full_path = project.GetProjectPath() + value;
            if (File.Exists(full_path)) return null;

            var file = project.FindImportFile(value);
            if (file != null)
            {
                full_path = file.GetFullPath();
                if (File.Exists(full_path)) return null;
            }

            return new ABnfGuessError(child, "Import的文件不存在");
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var text_element = m_element.GetText();
            if (text_element == null) return false;

            var analysis_text = text_element.GetElementString();
            int length = offset + 1 - text_element.GetStart() - 1;
            analysis_text = analysis_text.Substring(0, length);

            var project = m_file.GetProjectInfo() as AProtobufProjectInfo;
            if (project == null) return false;

            var pre_text = text_element.GetElementText().Substring(0, offset - text_element.GetStart());

            var query_list = project.QueryFilePath(analysis_text);
            foreach (var value in query_list)
            {
                if (value.EndsWith(".proto"))
                    list.Add(new ALanguageCompletionInfo(value, AProtobufFactoryClass.inst.sFileIcon, value, pre_text));
                else
                    list.Add(new ALanguageCompletionInfo(value, AProtobufFactoryClass.inst.sFolderIcon, value, pre_text));
            }

            return true;
        }
    }
}

