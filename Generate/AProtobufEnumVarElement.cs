
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufEnumVarElement : ABnfNodeElement
	{
		public AProtobufEnumVarElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_EnumVarName = false;
        private AProtobufEnumVarNameElement m_cache_EnumVarName = null;
        public AProtobufEnumVarNameElement GetEnumVarName()
        {
            if (m_flag_EnumVarName) return m_cache_EnumVarName;
            m_flag_EnumVarName = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufEnumVarNameElement)
                {
                    m_cache_EnumVarName = child as AProtobufEnumVarNameElement;
                    break;
                }
            }
            return m_cache_EnumVarName;
        }
        private bool m_flag_Number = false;
        private AProtobufNumberElement m_cache_Number = null;
        public AProtobufNumberElement GetNumber()
        {
            if (m_flag_Number) return m_cache_Number;
            m_flag_Number = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufNumberElement)
                {
                    m_cache_Number = child as AProtobufNumberElement;
                    break;
                }
            }
            return m_cache_Number;
        }
        List<AProtobufStringElement> m_list_String = null;
        public List<AProtobufStringElement> GetStringList()
        {
            var list = new List<AProtobufStringElement>();
            if (m_list_String == null)
            {
                m_list_String = new List<AProtobufStringElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufStringElement)
                        m_list_String.Add(child as AProtobufStringElement);
                }   
            }
            list.AddRange(m_list_String);
            return list;
        }

	}
}