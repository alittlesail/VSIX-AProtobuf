
using System.Collections.Generic;

namespace ALittle
{
    public class AProtobufMessageOptionReference : AProtobufReferenceTemplate<AProtobufMessageOptionElement>
    {
        public AProtobufMessageOptionReference(ABnfElement element) : base(element) { }

        public override string QueryQuickInfo()
        {
            if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW)
            {
                var custom_type = m_element.GetCustomType();
                if (custom_type == null) return null;

                var id_list = custom_type.GetIdList();
                if (id_list.Count == 0) return null;

                var id = id_list[id_list.Count - 1];
                if (id_list.Count != 1 && id_list.Count != 2) return null;
                if (id_list.Count == 2 && id_list[0].GetElementText() != "ProtoDB") return null;

                var const_value = m_element.GetConst();
                if (const_value == null) return null;

                var const_text = const_value.GetText();
                var id_text = id.GetElementText();

                if (id_text == "primary")
                {
                    return "填写一个字段为主键或多个字段为符合主键，多个字段使用,隔开";
                }
                else if (id_text == "unique" || id_text == "index")
                {
                    return "填写一个字段为索引或多个字段为符合索引，多个字段使用,隔开。可以定义多组，多组则使用|隔开";
                }
                else if (id_text == "split_count")
                {
                    return "定义分表数量";
                }
                else if (id_text == "split_incr_interval")
                {
                    return "定义分表之间自增长ID的区间间隔";
                }
                else if (id_text == "split_incr_start")
                {
                    return "定义第一张分表的自增长ID起始值";
                }
                else if (id_text == "primary_incr")
                {
                    return "定义主键是都是自增长，请填\"true\"或者\"false\"";
                }
            }

            return null;
        }

        public override ABnfGuessError CheckError()
        {
            if (GeneralOptions.Instance.ProjectTeam == ProjectTeamTypes.LW)
            {
                var message_body = m_element.GetParent() as AProtobufMessageBodyElement;
                if (message_body == null) return null;

                var var_list = message_body.GetMessageVarList();
                var name_set = new HashSet<string>();
                foreach (var var_dec in var_list)
                {
                    var var_name = var_dec.GetMessageVarName();
                    if (var_name != null && !name_set.Contains(var_name.GetElementText()))
                        name_set.Add(var_name.GetElementText());
                }

                var custom_type = m_element.GetCustomType();
                if (custom_type == null) return null;

                var id_list = custom_type.GetIdList();
                if (id_list.Count == 0) return null;

                var id = id_list[id_list.Count - 1];
                if (id_list.Count != 1 && id_list.Count != 2) return null;
                if (id_list.Count == 2 && id_list[0].GetElementText() != "ProtoDB") return null;

                var const_value = m_element.GetConst();
                if (const_value == null) return null;

                var const_text = const_value.GetText();
                var id_text = id.GetElementText();

                if (id_text == "primary")
                {
                    if (const_text == null)
                        return new ABnfGuessError(const_value, id_text + "必须使用字符串赋值");
                    var const_string = const_text.GetElementString();
                    if (const_string == "")
                        return new ABnfGuessError(const_value, id_text + "不能是空串");
                    var const_split = const_string.Split(',');
                    foreach (var const_var in const_split)
                    {
                        var const_var_trim = const_var.Trim();
                        if (const_var_trim.Length == 0)
                            return new ABnfGuessError(const_value, id_text +"内部包含空字段名");

                        if (!name_set.Contains(const_var_trim))
                            return new ABnfGuessError(const_value, const_var_trim + "不是字段名");
                    }
                }
                else if (id_text == "unique" || id_text == "index")
                {
                    if (const_text == null)
                        return new ABnfGuessError(const_value, id_text + "必须使用字符串赋值");
                    var const_string = const_text.GetElementString();
                    if (const_string == "")
                        return new ABnfGuessError(const_value, id_text + "不能是空串");
                    var const_combine_split = const_string.Split('|');
                    foreach (var const_combine in const_combine_split)
                    {
                        var const_split = const_combine.Split(',');
                        foreach (var const_var in const_split)
                        {
                            var const_var_trim = const_var.Trim();
                            if (const_var_trim.Length == 0)
                                return new ABnfGuessError(const_value, id_text + "内部包含空字段名");

                            if (!name_set.Contains(const_var_trim))
                                return new ABnfGuessError(const_value, const_var_trim + "不是字段名");
                        }
                    }
                }
                else if (id_text == "split_count")
                {
                    if (const_text == null)
                        return new ABnfGuessError(const_value, id_text + "必须使用字符串赋值");
                    var const_string = const_text.GetElementString();

                    if (!int.TryParse(const_string, out int result))
                        return new ABnfGuessError(const_value, id_text + "必须是一个数字");
                    if (result < 2)
                        return new ABnfGuessError(const_value, id_text + "必须大于或等于2");
                }
                else if (id_text == "split_incr_interval")
                {
                    if (const_text == null)
                        return new ABnfGuessError(const_value, id_text + "必须使用字符串赋值");
                    var const_string = const_text.GetElementString();

                    if (!long.TryParse(const_string, out long result))
                        return new ABnfGuessError(const_value, id_text + "必须是一个数字");
                    if (result <= 0)
                        return new ABnfGuessError(const_value, id_text + "必须大于0");
                }
                else if (id_text == "split_incr_start")
                {
                    if (const_text == null)
                        return new ABnfGuessError(const_value, id_text + "必须使用字符串赋值");
                    var const_string = const_text.GetElementString();

                    if (!int.TryParse(const_string, out int result))
                        return new ABnfGuessError(const_value, id_text + "必须是一个数字");
                    if (result < 0)
                        return new ABnfGuessError(const_value, id_text + "必须大于或等于0");
                }
                else if (id_text == "primary_incr")
                {
                    if (const_text == null)
                        return new ABnfGuessError(const_value, id_text + "必须使用字符串赋值");
                    var const_string = const_text.GetElementString();

                    if (const_string != "true" && const_string != "false")
                        return new ABnfGuessError(const_value, id_text + "只能填\"true\"或\"false\"");
                }
            }

            return null;
        }
    }
}
