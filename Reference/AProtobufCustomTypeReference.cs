
using System.Collections.Generic;

namespace ALittle
{
    public class AProtobufCustomTypeReference : AProtobufReferenceTemplate<AProtobufCustomTypeElement>
    {
        public AProtobufCustomTypeReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufCustomName";
        }

        public override string QueryQuickInfo()
        {
            if (m_project == null)
                return "将当前文件放入工程，会全工程范围提示";

            var id_child_list = m_element.GetIdList();
            if (id_child_list.Count == 0) return null;

            var name = id_child_list[id_child_list.Count - 1].GetElementText();
            id_child_list.RemoveAt(id_child_list.Count - 1);
            List<string> package_list = new List<string>();
            foreach (var id_child in id_child_list)
                package_list.Add(id_child.GetElementText());
            var package = string.Join(".", package_list);

            if (package == "" && m_file != null) package = m_file.GetPackage();

            // 先到指定包名查找
            var find_element = m_project.FindElement(package, name);
            if (find_element == null)
                find_element = m_project.FindElement("", name);
            if (find_element == null) return null;

            var info = find_element.GetElementText();
            char[] split_char = new char[1];
            split_char[0] = '\n';
            var split = info.Split(split_char);
            if (split.Length > 10)
                info = string.Join("\n", split, 0, 10) + "\n...";
            return info;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var project = m_element.GetFile().GetProjectInfo() as AProtobufProjectInfo;
            if (project == null)
            {
                list.Add(new ALanguageCompletionInfo("将当前文件放入工程，会全工程范围提示", null));
                return true;
            }

            AProtobufFile file = m_element.GetFile() as AProtobufFile;
            var analysis_text = m_element.GetElementText();
            var length = offset + 1 - m_element.GetStart();
            analysis_text = analysis_text.Substring(0, length);
            var split_text = analysis_text.Split('.');
            var id_child_list = new List<string>();
            foreach (var v in split_text)
                id_child_list.Add(v);
                
            var input = "";
            string package = "";
            if (id_child_list.Count > 0)
            {
                input = id_child_list[id_child_list.Count - 1];
                id_child_list.RemoveAt(id_child_list.Count - 1);
                List<string> name_list = new List<string>();
                foreach (var id_child in id_child_list)
                {
                    name_list.Add(id_child);
                }   
                package = string.Join(".", name_list);
            }

            var input_package = package;
            if (package == "") package = file.GetPackage();

            List<string> match_name_list = null;

            var message_set = new HashSet<string>();
            var enum_set = new HashSet<string>();

            // 如果父节点是MessageOption
            if (m_element.GetParent() is AProtobufMessageOptionElement)
            {
                match_name_list = project.MatchExtendList(package, input);
                foreach (var name_value in match_name_list)
                    list.Add(new ALanguageCompletionInfo(name_value, null));

                if (input_package.Length == 0)
                {
                    match_name_list = project.MatchExtendList("", input);
                    foreach (var name_value in match_name_list)
                        list.Add(new ALanguageCompletionInfo(name_value, null));
                }
            }
            else
            {
                match_name_list = project.MatchMessageList(package, input);
                foreach (var name_value in match_name_list)
                {
                    if (message_set.Contains(name_value)) continue;
                    message_set.Add(name_value);
                    list.Add(new ALanguageCompletionInfo(name_value, AProtobufFactoryClass.inst.sMessageIcon));
                }

                if (input_package.Length == 0)
                {
                    match_name_list = project.MatchMessageList(input, "");
                    foreach (var name_value in match_name_list)
                    {
                        if (message_set.Contains(name_value)) continue;
                        message_set.Add(name_value);
                        list.Add(new ALanguageCompletionInfo(name_value, AProtobufFactoryClass.inst.sMessageIcon));
                    }

                    match_name_list = project.MatchMessageList("", input);
                    foreach (var name_value in match_name_list)
                    {
                        if (message_set.Contains(name_value)) continue;
                        message_set.Add(name_value);
                        list.Add(new ALanguageCompletionInfo(name_value, AProtobufFactoryClass.inst.sMessageIcon));
                    }
                }

                match_name_list = project.MatchEnumList(package, input);
                foreach (var name_value in match_name_list)
                {
                    if (enum_set.Contains(name_value)) continue;
                    enum_set.Add(name_value);
                    list.Add(new ALanguageCompletionInfo(name_value, AProtobufFactoryClass.inst.sEnumIcon));
                }

                if (input_package.Length == 0)
                {
                    match_name_list = project.MatchEnumList("", input);
                    foreach (var name_value in match_name_list)
                    {
                        if (enum_set.Contains(name_value)) continue;
                        enum_set.Add(name_value);
                        list.Add(new ALanguageCompletionInfo(name_value, AProtobufFactoryClass.inst.sEnumIcon));
                    }
                }
            }

            // 根据当前输入来查找包名包名
            var package_list = project.MatchPackageList(analysis_text.Trim());
            foreach (var name in package_list)
            {
                if (input_package.Length > 0)
                {
                    var new_name = name.Substring(input_package.Length);
                    if (new_name.Length > 0 && new_name[0] == '.')
                        new_name = new_name.Substring(1);
                    list.Add(new ALanguageCompletionInfo(new_name, AProtobufFactoryClass.inst.sPackageIcon));
                }
                else
                {
                    list.Add(new ALanguageCompletionInfo(name, AProtobufFactoryClass.inst.sPackageIcon));
                }
            }

            return true;
        }

        public override bool PeekHighlightWord()
        {
            return true;
        }

        public override void QueryHighlightWordTag(List<ALanguageHighlightWordInfo> list)
        {
            AProtobufFile file = m_element.GetFile() as AProtobufFile;

            var value = m_element.GetElementText();

            HashSet<AProtobufIdElement> set;
            file.m_index.TryGetValue(value, out set);
            if (set == null) return;

            foreach (var element in set)
            {
                var info = new ALanguageHighlightWordInfo();
                info.start = element.GetStart();
                info.end = element.GetEnd();
                list.Add(info);
            }
        }

        public override ABnfElement GotoDefinition()
        {
            var file = m_element.GetFile() as AProtobufFile;

            var project = m_element.GetFile().GetProjectInfo() as AProtobufProjectInfo;
            if (project == null) return null;

            var id_child_list = m_element.GetIdList();
            if (id_child_list.Count == 0) return null;

            var name = id_child_list[id_child_list.Count - 1].GetElementText();
            id_child_list.RemoveAt(id_child_list.Count - 1);
            List<string> package_list = new List<string>();
            foreach (var id_child in id_child_list)
                package_list.Add(id_child.GetElementText());
            var package = string.Join(".", package_list);

            if (package == "" && file != null) package = file.GetPackage();

            ABnfElement out_element;
            if (m_element.GetParent() is AProtobufMessageOptionElement)
            {
                out_element = project.FindExtendElement(package, name);
                if (out_element == null)
                {
                    // 再到全局包名查找
                    out_element = project.FindExtendElement("", name);
                }
            }
            else
            {
                // 先到指定包名查找
                out_element = project.FindElement(package, name);
                if (out_element == null)
                {
                    // 再到全局包名查找
                    out_element = project.FindElement("", name);
                }
            }

            if (out_element != null)
            {
                if (out_element is AProtobufMessageElement)
                    return (out_element as AProtobufMessageElement).GetMessageName();
                else if (out_element is AProtobufEnumElement)
                    return (out_element as AProtobufEnumElement).GetEnumName();
                return out_element;
            }

            return null;
        }
    }
}

