
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace ALittle
{
    public class AProtobufRefrenceInfo
    {
        public Dictionary<AProtobufFileItem, AProtobufCustomInfo> m_custom_info = new Dictionary<AProtobufFileItem, AProtobufCustomInfo>();
        public Dictionary<string, HashSet<AProtobufCustomInfo>> m_message_info = new Dictionary<string, HashSet<AProtobufCustomInfo>>();
        public Dictionary<string, HashSet<AProtobufCustomInfo>> m_enum_info = new Dictionary<string, HashSet<AProtobufCustomInfo>>();
    }

    public class AProtobufProjectInfo : ProjectInfo
    {
        Dictionary<string, AProtobufRefrenceInfo> m_refrences = new Dictionary<string, AProtobufRefrenceInfo>();
        Dictionary<AProtobufFileItem, string> m_file_map_package = new Dictionary<AProtobufFileItem, string>();

        // import路径映射AProtobufProjectDetail
        Dictionary<string, AProtobufFileItem> m_import_cache_1 = new Dictionary<string, AProtobufFileItem>();
        // AProtobufProjectDetail内部的所有import路径
        Dictionary<AProtobufFileItem, HashSet<string>> m_import_cache_2 = new Dictionary<AProtobufFileItem, HashSet<string>>();

        Dictionary<FileItem, Dictionary<string, int>> m_file_map_ref = new Dictionary<FileItem, Dictionary<string, int>>();
        Dictionary<string, int> m_ref_map = new Dictionary<string, int>();

        public AProtobufProjectInfo(ABnfFactory factory, ABnf abnf, string path)
            : base(factory, abnf, path)
        {
        }

        public override void AddAnalysis(FileItem file_item)
        {
            var file = file_item as AProtobufFileItem;
            if (file == null) return;

            RemoveAnalysis(file);

            string package = file.GetPackage();
            m_file_map_package[file] = package;

            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null)
            {
                refernce_info = new AProtobufRefrenceInfo();
                m_refrences[package] = refernce_info;
            }

            var custom_info = file.GetCustomInfo();
            refernce_info.m_custom_info.Add(file, custom_info);
            foreach (var name in custom_info.messages.Keys)
            {
                HashSet<AProtobufCustomInfo> info_set;
                if (!refernce_info.m_message_info.TryGetValue(name, out info_set))
                {
                    info_set = new HashSet<AProtobufCustomInfo>();
                    refernce_info.m_message_info.Add(name, info_set);
                }
                info_set.Add(custom_info);
            }
            foreach (var name in custom_info.enums.Keys)
            {
                HashSet<AProtobufCustomInfo> info_set;
                if (!refernce_info.m_enum_info.TryGetValue(name, out info_set))
                {
                    info_set = new HashSet<AProtobufCustomInfo>();
                    refernce_info.m_enum_info.Add(name, info_set);
                }
                info_set.Add(custom_info);
            }

            file.CollectReference();

            m_file_map_ref.Remove(file_item);
            m_file_map_ref.Add(file_item, custom_info.ref_map);
            foreach (var pair in custom_info.ref_map)
            {
                if (pair.Value <= 0) continue;

                int count = 0;
                if (m_ref_map.TryGetValue(pair.Key, out count))
                {
                    m_ref_map.Remove(pair.Key);
                    count += pair.Value;
                }
                else
                    count = pair.Value;

                if (count > 0)
                    m_ref_map.Add(pair.Key, count);
            }
        }

        public override void RemoveAnalysis(FileItem file)
        {
            var file_item = file as AProtobufFileItem;
            if (file_item == null) return;

            if (!m_file_map_package.TryGetValue(file_item, out string package)) return;
            m_file_map_package.Remove(file_item);

            HashSet<string> set;
            if (m_import_cache_2.TryGetValue(file_item, out set))
            {
                foreach (var path in set)
                    m_import_cache_1.Remove(path);
                m_import_cache_2.Remove(file_item);
            }

            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null) return;

            var custom_info = file_item.GetCustomInfo();
            foreach (var name in custom_info.messages.Keys)
            {
                HashSet<AProtobufCustomInfo> file_set;
                if (refernce_info.m_message_info.TryGetValue(name, out file_set))
                {
                    file_set.Remove(custom_info);
                    if (file_set.Count == 0)
                        refernce_info.m_message_info.Remove(name);
                }
            }
            foreach (var name in custom_info.enums.Keys)
            {
                HashSet<AProtobufCustomInfo> file_set;
                if (refernce_info.m_enum_info.TryGetValue(name, out file_set))
                {
                    file_set.Remove(custom_info);
                    if (file_set.Count == 0)
                        refernce_info.m_enum_info.Remove(name);
                }
            }

            refernce_info.m_custom_info.Remove(file_item);
            if (refernce_info.m_custom_info.Count == 0)
                m_refrences.Remove(package);

            if (m_file_map_ref.TryGetValue(file_item, out Dictionary<string, int> map))
            {
                m_file_map_ref.Remove(file_item);
                foreach (var pair in map)
                {
                    if (pair.Value <= 0) continue;

                    int count = 0;
                    if (m_ref_map.TryGetValue(pair.Key, out count))
                    {
                        m_ref_map.Remove(pair.Key);
                        count -= pair.Value;
                    }

                    if (count > 0)
                        m_ref_map.Add(pair.Key, count);
                }
            }
        }

        public int GetReferenceCount(string name)
        {
            if (m_ref_map.TryGetValue(name, out int count))
                return count;
            return 0;
        }

        public AProtobufRefrenceInfo GetReferenceInfo(string package)
        {
            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null) return null;
            return refernce_info;
        }

        public HashSet<string> QueryFilePath(string text)
        {
            // 把正斜杠改为反斜杠
            text = text.Replace('/', '\\');

            var input = text;
            var pre_index = text.LastIndexOf('\\');
            if (pre_index >= 0)
                input = text.Substring(pre_index + 1);
            else
                pre_index = 0;

            var query_set = new HashSet<string>();
            foreach (var pair in m_file_map_package)
            {
                var index = 0;
                var full_path = pair.Key.GetFullPath();
                if (!full_path.StartsWith(text))
                {
                    index = full_path.LastIndexOf("\\" + text);
                    if (index < 0) continue;
                    index += 1;
                }

                var name = "";
                var start_index = index + (text.Length - input.Length);
                var end_index = full_path.IndexOf('\\', start_index);
                if (end_index < 0)
                    name = full_path.Substring(start_index);
                else
                    name = full_path.Substring(start_index, end_index - start_index);

                if (name.Length > 0 && name.StartsWith(input) && !query_set.Contains(name))
                    query_set.Add(name);
            }
            return query_set;
        }

        public AProtobufFileItem FindImportFile(string import_path)
        {
            // 转为大写
            import_path = import_path.ToUpper();
            // 把正斜杠改为反斜杠
            import_path = import_path.Replace('/', '\\');

            AProtobufFileItem result;
            if (m_import_cache_1.TryGetValue(import_path, out result))
                return result;

            foreach (var file in m_file_map_package.Keys)
            {
                if (file.GetFullPath().ToUpper().EndsWith(import_path))
                {
                    m_import_cache_1.Add(import_path, file);

                    HashSet<string> set;
                    if (!m_import_cache_2.TryGetValue(file, out set))
                    {
                        set = new HashSet<string>();
                        m_import_cache_2.Add(file, set);
                    }
                    set.Add(import_path);
                    return file;
                }   
            }
            return null;
        }

        // 查找对应的包名
        public List<string> MatchPackageList(string name)
        {
            List<string> list = new List<string>();
            foreach (var package_name in m_refrences.Keys)
            {
                if (package_name.StartsWith(name) && package_name != name)
                    list.Add(package_name);
            }
            return list;
        }
        
        public List<string> MatchMessageList(string package, string name)
        {
            List<string> list = new List<string>();

            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null) return list;

            foreach (var custom_info in refernce_info.m_custom_info)
            {
                var info = custom_info.Value;

                foreach (var message_name in info.messages.Keys)
                {
                    if (name.Length == 0 || message_name.StartsWith(name))
                        list.Add(message_name);
                }
            }

            return list;
        }

        public List<string> MatchEnumList(string package, string name)
        {
            List<string> list = new List<string>();

            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null) return list;

            foreach (var custom_info in refernce_info.m_custom_info)
            {
                var info = custom_info.Value;

                foreach (var enum_name in info.enums.Keys)
                {
                    if (name.Length == 0 || enum_name.StartsWith(name))
                        list.Add(enum_name);
                }
            }

            return list;
        }

        public List<string> MatchExtendList(string package, string name)
        {
            List<string> list = new List<string>();

            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null) return list;

            foreach (var custom_info in refernce_info.m_custom_info)
            {
                var info = custom_info.Value;

                foreach (var extend_name in info.extend_names.Keys)
                {
                    if (name.Length == 0 || extend_name.StartsWith(name))
                        list.Add(extend_name);
                }
            }

            return list;
        }

        public ABnfElement FindElementInAllPackage(string name)
        {
            foreach (var pair in m_refrences)
            {
                var element = FindElement(pair.Key, name);
                if (element != null) return element;
            }

            return null;
        }

        public ABnfElement FindElement(string package, string name)
        {
            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null) return null;

            if (refernce_info.m_message_info.TryGetValue(name, out HashSet<AProtobufCustomInfo> message_set))
            {
                foreach (var info in message_set)
                {
                    info.messages.TryGetValue(name, out AProtobufMessageInfo message_info);
                    if (message_info != null) return message_info.element;
                }
            }

            if (refernce_info.m_enum_info.TryGetValue(name, out HashSet<AProtobufCustomInfo> enum_set))
            {
                foreach (var info in enum_set)
                {
                    info.enums.TryGetValue(name, out AProtobufEnumInfo enum_info);
                    if (enum_info != null) return enum_info.element;
                }
            }

            return null;
        }

        public AProtobufEnumInfo FindEnumElementInfo(string package, string name)
        {
            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null) return null;

            if (refernce_info.m_enum_info.TryGetValue(name, out HashSet<AProtobufCustomInfo> enum_set))
            {
                foreach (var info in enum_set)
                {
                    info.enums.TryGetValue(name, out AProtobufEnumInfo enum_info);
                    if (enum_info != null) return enum_info;
                }
            }

            return null;
        }

        public ABnfElement FindExtendElement(string package, string name)
        {
            m_refrences.TryGetValue(package, out AProtobufRefrenceInfo refernce_info);
            if (refernce_info == null) return null;

            foreach (var custom_info in refernce_info.m_custom_info)
            {
                var info = custom_info.Value;

                info.extend_names.TryGetValue(name, out ABnfElement element);
                return element;
            }

            return null;
        }
    }
}
