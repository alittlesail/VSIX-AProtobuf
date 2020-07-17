
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ALittle
{
    // 对象工厂
    public class AProtobufFactoryClass : AProtobufFactory
    {
        public static AProtobufFactoryClass inst = new AProtobufFactoryClass();

        protected HashSet<string> m_int_set = new HashSet<string>();

        // 图标
        public ImageSource sFileIcon;
        public ImageSource sFolderIcon;
        public ImageSource sPackageIcon;
        public ImageSource sMessageIcon;
        public ImageSource sEnumIcon;

        public AProtobufFactoryClass()
        {
            m_int_set.Add("int32");
            m_int_set.Add("uint32");
            m_int_set.Add("int64");
            m_int_set.Add("uint64");
            m_int_set.Add("sint32");
            m_int_set.Add("sint64");
            m_int_set.Add("fixed32");
            m_int_set.Add("fixed64");
            m_int_set.Add("sfixed32");
            m_int_set.Add("sfixed64");
        }

        public override ABnfReference CreateReference(ABnfElement element)
        {
            if (element is AProtobufLineCommentElement) return new AProtobufLineCommentReference(element);
            if (element is AProtobufBlockCommentElement) return new AProtobufBlockCommentReference(element);
            if (element is AProtobufCustomTypeElement) return new AProtobufCustomTypeReference(element);
            if (element is AProtobufEnumElement) return new AProtobufEnumReference(element);
            if (element is AProtobufEnumBodyElement) return new AProtobufEnumBodyReference(element);
            if (element is AProtobufEnumNameElement) return new AProtobufEnumNameReference(element);
            if (element is AProtobufEnumVarNameElement) return new AProtobufEnumVarNameReference(element);
            if (element is AProtobufIdElement) return new AProtobufIdReference(element);
            if (element is AProtobufImportElement) return new AProtobufImportReference(element);
            if (element is AProtobufOneofElement) return new AProtobufOneofReference(element);
            if (element is AProtobufOneofNameElement) return new AProtobufOneofNameReference(element);
            if (element is AProtobufMessageElement) return new AProtobufMessageReference(element);
            if (element is AProtobufMessageNameElement) return new AProtobufMessageNameReference(element);
            if (element is AProtobufMessageBodyElement) return new AProtobufMessageBodyReference(element);
            if (element is AProtobufMessageOptionElement) return new AProtobufMessageOptionReference(element);
            if (element is AProtobufMessageVarNameElement) return new AProtobufMessageVarNameReference(element);
            if (element is AProtobufNumberElement) return new AProtobufNumberReference(element);
            if (element is AProtobufPackageNameElement) return new AProtobufPackageNameReference(element);
            if (element is AProtobufPrimitiveTypeElement) return new AProtobufPrimitiveTypeReference(element);
            if (element is AProtobufKeyElement) return new AProtobufKeyReference(element);
            if (element is AProtobufSyntaxElement) return new AProtobufSyntaxReference(element);
            if (element is AProtobufTextElement) return new AProtobufTextReference(element);
            if (element is AProtobufRegexElement) return new AProtobufRegexReference(element);

            return new AProtobufReferenceTemplate<ABnfElement>(element);
        }

        public bool IsInt(string value) { return m_int_set.Contains(value); }

        public override void MainThreadInit()
        {
            sEnumIcon = ALanguageUtility.ToImageSource(Properties.Resources.EnumIcon);
            sMessageIcon = ALanguageUtility.ToImageSource(Properties.Resources.MessageIcon);
            sPackageIcon = ALanguageUtility.ToImageSource(Properties.Resources.PackageIcon);
            sFolderIcon = ALanguageUtility.ToImageSource(Properties.Resources.FolderIcon);
            sFileIcon = ALanguageUtility.ToImageSource(Properties.Resources.FileIcon);

            // 初始化配置项
            { var _ = GeneralOptions.Instance; }
        }

        public override TextMarkerTag CreateTextMarkerTag()
        {
            return new AProtobufHighlightWordTag();
        }

        public override string GetDotExt() { return ".proto"; }

        public override byte[] LoadABnf() { return Properties.Resources.AProtobuf; }

        public void GotoEMsgTypes(ALanguageServer server, string project_path, string full_path, int offset)
        {
            var view_item = server.GetView(full_path);
            if (view_item == null) return;

            var project_info = server.GetProject(project_path) as AProtobufProjectInfo;
            if (project_info == null) return;

            var regex_element = view_item.GetException(offset) as AProtobufRegexElement;
            if (regex_element == null) return;
            var id_element = regex_element.GetParent() as AProtobufIdElement;
            if (id_element == null) return;
            var message_name_dec = id_element.GetParent() as AProtobufMessageNameElement;
            if (message_name_dec == null) return;

            var enum_info = project_info.FindEnumElementInfo("", "EMsgTypes");
            if (enum_info == null) return;

            var enum_var_name = "_" + message_name_dec.GetElementText();
            enum_info.name_map.TryGetValue(enum_var_name, out AProtobufEnumVarElement var_dec);
            if (var_dec == null)
            {
                full_path = enum_info.element.GetFullPath();
                Application.Current.Dispatcher.Invoke(() =>
                {
					ALanguageUtility.OpenFile(m_open_document, m_adapters_factory
                                            , full_path, 0, 0);
                });
                return;
            }

            var name_dec = var_dec.GetEnumVarName();
            if (name_dec == null) return;

            full_path = name_dec.GetFullPath();
            int start = name_dec.GetStart();
            int length = name_dec.GetLength();
            Application.Current.Dispatcher.Invoke(() =>
            {
				ALanguageUtility.OpenFile(m_open_document, m_adapters_factory
                                        , full_path, start, length);
            });
        }

        public override string FastGoto(ALanguageServer server, Dictionary<string, ProjectInfo> projects, string text)
        {
            // 按::切割
            char[] sy = { ':', '.' };
            string[] split = text.Split(sy);
            var temp_split = new List<string>();
            foreach (var c in split)
            {
                if (c != "")
                    temp_split.Add(c);
            }
            if (temp_split.Count == 0) return null;

            var package = "";
            string name = temp_split[temp_split.Count - 1];
            temp_split.RemoveAt(temp_split.Count - 1);
                
            package = string.Join(".", temp_split);

			ABnfElement element = null;
            // 获取所有工程
            foreach (var pair in projects)
            {
                if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW)
                {
                    if (name.EndsWith("_struct"))
                        name = name.Substring(0, name.Length - "_struct".Length);
                    else if (name.EndsWith("_dirty"))
                        name = name.Substring(0, name.Length - "_dirty".Length);
                }

                var project = pair.Value as AProtobufProjectInfo;
                if (project == null) continue;

                if (package == "")
                {
                    element = project.FindElementInAllPackage(name);
                    if (element != null) break;
                }
                else
                {
                    element = project.FindElement(package, name);
                    if (element != null) break;
                }
            }

            // 把package当做 enum枚举名，name当做枚举项名
            if (element == null)
            {
                if (temp_split.Count == 1 || temp_split.Count == 2)
                {
                    foreach (var pair in projects)
                    {
                        var project = pair.Value as AProtobufProjectInfo;
                        if (project == null) continue;

                        if (temp_split.Count == 1)
                        {
                            element = project.FindElementInAllPackage(temp_split[0]);
                            if (element != null) break;
                        }
                        else if (temp_split.Count == 2)
                        {
                            element = project.FindElement(temp_split[0], temp_split[1]);
                            if (element != null) break;
                        }
                    }
                }
                else if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW && temp_split.Count == 0)
                {
                    if (name.EndsWith("_struct") || name.EndsWith("_dirty"))
                    {
                        var sub_name = name;
                        if (sub_name.EndsWith("_struct"))
                            sub_name = sub_name.Substring(0, sub_name.Length - "_struct".Length);
                        else if (sub_name.EndsWith("_dirty"))
                            sub_name = sub_name.Substring(0, sub_name.Length - "_dirty".Length);

                        foreach (var pair in projects)
                        {
                            var project = pair.Value as AProtobufProjectInfo;
                            if (project == null) continue;

                            element = project.FindElementInAllPackage(sub_name);
                            if (element != null) break;
                        }
                    }

                    if (element == null)
                    {
                        foreach (var pair in projects)
                        {
                            var project = pair.Value as AProtobufProjectInfo;
                            if (project == null) continue;

                            element = project.FindElementInAllPackage(name);
                            if (element != null) break;
                        }
                    }

                    if (element == null)
                    {
                        foreach (var pair in projects)
                        {
                            var project = pair.Value as AProtobufProjectInfo;
                            if (project == null) continue;

                            element = project.FindElement("", "EMsgErrorCode");
                            if (element != null) break;
                        }
                    }
                }

                if (element is AProtobufEnumElement)
                {
                    var body_dec = (element as AProtobufEnumElement).GetEnumBody();
                    if (body_dec != null)
                    {
                        foreach (var pair in body_dec.GetEnumVarList())
                        {
                            if (pair.GetEnumVarName().GetElementText() == name)
                            {
                                element = pair.GetEnumVarName();
                                break;
                            }
                        }
                    }
                }
            }

            if (element == null)
                return "找不到在包(" + package + ")的协议或枚举(" + name + ")";

            if (element is AProtobufMessageElement)
                element = (element as AProtobufMessageElement).GetMessageName();
            else if (element is AProtobufEnumElement)
                element = (element as AProtobufEnumElement).GetEnumName();

            string full_path = element.GetFullPath();
            int start = element.GetStart();
            int length = element.GetLength();
            Application.Current.Dispatcher.Invoke(() =>
            {
				ALanguageUtility.OpenFile(m_open_document, m_adapters_factory
                                        , full_path, start, length);
            });

            return null;
        }

        public override ABnfFile CreateABnfFile(string full_path, ABnf abnf, string text)
        {
            return new AProtobufFile(full_path, abnf, text);
        }

        public override FileItem CreateFileItem(ProjectInfo project, ABnf abnf, string full_path, uint item_id, ABnfFile file)
        {
            return new AProtobufFileItem(project, abnf, full_path, item_id, file);
        }

        public override ProjectInfo CreateProjectInfo(ABnfFactory factory, ABnf abnf, string path)
        {
            return new AProtobufProjectInfo(factory, abnf, path);
        }

        public override string ShowKeyWordCompletion(string input, ABnfElement pick)
        {
            if (pick is AProtobufTextElement) return null;

            if (pick is AProtobufIdElement)
                pick = pick.GetParent();

            if (pick is AProtobufMessageNameElement) return null;
            if (pick is AProtobufEnumNameElement) return null;
            if (pick is AProtobufOneofNameElement) return null;
            if (pick is AProtobufMessageVarNameElement) return null;
            if (pick is AProtobufEnumVarNameElement) return null;

            if (pick is AProtobufCustomTypeElement)
            {
                var element = pick as AProtobufCustomTypeElement;
                // 如果出现点，那么就不要显示关键字
                if (element.GetStringList().Count > 0) return null;
                return element.GetElementText();
            }
            else if (pick is AProtobufPackageNameElement)
            {
                var element = pick as AProtobufPackageNameElement;
                // 如果出现点，那么就不要显示关键字
                if (element.GetStringList().Count > 0) return null;
            }

            var node = pick as ABnfNodeElement;
            if (node == null && pick != null) node = pick.GetParent();
            if (node is AProtobufLineCommentElement) return null;
            if (node is AProtobufBlockCommentElement) return null;
            return input;
        }

		public override bool FormatViewContent(UIViewItem info)
		{
            return false;
        }
    }

    public class AProtobufReferenceTemplate<T> : ABnfReferenceTemplate<T> where T : ABnfElement
	{
        public AProtobufReferenceTemplate(ABnfElement element) : base(element) { }

        public AProtobufFile m_file { get { return m_element.GetFile() as AProtobufFile; } }
        public AProtobufProjectInfo m_project { get { return m_element.GetFile().GetProjectInfo() as AProtobufProjectInfo; } }

        public override int GetFormateIndentation(int offset, ABnfElement select)
        {
            var parent = m_element.GetParent();
            if (parent == null) return 0;
            return parent.GetReference().GetFormateIndentation(offset, select);
        }
    }
}

