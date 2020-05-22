
using System.Collections.Generic;

namespace ALittle
{
    public class AProtobufPackageNameReference : AProtobufReferenceTemplate<AProtobufPackageNameElement>
    {
        public AProtobufPackageNameReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufCustomName";
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var project = m_file.GetProjectInfo() as AProtobufProjectInfo;
            if (project == null)
            {
                list.Add(new ALanguageCompletionInfo("将当前文件放入工程，会全工程范围提示", null));
                return true;
            }

            var value = m_element.GetElementText();
            var package_list = project.MatchPackageList(value);
            foreach (var name in package_list)
                list.Add(new ALanguageCompletionInfo(name, AProtobufFactoryClass.inst.sPackageIcon));

            return true;
        }
    }
}
