
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufRootElement : ABnfNodeElement
	{
		public AProtobufRootElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Syntax = false;
        private AProtobufSyntaxElement m_cache_Syntax = null;
        public AProtobufSyntaxElement GetSyntax()
        {
            if (m_flag_Syntax) return m_cache_Syntax;
            m_flag_Syntax = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufSyntaxElement)
                {
                    m_cache_Syntax = child as AProtobufSyntaxElement;
                    break;
                }
            }
            return m_cache_Syntax;
        }
        List<AProtobufPackageElement> m_list_Package = null;
        public List<AProtobufPackageElement> GetPackageList()
        {
            var list = new List<AProtobufPackageElement>();
            if (m_list_Package == null)
            {
                m_list_Package = new List<AProtobufPackageElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufPackageElement)
                        m_list_Package.Add(child as AProtobufPackageElement);
                }   
            }
            list.AddRange(m_list_Package);
            return list;
        }
        List<AProtobufImportElement> m_list_Import = null;
        public List<AProtobufImportElement> GetImportList()
        {
            var list = new List<AProtobufImportElement>();
            if (m_list_Import == null)
            {
                m_list_Import = new List<AProtobufImportElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufImportElement)
                        m_list_Import.Add(child as AProtobufImportElement);
                }   
            }
            list.AddRange(m_list_Import);
            return list;
        }
        List<AProtobufExtendElement> m_list_Extend = null;
        public List<AProtobufExtendElement> GetExtendList()
        {
            var list = new List<AProtobufExtendElement>();
            if (m_list_Extend == null)
            {
                m_list_Extend = new List<AProtobufExtendElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufExtendElement)
                        m_list_Extend.Add(child as AProtobufExtendElement);
                }   
            }
            list.AddRange(m_list_Extend);
            return list;
        }
        List<AProtobufMessageElement> m_list_Message = null;
        public List<AProtobufMessageElement> GetMessageList()
        {
            var list = new List<AProtobufMessageElement>();
            if (m_list_Message == null)
            {
                m_list_Message = new List<AProtobufMessageElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufMessageElement)
                        m_list_Message.Add(child as AProtobufMessageElement);
                }   
            }
            list.AddRange(m_list_Message);
            return list;
        }
        List<AProtobufEnumElement> m_list_Enum = null;
        public List<AProtobufEnumElement> GetEnumList()
        {
            var list = new List<AProtobufEnumElement>();
            if (m_list_Enum == null)
            {
                m_list_Enum = new List<AProtobufEnumElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufEnumElement)
                        m_list_Enum.Add(child as AProtobufEnumElement);
                }   
            }
            list.AddRange(m_list_Enum);
            return list;
        }
        List<AProtobufOptionElement> m_list_Option = null;
        public List<AProtobufOptionElement> GetOptionList()
        {
            var list = new List<AProtobufOptionElement>();
            if (m_list_Option == null)
            {
                m_list_Option = new List<AProtobufOptionElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufOptionElement)
                        m_list_Option.Add(child as AProtobufOptionElement);
                }   
            }
            list.AddRange(m_list_Option);
            return list;
        }
        List<AProtobufServiceElement> m_list_Service = null;
        public List<AProtobufServiceElement> GetServiceList()
        {
            var list = new List<AProtobufServiceElement>();
            if (m_list_Service == null)
            {
                m_list_Service = new List<AProtobufServiceElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufServiceElement)
                        m_list_Service.Add(child as AProtobufServiceElement);
                }   
            }
            list.AddRange(m_list_Service);
            return list;
        }

	}
}