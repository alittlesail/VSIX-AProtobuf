
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ALittle
{
    // Message类型
    public class AProtobufMessageInfo
    {
        public AProtobufMessageElement element;
        public Dictionary<string, AProtobufEnumInfo> enums = new Dictionary<string, AProtobufEnumInfo>();
        public Dictionary<string, AProtobufMessageInfo> messages = new Dictionary<string, AProtobufMessageInfo>();
    }

    // Enum类型
    public class AProtobufEnumInfo
    {
        public AProtobufEnumElement element;
        public Dictionary<string, AProtobufEnumVarElement> name_map = new Dictionary<string, AProtobufEnumVarElement>();
    }

    // 自定义类型
    public class AProtobufCustomInfo
    {
        public Dictionary<string, AProtobufMessageInfo> messages = new Dictionary<string, AProtobufMessageInfo>();
        public Dictionary<string, AProtobufEnumInfo> enums = new Dictionary<string, AProtobufEnumInfo>();
        public Dictionary<string, ABnfElement> extend_names = new Dictionary<string, ABnfElement>();
        public HashSet<int> extend_numbers = new HashSet<int>();
        public Dictionary<string, ABnfElement> oneof_names = new Dictionary<string, ABnfElement>();
        public HashSet<int> oneof_numbers = new HashSet<int>();
        public Dictionary<string, int> ref_map = new Dictionary<string, int>();
    }

    public class AProtobufFile : ABnfFileInfo
    {
        protected string m_syntax = "proto2";
        protected string m_package = "";
        protected HashSet<string> m_imports = new HashSet<string>();
        protected AProtobufCustomInfo m_custom_info = new AProtobufCustomInfo();

        // 获取规则
        internal Dictionary<string, HashSet<AProtobufIdElement>> m_index = new Dictionary<string, HashSet<AProtobufIdElement>>();
        // 字段名称集合
        HashSet<string> m_name_set = new HashSet<string>();
        // 收集所有的CustomType，用于Reference分析
        HashSet<AProtobufCustomTypeElement> m_customtype_set = new HashSet<AProtobufCustomTypeElement>();

        public AProtobufFile(string full_path, ABnf abnf, string text) : base(full_path, abnf, text)
        {
        }

        //编译部分//////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool CompileDocument()
        {
            var project_info = GetProjectInfo();
            if (project_info == null)
            {
                MessageBox.Show("请将当前文件加入到工程后再进行编译");
                return true;
            }

            string path = project_info.GetProjectPath();
            string bat_file_path = path + "BuildProtoFile.bat";
            if (!File.Exists(bat_file_path))
            {
                MessageBox.Show("请在当前工程目录下编写BuildProtoFile.bat，并且接受一个参数：文件路径");
                return true;
            }

            string file_path = GetFullPath();
            if (file_path.IndexOf(" ") >= 0) file_path = "\"" + file_path + "\"";

            ProcessStartInfo info = new ProcessStartInfo("BuildProtoFile.bat", file_path);
            info.WorkingDirectory = path;
            System.Diagnostics.Process.Start(info);
            return true;
        }

        public override bool CompileProject()
        {
            var project_info = GetProjectInfo();
            if (project_info == null)
            {
                MessageBox.Show("请将当前文件加入到工程后再进行编译");
                return true;
            }

            string path = project_info.GetProjectPath();
            string bat_file_path = path + "BuildProtoProject.bat";
            if (!File.Exists(bat_file_path))
            {
                MessageBox.Show("请在当前工程目录下编写BuildProtoProject.bat，没有参数");
                return true;
            }

            ProcessStartInfo info = new ProcessStartInfo("BuildProtoProject.bat");
            info.WorkingDirectory = path;
            System.Diagnostics.Process.Start(info);
            return true;
        }

        //格式化部分//////////////////////////////////////////////////////////////////////////////////////////////////
        public override string FormatDocument()
        {
            if (HasError())
            {
                // 提示错误
                MessageBox.Show("当前有语法错误，请修正后再格式化");
                return null;
            }

            try
            {
                // 到这里说明都没有错误，那么就开始格式化
                string buffer = "";
                var child_list = m_root.GetChilds();
                int last_line = 0;
                for (int index = 0; index < child_list.Count; ++index)
                {
                    var child = child_list[index];
                    ABnfElement next_child = null;
                    if (index + 1 < child_list.Count) next_child = child_list[index + 1];

                    if (last_line != 0 && child.GetStartLine() != last_line)
                    {
                        int line_count = child.GetStartLine() - last_line - 1;
                        for (int i = 0; i < line_count; ++i) buffer += "\n";
                    }

                    if (child.GetNodeType() == "Syntax")
                    {
                        FormatSyntax(child as AProtobufSyntaxElement, "", ref buffer);
                    }
                    else if (child.GetNodeType() == "Package")
                    {
                        FormatPackage(child as AProtobufPackageElement, "", ref buffer);
                    }
                    else if (child.GetNodeType() == "Import")
                    {
                        FormatImport(child as AProtobufImportElement, "", ref buffer);
                    }
                    else if (child.GetNodeType() == "Option")
                    {
                        FormatOption(child as AProtobufOptionElement, "", ref buffer);
                    }
                    else if (child.GetNodeType() == "Enum")
                    {
                        FormatEnum(child as AProtobufEnumElement, "", ref buffer);
                    }
                    else if (child.GetNodeType() == "Message")
                    {
                        FormatMessage(child as AProtobufMessageElement, "", ref buffer);
                    }
                    else if (child.GetNodeType() == "Extend")
                    {
                        FormatExtend(child as AProtobufExtendElement, "", ref buffer);
                    }
                    else if (child.GetNodeType() == "Service")
                    {
                        FormatService(child as AProtobufServiceElement, "", ref buffer);
                    }
                    else if (child.GetNodeType() == "LineComment")
                    {
                        
                        var value = child.GetElementText();
                        if (value.Length >= 3 && value[2] != '/')
                            buffer += "// " + value.Substring(2).Trim();
                        else
                            buffer += value;
                        buffer += "\n";
                    }
                    else if (child.GetNodeType() == "BlockComment")
                    {
                        buffer += child.GetElementText();
                        buffer += "\n";
                    }
                    else
                        throw new System.Exception("未知节点类型:" + child.GetNodeType());

                    last_line = child.GetEndLine();
                }

                return buffer;
            }
            catch (System.Exception e)
            {
                // 提示错误
                MessageBox.Show(e.Message);
            }

            return null;
        }

        public void FormatSyntax(AProtobufSyntaxElement child, string indent, ref string buffer)
        {
            var text = child.GetText();
            if (text == null) throw new System.Exception("child.GetText() == null");
            buffer += indent + "syntax = " + text.GetElementText() + ";\n";
        }
        public void FormatPackage(AProtobufPackageElement child, string indent, ref string buffer)
        {
            var name = child.GetPackageName();
            if (name == null) throw new System.Exception("child.GetPackageName() == null");
            var id_list = name.GetIdList();
            var name_list = new List<string>();
            foreach (var id in id_list) name_list.Add(id.GetElementText());
            if (name_list.Count == 0) throw new System.Exception("name_list.Count == 0");
            buffer += indent + "package " + string.Join(".", name_list) + ";\n";
        }
        public void FormatImport(AProtobufImportElement child, string indent, ref string buffer)
        {
            var text = child.GetText();
            if (text == null) throw new System.Exception("child.GetText() == null");
            buffer += indent + "import " + text.GetElementText() + ";\n";
        }
        public void FormatOption(AProtobufOptionElement child, string indent, ref string buffer)
        {
            var id = child.GetId();
            if (id == null) throw new System.Exception("child.GetId() == null");

            var value = child.GetOptionValue();
            if (value == null) throw new System.Exception("child.GetOptionValue() == null");

            buffer += indent + "option " + id.GetElementText() + " = " + value.GetElementText() + ";\n";
        }
        public void FormatEnum(AProtobufEnumElement child, string indent, ref string buffer)
        {
            var enum_name = child.GetEnumName();
            if (enum_name == null) throw new System.Exception("child.GetEnumName() == null");

            var enum_body = child.GetEnumBody();
            if (enum_body == null) throw new System.Exception("child.GetEnumBody() == null");

            int name_max = 0;
            int number_max = 0;
            var var_list = enum_body.GetEnumVarList(); 
            foreach (var variable in var_list)
            {
                var name = variable.GetEnumVarName();
                if (name == null) throw new System.Exception("variable.GetEnumVarName() == null");
                if (name_max < name.GetLength()) name_max = name.GetLength();
                var number = variable.GetNumber();
                if (number == null) throw new System.Exception("variable.GetNumber() == null");
                if (number_max < number.GetLength() + 3) number_max = number.GetLength() + 3; // 算上number前面的等号和空格，number后面的分号
            }
            // 将name_max,number_max调整为大于当前的4的倍数
            name_max += 4 - name_max % 4;
            number_max += 4 - number_max % 4;

            buffer += indent + "enum " + enum_name.GetElementText() + "\n";
            buffer += indent + "{\n";

            int last_line = 0;
            var child_list = enum_body.GetChilds();
            for (int index = 0; index < child_list.Count; ++index)
            {
                var sub_child = child_list[index];
                ABnfElement next_child = null;
                if (index + 1 < child_list.Count) next_child = child_list[index + 1];

                if (last_line != 0)
                {
                    int line_count = sub_child.GetStartLine() - last_line - 1;
                    for (int i = 0; i < line_count; ++i) buffer += "\n";
                }

                if (sub_child is AProtobufEnumVarElement)
                {
                    var sub_element = sub_child as AProtobufEnumVarElement;
                    var name = sub_element.GetEnumVarName();
                    if (name == null) throw new System.Exception("sub_element.GetEnumVarName() == null");
                    string name_value = name.GetElementText();
                    var number = sub_element.GetNumber();
                    if (number == null) throw new System.Exception("sub_element.GetNumber() == null");
                    string number_value = number.GetElementText();

                    buffer += indent + "    " + name_value;
                    for (int i = name_value.Length; i < name_max; ++i) buffer += " ";
                    number_value = "= " + number_value + ";";
                    buffer += number_value;
                    
                    if (next_child == null || (next_child.GetNodeType() != "LineComment" && next_child.GetNodeType() != "BlockComment"))
                    {
                        buffer += "\n";
                    }
                    else
                    {
                        if (next_child.GetStartLine() == sub_child.GetEndLine())
                        {
                            for (int i = number_value.Length; i < number_max; ++i) buffer += " ";
                        }
                        else
                        {
                            buffer += "\n";
                        }
                    }
                }
                else if (sub_child.GetNodeType() == "LineComment" || sub_child.GetNodeType() == "BlockComment")
                {
                    if (last_line != 0 && sub_child.GetStartLine() != last_line)
                        buffer += indent + "    ";

                    if (sub_child.GetNodeType() == "LineComment")
                    {
                        var value = sub_child.GetElementText();
                        if (value.Length >= 3 && value[2] != '/')
                            buffer += "// " + value.Substring(2).Trim();
                        else
                            buffer += value;
                    }   
                    else
                        buffer += sub_child.GetElementText();

                    buffer += "\n";
                }

                last_line = sub_child.GetEndLine();
            }
            buffer += indent + "}\n";
        }

        public void FormatService(AProtobufServiceElement child, string indent, ref string buffer)
        {
            var service_name = child.GetServiceName();
            if (service_name == null) throw new System.Exception("child.GetServiceName() == null");

            buffer += indent + "service " + service_name.GetElementText() + "\n";

            var service_body = child.GetServiceBody();
            if (service_body == null) throw new System.Exception("child.GetServiceBody() == null");

            buffer += indent + "{\n";

            int last_line = 0;
            var child_list = service_body.GetChilds();
            for (int index = 0; index < child_list.Count; ++index)
            {
                var sub_child = child_list[index];
                ABnfElement next_child = null;
                if (index + 1 < child_list.Count) next_child = child_list[index + 1];

                if (last_line != 0)
                {
                    int line_count = sub_child.GetStartLine() - last_line - 1;
                    for (int i = 0; i < line_count; ++i) buffer += "\n";
                }

                if (sub_child is AProtobufServiceRpcElement)
                {
                    var sub_element = sub_child as AProtobufServiceRpcElement;
                    FormatServiceRpc(sub_element, indent + "    ", ref buffer);

                    if (next_child == null || (next_child.GetNodeType() != "LineComment" && next_child.GetNodeType() != "BlockComment"))
                    {
                        buffer += "\n";
                    }
                    else
                    {
                        if (next_child.GetStartLine() == sub_child.GetEndLine())
                        {
                            buffer += " ";
                        }
                        else
                        {
                            buffer += "\n";
                        }
                    }
                }
                else if (sub_child.GetNodeType() == "LineComment" || sub_child.GetNodeType() == "BlockComment")
                {
                    if (last_line != 0 && sub_child.GetStartLine() != last_line)
                        buffer += indent + "    ";

                    if (sub_child.GetNodeType() == "LineComment")
                    {
                        var value = sub_child.GetElementText();
                        if (value.Length >= 3 && value[2] != '/')
                            buffer += "// " + value.Substring(2).Trim();
                        else
                            buffer += value;
                    }
                    else
                        buffer += sub_child.GetElementText();

                    buffer += "\n";
                }

                last_line = sub_child.GetEndLine();
            }
            
            buffer += indent + "}\n";
        }

        public void FormatServiceRpc(AProtobufServiceRpcElement child, string indent, ref string buffer)
		{
            var rpc_name = child.GetServiceRpcName();
            if (rpc_name == null) throw new System.Exception("child.GetServiceRpcName() == null");

            buffer += indent + "rpc " + rpc_name.GetElementText() + "(";

            var rpc_req = child.GetServiceRpcReq();
            if (rpc_req == null) throw new System.Exception("child.GetServiceRpcReq() == null");
            buffer += FormatCustomType(rpc_req.GetCustomType()) + ")";

            buffer += " returns (";

            var rpc_rsp = child.GetServiceRpcRsp();
            if (rpc_rsp == null) throw new System.Exception("child.GetServiceRpcRsp() == null");
            buffer += FormatCustomType(rpc_rsp.GetCustomType()) + ")";

            var rpc_body = child.GetServiceRpcBody();
            if (rpc_body != null)
			{
                buffer += "\n";
                FormatServiceRpcBody(rpc_body, indent, ref buffer);
            }

            buffer += ";";
        }

        public void FormatServiceRpcBody(AProtobufServiceRpcBodyElement child, string indent, ref string buffer)
		{
            int option_name_max = 0;
            int option_value_max = 0;
            var option_list = child.GetServiceOptionList();
            foreach (var option in option_list)
            {
                var custom_type = option.GetCustomType();
                if (custom_type == null) throw new System.Exception("option.GetCustomType() == null");
                string name = "(" + FormatCustomType(custom_type) + ")";
                if (option_name_max < name.Length) option_name_max = name.Length;

                var text = option.GetConst();
                if (text == null) throw new System.Exception("option.GetConst() == null");
                if (option_value_max < text.GetLength() + 3) option_value_max = text.GetLength() + 3;// 算上text前面的等号和空格，text后面的分号 
            }
            // 将option_name_max,option_value_max调整为大于当前的4的倍数
            option_name_max += 4 - option_name_max % 4;
            option_value_max += 4 - option_value_max % 4;

            buffer += indent + "{\n";

            int last_line = 0;
            var child_list = child.GetChilds();
            for (int index = 0; index < child_list.Count; ++index)
            {
                var sub_child = child_list[index];
                ABnfElement next_child = null;
                if (index + 1 < child_list.Count) next_child = child_list[index + 1];

                if (last_line != 0)
                {
                    int line_count = sub_child.GetStartLine() - last_line - 1;
                    for (int i = 0; i < line_count; ++i) buffer += "\n";
                }

                if (sub_child.GetNodeType() == "LineComment" || sub_child.GetNodeType() == "BlockComment")
                {
                    if (last_line != 0 && sub_child.GetStartLine() != last_line)
                        buffer += indent + "    ";

                    if (sub_child.GetNodeType() == "LineComment")
                    {
                        var value = sub_child.GetElementText();
                        if (value.Length >= 3 && value[2] != '/')
                            buffer += "// " + value.Substring(2).Trim();
                        else
                            buffer += value;
                    }
                    else
                        buffer += sub_child.GetElementText();

                    buffer += "\n";
                }
                else if (sub_child is AProtobufServiceOptionElement)
                {
                    buffer += indent + "    option ";

                    var sub_element = sub_child as AProtobufServiceOptionElement;
                    var custom_type = sub_element.GetCustomType();
                    if (custom_type == null) throw new System.Exception("sub_element.GetCustomType() == null");

                    string name = "(" + FormatCustomType(custom_type) + ")";
                    buffer += name;

                    var text = sub_element.GetConst();
                    if (text == null) throw new System.Exception("sub_element.GetConst() == null");

                    for (int i = name.Length; i < option_name_max; ++i) buffer += " ";
                    var option_value = "= " + text.GetElementText() + ";";
                    buffer += option_value;

                    if (next_child == null || (next_child.GetNodeType() != "LineComment" && next_child.GetNodeType() != "BlockComment"))
                    {
                        buffer += "\n";
                    }
                    else
                    {
                        if (next_child.GetStartLine() == sub_child.GetEndLine())
                        {
                            for (int i = option_value.Length; i < option_value_max; ++i) buffer += " ";
                        }
                        else
                        {
                            buffer += "\n";
                        }
                    }
                }
                
                last_line = sub_child.GetEndLine();
            }
            buffer += indent + "}";
        }

        public int CalcAllTypeFormatLength(AProtobufAllTypeElement all_type)
        {
            var repeated_type = all_type.GetRepeatedType();
            if (repeated_type != null)
            {
                int length = "repeated ".Length;
                var sub_primitive_type = repeated_type.GetPrimitiveType();
                var sub_custom_type = repeated_type.GetCustomType();
                if (sub_primitive_type != null)
                    length += sub_primitive_type.GetLength();
                else if (sub_custom_type != null)
                    length += CalcCustomTypeFormatLength(sub_custom_type as AProtobufCustomTypeElement);
                else
                    throw new System.Exception("repeated_type.GetPrimitiveType() == null && repeated_type.GetCustomType() == null");

                return length;
            }

            var map_type = all_type.GetMapType();
            if (map_type != null)
            {
                int length = "map<".Length;
                var sub_all_type = map_type.GetAllType();
                if (sub_all_type == null) throw new System.Exception("map_type.GetAllType() == null");
                length += CalcAllTypeFormatLength(sub_all_type);

                length += ", ".Length;

                var sub_primitive_type = map_type.GetPrimitiveType();
                var sub_custom_type = map_type.GetCustomType();
                if (sub_primitive_type != null)
                    length += sub_primitive_type.GetLength();
                else if (sub_custom_type != null)
                    length += CalcCustomTypeFormatLength(sub_custom_type as AProtobufCustomTypeElement);
                else
                    throw new System.Exception("map_type.GetPrimitiveType() == null && map_type.GetCustomType() == null");

                length += ">".Length;
                return length;
            }

            var primitive_type = all_type.GetPrimitiveType();
            if (primitive_type != null) return primitive_type.GetLength();

            var custom_type = all_type.GetCustomType();
            if (custom_type != null) return CalcCustomTypeFormatLength(custom_type);

            throw new System.Exception("CalcAllTypeFormatLength");
        }

        public int CalcCustomTypeFormatLength(AProtobufCustomTypeElement custom_type)
        {
            var id_list = custom_type.GetIdList();
            int length = 0;
            foreach (var id in id_list)
                length += id.GetLength();
            if (id_list.Count > 0)
                length += id_list.Count - 1;
            return length;
        }

        public string FormatAllType(AProtobufAllTypeElement all_type)
        {
            var repeated_type = all_type.GetRepeatedType();
            if (repeated_type != null)
            {
                string buffer = "repeated ";
                var sub_primitive_type = repeated_type.GetPrimitiveType();
                var sub_custom_type = repeated_type.GetCustomType();
                if (sub_primitive_type != null)
                    buffer += sub_primitive_type.GetElementText();
                else if (sub_custom_type != null)
                    buffer += FormatCustomType(sub_custom_type as AProtobufCustomTypeElement);
                else
                    throw new System.Exception("repeated_type.GetPrimitiveType() == null && repeated_type.GetCustomType() == null");

                return buffer;
            }

            var map_type = all_type.GetMapType();
            if (map_type != null)
            {
                string buffer = "map<";
                var sub_all_type = map_type.GetAllType();
                if (sub_all_type == null) throw new System.Exception("map_type.GetAllType() == null");
                buffer += FormatAllType(sub_all_type);

                buffer += ", ";

                var sub_primitive_type = map_type.GetPrimitiveType();
                var sub_custom_type = map_type.GetCustomType();
                if (sub_primitive_type != null)
                    buffer += sub_primitive_type.GetElementText();
                else if (sub_custom_type != null)
                    buffer += FormatCustomType(sub_custom_type as AProtobufCustomTypeElement);
                else
                    throw new System.Exception("map_type.GetPrimitiveType() == null && map_type.GetCustomType() == null");

                buffer += ">";
                return buffer;
            }

            var primitive_type = all_type.GetPrimitiveType();
            if (primitive_type != null) return primitive_type.GetElementText();

            var custom_type = all_type.GetCustomType();
            if (custom_type != null) return FormatCustomType(custom_type);

            throw new System.Exception("FormatAllType");
        }

        public string FormatCustomType(AProtobufCustomTypeElement custom_type)
        {
            var id_list = custom_type.GetIdList();
            var name_list = new List<string>();
            foreach (var id in id_list)
                name_list.Add(id.GetElementText());
            return string.Join(".", name_list);
        }

        public void FormatExtend(AProtobufExtendElement child, string indent, ref string buffer)
        {
            var custom_type = child.GetCustomType();
            if (custom_type == null) throw new System.Exception("child.GetCustomType() == null");

            buffer += indent + "extend " + FormatCustomType(custom_type) + "\n";

            var message_body = child.GetMessageBody();
            if (message_body == null) throw new System.Exception("child.GetMessageBody() == null");

            FormatMessageBody(message_body, indent, ref buffer);
        }

        public void FormatOneof(AProtobufOneofElement child, string indent, ref string buffer)
        {
            var oneof_name = child.GetOneofName();
            if (oneof_name == null) throw new System.Exception("child.GetOneofName() == null");

            buffer += indent + "oneof " + oneof_name.GetElementText() + "\n";

            var message_body = child.GetMessageBody();
            if (message_body == null) throw new System.Exception("child.GetMessageBody() == null");

            FormatMessageBody(message_body, indent, ref buffer);
        }

        public void FormatMessage(AProtobufMessageElement child, string indent, ref string buffer)
        {
            var message_name = child.GetMessageName();
            if (message_name == null) throw new System.Exception("child.GetMessageName() == null");

            buffer += indent + "message " + message_name.GetElementText() + "\n";

            var message_body = child.GetMessageBody();
            if (message_body == null) throw new System.Exception("child.GetMessageBody() == null");

            FormatMessageBody(message_body, indent, ref buffer);
        }

        public void FormatMessageBody(AProtobufMessageBodyElement child, string indent, ref string buffer)
        {
            int message_var_max = 0;
            int message_number_max = 0;
            var var_list = child.GetMessageVarList();
            foreach (var variable in var_list)
            {
                int length = 0;
                // 计算修饰符的长度
                var modifier = variable.GetMessageVarModifier();
                if (modifier != null) length += modifier.GetLength() + 1;   // 修饰符后面加空格

                // 计算变量类型的长度
                var all_type = variable.GetAllType();
                if (all_type == null) throw new System.Exception("variable.GetAllType() == null");
                length += CalcAllTypeFormatLength(all_type) + 1;    // 类型后面加空格

                // 计算变量名的长度
                var name = variable.GetMessageVarName();
                if (name == null) throw new System.Exception("variable.GetMessageVarName() == null");
                length += name.GetLength();

                // 记录最长的长度
                if (message_var_max < length) message_var_max = length;

                // 计算编号的长度
                var number = variable.GetNumber();
                if (number == null) throw new System.Exception("variable.GetNumber() == null");
                length = number.GetLength() + 3;    // 算上number前面的等号和空格，number后面的分号 

                // 如果有option，那么就计算option
                var option = variable.GetMessageVarOption();
                if (option != null)
                {
                    length += 2;    // 算上空格和[

                    var option_id = option.GetId();
                    if (option_id == null) throw new System.Exception("option.GetId() == null");
                    length += option_id.GetLength();
                    length += 3;    // 算上空格，等号，空格
                    var option_value = option.GetMessageVarOptionValue();
                    if (option_value == null) throw new System.Exception("option.GetMessageVarOptionValue() == null");
                    length += option_value.GetLength();

                    length += 1;    // 算上]
                }

                // 记录最长的长度
                if (message_number_max < length) message_number_max = length;
            }
            // 将message_var_max,message_number_max调整为大于当前的4的倍数
            message_var_max += 4 - message_var_max % 4;
            message_number_max += 4 - message_number_max % 4;

            int option_name_max = 0;
            int option_value_max = 0;
            var option_list = child.GetMessageOptionList();
            foreach (var option in option_list)
            {
                var custom_type = option.GetCustomType();
                if (custom_type == null) throw new System.Exception("option.GetCustomType() == null");
                string name = "(" + FormatCustomType(custom_type) + ")";
                if (option_name_max < name.Length) option_name_max = name.Length;

                var text = option.GetConst();
                if (text == null) throw new System.Exception("option.GetConst() == null");
                if (option_value_max < text.GetLength() + 3) option_value_max = text.GetLength() + 3;// 算上text前面的等号和空格，text后面的分号 
            }
            // 将option_name_max,option_value_max调整为大于当前的4的倍数
            option_name_max += 4 - option_name_max % 4;
            option_value_max += 4 - option_value_max % 4;

            buffer += indent + "{\n";

            int last_line = 0;
            var child_list = child.GetChilds();
            for (int index = 0; index < child_list.Count; ++index)
            {
                var sub_child = child_list[index];
                ABnfElement next_child = null;
                if (index + 1 < child_list.Count) next_child = child_list[index + 1];

                if (last_line != 0)
                {
                    int line_count = sub_child.GetStartLine() - last_line - 1;
                    for (int i = 0; i < line_count; ++i) buffer += "\n";
                }

                if (sub_child is AProtobufMessageVarElement)
                {
                    var sub_element = sub_child as AProtobufMessageVarElement;
                    var name_value = "";
                    var modifier = sub_element.GetMessageVarModifier();
                    if (modifier != null) name_value += modifier.GetElementText() + " ";

                    var all_type = sub_element.GetAllType();
                    if (all_type == null) throw new System.Exception("sub_element.GetAllType() == null");
                    name_value += FormatAllType(all_type) + " ";

                    var name = sub_element.GetMessageVarName();
                    if (name == null) throw new System.Exception("sub_element.GetMessageVarName() == null");
                    name_value += name.GetElementText();

                    var number = sub_element.GetNumber();
                    if (number == null) throw new System.Exception("sub_element.GetNumber() == null");
                    string number_value = number.GetElementText();

                    buffer += indent + "    " + name_value;
                    for (int i = name_value.Length; i < message_var_max; ++i) buffer += " ";

                    var option = sub_element.GetMessageVarOption();
                    if (option != null)
                    {
                        number_value += " [";

                        var option_id = option.GetId();
                        if (option_id == null) throw new System.Exception("option.GetId() == null");
                        number_value += option_id.GetElementText();
                        number_value += " = ";
                        var option_value = option.GetMessageVarOptionValue();
                        if (option_value == null) throw new System.Exception("option.GetMessageVarOptionValue() == null");
                        number_value += option_value.GetElementText();

                        number_value += "]";
                    }

                    number_value = "= " + number_value + ";";
                    buffer += number_value;

                    if (next_child == null || (next_child.GetNodeType() != "LineComment" && next_child.GetNodeType() != "BlockComment"))
                    {
                        buffer += "\n";
                    }
                    else
                    {
                        if (next_child.GetStartLine() == sub_child.GetEndLine())
                        {
                            for (int i = number_value.Length; i < message_number_max; ++i) buffer += " ";
                        }
                        else
                        {
                            buffer += "\n";
                        }
                    }
                }
                else if (sub_child.GetNodeType() == "LineComment" || sub_child.GetNodeType() == "BlockComment")
                {
                    if (last_line != 0 && sub_child.GetStartLine() != last_line)
                        buffer += indent + "    ";

                    if (sub_child.GetNodeType() == "LineComment")
                    {
                        var value = sub_child.GetElementText();
                        if (value.Length >= 3 && value[2] != '/')
                            buffer += "// " + value.Substring(2).Trim();
                        else
                            buffer += value;
                    }
                    else
                        buffer += sub_child.GetElementText();

                    buffer += "\n";
                }
                else if (sub_child is AProtobufMessageOptionElement)
                {
                    buffer += indent + "    option ";

                    var sub_element = sub_child as AProtobufMessageOptionElement;
                    var custom_type = sub_element.GetCustomType();
                    if (custom_type == null) throw new System.Exception("sub_element.GetCustomType() == null");

                    string name = "(" + FormatCustomType(custom_type) + ")";
                    buffer += name;

                    var text = sub_element.GetConst();
                    if (text == null) throw new System.Exception("sub_element.GetConst() == null");

                    for (int i = name.Length; i < option_name_max; ++i) buffer += " ";
                    var option_value = "= " + text.GetElementText() + ";";
                    buffer += option_value;

                    if (next_child == null || (next_child.GetNodeType() != "LineComment" && next_child.GetNodeType() != "BlockComment"))
                    {
                        buffer += "\n";
                    }
                    else
                    {
                        if (next_child.GetStartLine() == sub_child.GetEndLine())
                        {
                            for (int i = option_value.Length; i < option_value_max; ++i) buffer += " ";
                        }
                        else
                        {
                            buffer += "\n";
                        }
                    }
                }
                else if (sub_child is AProtobufMessageExtensionsElement
                    || sub_child is AProtobufMessageReservedElement)
                {
                    buffer += indent + "    " + sub_child.GetElementText();

                    if (next_child == null || (next_child.GetNodeType() != "LineComment" && next_child.GetNodeType() != "BlockComment"))
                    {
                        buffer += "\n";
                    }
                    else
                    {
                        if (next_child.GetStartLine() == sub_child.GetEndLine())
                        {
                            buffer += "    ";
                        }
                        else
                        {
                            buffer += "\n";
                        }
                    }
                }
                else if (sub_child is AProtobufEnumElement)
                {
                    FormatEnum(sub_child as AProtobufEnumElement, indent + "    ", ref buffer);
                }
                else if (sub_child is AProtobufMessageElement)
                {
                    FormatMessage(sub_child as AProtobufMessageElement, indent + "    ", ref buffer);
                }
                else if (sub_child is AProtobufOneofElement)
                {
                    FormatOneof(sub_child as AProtobufOneofElement, indent + "    ", ref buffer);
                }

                last_line = sub_child.GetEndLine();
            }
            buffer += indent + "}\n";
        }

        // 解析部分////////////////////////////////////////////////////////////////////////////////////////////////////

        public string GetPackage()
        {
            return m_package;
        }

        // 更新引用表现
        public override void UpdateReference()
        {
            var project = m_project as AProtobufProjectInfo;
            if (project == null) return;

            foreach (var pair in m_custom_info.messages)
            {
                int count = project.GetReferenceCount(m_package + "." + pair.Key);
                AddReferenceInfo(pair.Value.element, count);
            }

            foreach (var pair in m_custom_info.enums)
            {
                int count = project.GetReferenceCount(m_package + "." + pair.Key);
                AddReferenceInfo(pair.Value.element, count);
            }
        }

        // 更新引用信息
        public void CollectReference()
        {
            m_custom_info.ref_map.Clear();
            foreach (var custom_type in m_customtype_set)
            {
                var element = custom_type.GetReference().GotoDefinition();
                if (element == null) continue;

                // 获取所在的包
                var file = element.GetFile() as AProtobufFile;
                if (file == null) continue;

                string name = null;
                if (element is AProtobufMessageNameElement)
                {
                    name = file.m_package + "." + element.GetElementText();
                }
                else if (element is AProtobufEnumNameElement)
                {
                    name = file.m_package + "." + element.GetElementText();
                }

                if (name == null) continue;

                int count = 0;
                if (m_custom_info.ref_map.TryGetValue(name, out count))
                    m_custom_info.ref_map.Remove(name);
                count++;
                m_custom_info.ref_map.Add(name, count);
            }
        }

        // 更新分析内容
        public override void UpdateAnalysis()
        {
            m_index.Clear();
            m_name_set.Clear();
            m_customtype_set.Clear();
            m_root = null;
            if (m_abnf == null) return;

            m_root = m_abnf.Analysis(this);
            if (m_root == null) return;

            CollectIndex(m_root);
            CollectCustomInfo();
        }

        public override void UpdateError()
        {
            if (m_root == null) return;

            CollectError(m_root);
            AnalysisError(m_root);
            CheckRepeatDefine();
        }

        public void CollectIndex(ABnfNodeElement node)
        {
            if (node is AProtobufIdElement)
            {
                var value = node.GetElementText();
                HashSet<AProtobufIdElement> set;
                if (!m_index.TryGetValue(value, out set))
                {
                    set = new HashSet<AProtobufIdElement>();
                    m_index.Add(value, set);
                }
                set.Add(node as AProtobufIdElement);
            }

            foreach (var child in node.GetChilds())
            {
                if (child is ABnfNodeElement)
                    CollectIndex(child as ABnfNodeElement);
            }
        }

        public Dictionary<string, HashSet<AProtobufIdElement>> GetIndex() { return m_index; }

        // 收集参数
        private void CollectCustomInfo()
        {
            m_syntax = "proto2";
            m_package = "";
            m_imports.Clear();
            m_custom_info = new AProtobufCustomInfo();

            // 获取syntax
            foreach (var element in m_root.GetChilds())
            {
                if (element is ABnfErrorElement) continue;

                if (element is AProtobufSyntaxElement)
                {
                    var syntax = element as AProtobufSyntaxElement;
                    if (syntax == null) continue;
                    var string_child = syntax.GetText();
                    if (string_child == null) continue;

                    var string_value = string_child.GetElementString();
                    if (string_value != "proto2" && string_value != "proto3")
                        AddAnalysisErrorInfo(string_child, "这里只能填写proto2或者proto3");
                    m_syntax = string_value;

                    var string_list = syntax.GetStringList();
                    if (string_list.Count == 0
                        || string_list[string_list.Count - 1].GetElementText() != ";")
                    {
                        AddAnalysisErrorInfo(element, "必须以;结尾");
                    }
                    continue;
                }
                
                if (element is AProtobufPackageElement)
                {
                    var package = element as AProtobufPackageElement;
                    if (package == null) continue;

                    var name_child = package.GetPackageName();
                    if (name_child == null) continue;

                    var name = name_child.GetElementText();
                    if (name.Length != 0 && m_package.Length != 0)
                    {
                        AddAnalysisErrorInfo(element, "package最多只能定义一个");
                        continue;
                    }
                    m_package = name;

                    if (package.GetString() == null)
                    {
                        AddAnalysisErrorInfo(element, "必须以;结尾");
                    }
                    continue;
                }
                
                if (element is AProtobufImportElement)
                {
                    var import = element as AProtobufImportElement;
                    var string_child = import.GetText();
                    if (string_child != null)
                    {
                        m_imports.Add(string_child.GetElementString());
                    }

                    if (import.GetString() == null)
                    {
                        AddAnalysisErrorInfo(element, "必须以;结尾");
                    }
                    continue;
                }
                
                if (element is AProtobufMessageElement)
                {
                    CheckMessage(element as AProtobufMessageElement, m_custom_info.messages);
                    continue;
                }

                if (element is AProtobufExtendElement)
                {
                    CheckExtend(element as AProtobufExtendElement, m_custom_info.extend_names, m_custom_info.extend_numbers);
                    continue;
                }

                if (element is AProtobufOneofElement)
                {
                    CheckOneof(element as AProtobufOneofElement, m_custom_info.oneof_names, m_custom_info.oneof_numbers);
                    continue;
                }

                if (element is AProtobufEnumElement)
                {
                    CheckEnum(element as AProtobufEnumElement, m_custom_info.enums);
                    continue;
                }
            }
        }

        // 检查重复定义
        private void CheckRepeatDefine()
        {
            AProtobufProjectInfo project = GetProjectInfo() as AProtobufProjectInfo;
            if (project == null) return;

            var reference_info = project.GetReferenceInfo(m_package);
            if (reference_info == null) return;

            foreach (var message in m_custom_info.messages)
            {
                HashSet<AProtobufCustomInfo> info_set;
                if (reference_info.m_message_info.TryGetValue(message.Key, out info_set))
                {
                    if (info_set.Count <= 1) continue;

                    AddCheckErrorInfo(message.Value.element.GetMessageName(), "重复定义");
                }
            }

            foreach (var enumv in m_custom_info.enums)
            {
                HashSet<AProtobufCustomInfo> info_set;
                if (reference_info.m_enum_info.TryGetValue(enumv.Key, out info_set))
                {
                    if (info_set.Count <= 1) continue;

                    AddCheckErrorInfo(enumv.Value.element.GetEnumName(), "重复定义");
                }
            }
        }

        public AProtobufCustomInfo GetCustomInfo() { return m_custom_info; }

        public HashSet<string> GetNameSet() { return m_name_set; }

        private void CheckExtend(AProtobufExtendElement element, Dictionary<string, ABnfElement> extend_names, HashSet<int> extend_numbers)
        {
            var message_body = element.GetMessageBody();
            if (message_body == null) return;

            {
                var variable_list = message_body.GetMessageVarList();
                foreach (var variable in variable_list)
                {
                    var var_name_child = variable.GetMessageVarName();
                    if (var_name_child == null) continue;
                    string var_name = var_name_child.GetElementText();
                    if (extend_names.ContainsKey(var_name))
                    {
                        AddAnalysisErrorInfo(var_name_child, "重复定义");
                    }
                    else if (var_name.Length > 0 && char.IsDigit(var_name[0]))
                    {
                        AddAnalysisErrorInfo(var_name_child, "extend字段名称不能以数字开头");
                    }
                    else
                    {
                        extend_names.Add(var_name, var_name_child);
                        m_name_set.Add(var_name);
                    }

                    var var_num_child = variable.GetNumber();
                    if (var_num_child == null) continue;
                    int var_num = 0;
                    if (!int.TryParse(var_num_child.GetElementText(), out var_num) || var_num <= 0)
                    {
                        AddAnalysisErrorInfo(var_num_child, "请输入大于0的数字");
                    }
                    else if (extend_numbers.Contains(var_num))
                    {
                        AddAnalysisErrorInfo(var_num_child, "当前数字已被使用");
                    }
                    else
                    {
                        extend_numbers.Add(var_num);
                    }
                }
            }
        }

        private void CheckOneof(AProtobufOneofElement element, Dictionary<string, ABnfElement> oneof_names, HashSet<int> oneof_numbers)
        {
            var message_body = element.GetMessageBody();
            if (message_body == null) return;

            {
                var variable_list = message_body.GetMessageVarList();
                foreach (var variable in variable_list)
                {
                    var var_name_child = variable.GetMessageVarName();
                    if (var_name_child == null) continue;
                    string var_name = var_name_child.GetElementText();
                    if (oneof_names.ContainsKey(var_name))
                    {
                        AddAnalysisErrorInfo(var_name_child, "重复定义");
                    }
                    else if (var_name.Length > 0 && char.IsDigit(var_name[0]))
                    {
                        AddAnalysisErrorInfo(var_name_child, "oneof字段名称不能以数字开头");
                    }
                    else
                    {
                        oneof_names.Add(var_name, var_name_child);
                        m_name_set.Add(var_name);
                    }

                    var var_num_child = variable.GetNumber();
                    if (var_num_child == null) continue;
                    int var_num = 0;
                    if (!int.TryParse(var_num_child.GetElementText(), out var_num) || var_num <= 0)
                    {
                        AddAnalysisErrorInfo(var_num_child, "请输入大于0的数字");
                    }
                    else if (oneof_numbers.Contains(var_num))
                    {
                        AddAnalysisErrorInfo(var_num_child, "当前数字已被使用");
                    }
                    else
                    {
                        oneof_numbers.Add(var_num);
                    }
                }
            }
        }

        private void CheckMessage(AProtobufMessageElement element, Dictionary<string, AProtobufMessageInfo> messages)
        {
            var name_child = element.GetMessageName();
            if (name_child == null)
            {
                AddAnalysisErrorInfo(element, "message没有定义名称");
                return;
            }

            var message_name = name_child.GetElementText();
            if (messages.ContainsKey(message_name))
            {
                AddAnalysisErrorInfo(name_child, "重复定义");
            }
            else if (message_name.Length > 0 && char.IsDigit(message_name[0]))
            {
                AddAnalysisErrorInfo(name_child, "message名称不能以数字开头");
            }

            AProtobufMessageInfo info = new AProtobufMessageInfo();
            info.element = element;
            messages[message_name] = info;

            var message_body = element.GetMessageBody();
            if (message_body == null)
            {
                AddAnalysisErrorInfo(element, "message没有定义内容");
                return;
            }

            {
                var variable_list = message_body.GetMessageVarList();
                HashSet<int> var_nums = new HashSet<int>();
                HashSet<string> var_names = new HashSet<string>();
                foreach (var variable in variable_list)
                {
                    var modifier_child = variable.GetMessageVarModifier();
                    if (modifier_child != null)
                    {
                        var modifier_value = modifier_child.GetElementText();
                        if (m_syntax == "proto2")
                        {
                            if (modifier_value == "singular")
                            {
                                AddAnalysisErrorInfo(modifier_child, m_syntax + "不支持关键字:" + modifier_value + ", proto3支持");
                            }
                        }
                        else if (m_syntax == "proto3")
                        {
                            if (modifier_value == "required" || modifier_value == "optional")
                            {
                                AddAnalysisErrorInfo(modifier_child, m_syntax + "不支持关键字:" + modifier_value + ", proto2支持");
                            }
                        }
                    }

                    var all_type_child = variable.GetAllType();
                    if (all_type_child == null) continue;
                    CheckAllType(all_type_child);

                    var var_name_child = variable.GetMessageVarName();
                    if (var_name_child == null) continue;
                    string var_name = var_name_child.GetElementText();
                    if (var_names.Contains(var_name))
                    {
                        AddAnalysisErrorInfo(var_name_child, "重复定义");
                    }
                    else if (var_name.Length > 0 && char.IsDigit(var_name[0]))
                    {
                        AddAnalysisErrorInfo(var_name_child, "message字段名称不能以数字开头");
                    }
                    else
                    {
                        var_names.Add(var_name);
                        m_name_set.Add(var_name);
                    }

                    var var_num_child = variable.GetNumber();
                    if (var_num_child == null) continue;
                    int var_num = 0;
                    if (!int.TryParse(var_num_child.GetElementText(), out var_num) || var_num <= 0)
                    {
                        AddAnalysisErrorInfo(var_num_child, "请输入大于0的数字");
                    }
                    else if (var_nums.Contains(var_num))
                    {
                        AddAnalysisErrorInfo(var_num_child, "当前数字已被使用");
                    }
                    else
                    {
                        var_nums.Add(var_num);
                    }
                }
            }
            {
                var option_list = message_body.GetMessageOptionList();
                HashSet<string> option_names = new HashSet<string>();
                foreach (var option in option_list)
                {
                    var custom_type = option.GetCustomType();
                    if (custom_type == null) continue;

                    var option_name = FormatCustomType(custom_type);
                    if (option_names.Contains(option_name))
                    {
                        AddAnalysisErrorInfo(custom_type, "重复定义");
                    }
                    else
                    {
                        option_names.Add(option_name);
                        m_name_set.Add(option_name);
                    }
                }
            }
            {
                var enum_list = message_body.GetEnumList();
                foreach (var enumv in enum_list)
                {
                    CheckEnum(enumv, info.enums);
                }
            }
            {
                var message_list = message_body.GetMessageList();
                foreach (var messagev in message_list)
                {
                    CheckMessage(messagev, info.messages);
                }
            }
        }

        private void CheckEnum(AProtobufEnumElement element, Dictionary<string, AProtobufEnumInfo> enums)
        {
            var name_child = element.GetEnumName();
            if (name_child == null)
            {
                AddAnalysisErrorInfo(element, "enum没有定义名称");
                return;
            }

            var enum_name = name_child.GetElementText();
            if (enums.ContainsKey(enum_name))
            {
                AddAnalysisErrorInfo(name_child, "重复定义");
            }
            else if (enum_name.Length > 0 && char.IsDigit(enum_name[0]))
            {
                AddAnalysisErrorInfo(name_child, "enum名称不能以数字开头");
            }

            var enum_body = element.GetEnumBody();
            if (enum_body == null)
            {
                AddAnalysisErrorInfo(element, "enum没有定义内容");
                return;
            }

            AProtobufEnumInfo info = new AProtobufEnumInfo();
            info.element = element;
            enums[enum_name] = info;

            var variable_list = enum_body.GetEnumVarList();
            HashSet<int> var_nums = new HashSet<int>();
            HashSet<string> var_names = new HashSet<string>();
            foreach (var variable in variable_list)
            {
                var var_name_child = variable.GetEnumVarName();
                if (var_name_child == null) continue;
                string var_name = var_name_child.GetElementText();
                if (var_names.Contains(var_name))
                {
                    AddAnalysisErrorInfo(var_name_child, "重复定义");
                }
                else if (var_name.Length > 0 && char.IsDigit(var_name[0]))
                {
                    AddAnalysisErrorInfo(var_name_child, "enum字段名称不能以数字开头");
                }
                else
                {
                    var_names.Add(var_name);
                    m_name_set.Add(var_name);

                    info.name_map.Add(var_name, variable);
                }

                var var_num_child = variable.GetNumber();
                if (var_num_child == null) continue;
                int var_num = 0;
                if (!int.TryParse(var_num_child.GetElementText(), out var_num) || var_num < 0)
                {
                    AddAnalysisErrorInfo(var_num_child, "请输入大于0的数字");
                }
                else if (var_nums.Contains(var_num))
                {
                    AddAnalysisErrorInfo(var_num_child, "当前数字已被使用");
                }
                else if (var_nums.Count == 0 && var_num != 0)
                {
                    if (m_syntax == "proto3")
                    {
                        AddAnalysisErrorInfo(var_num_child, "proto3语法中的枚举第一个字段的枚举值必须是0");
                    }
                }
                else
                {
                    var_nums.Add(var_num);
                }
            }
        }

        private void CheckMapType(AProtobufMapTypeElement element)
        {
            var all_type = element.GetAllType();
            if (all_type == null)
            {
                AddAnalysisErrorInfo(element, "map没有定义Key的类型");
            }
            else
            {
                var key_type = all_type.GetElementText();
                if (key_type != "string" && key_type != "bool" && !AProtobufFactoryClass.inst.IsInt(key_type))
                    AddAnalysisErrorInfo(all_type, "map的key必须是整型、string、bool");
            }

            CollectCustomTypeElement(element.GetCustomType());

            if (element.GetPrimitiveType() == null && element.GetCustomType() == null)
                AddAnalysisErrorInfo(element, "map没有定义Value的类型");
        }

        private void CheckRepeatedType(AProtobufRepeatedTypeElement element)
        {
            CollectCustomTypeElement(element.GetCustomType());
        }

        private void CheckAllType(AProtobufAllTypeElement element)
        {
            var map_child = element.GetMapType();
            if (map_child != null)
            {
                CheckMapType(map_child);
                return;
            }

            var repeated_child = element.GetRepeatedType();
            if (repeated_child != null)
            {
                CheckRepeatedType(repeated_child);
                return;
            }

            CollectCustomTypeElement(element.GetCustomType());
        }

        private void CollectCustomTypeElement(AProtobufCustomTypeElement element)
        {
            if (element == null) return;
            if (!m_customtype_set.Contains(element))
                m_customtype_set.Add(element);
        }
    }
}
