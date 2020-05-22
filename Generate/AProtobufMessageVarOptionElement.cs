
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufMessageVarOptionElement : ABnfNodeElement
	{
		public AProtobufMessageVarOptionElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Id = false;
        private AProtobufIdElement m_cache_Id = null;
        public AProtobufIdElement GetId()
        {
            if (m_flag_Id) return m_cache_Id;
            m_flag_Id = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufIdElement)
                {
                    m_cache_Id = child as AProtobufIdElement;
                    break;
                }
            }
            return m_cache_Id;
        }
        private bool m_flag_MessageVarOptionValue = false;
        private AProtobufMessageVarOptionValueElement m_cache_MessageVarOptionValue = null;
        public AProtobufMessageVarOptionValueElement GetMessageVarOptionValue()
        {
            if (m_flag_MessageVarOptionValue) return m_cache_MessageVarOptionValue;
            m_flag_MessageVarOptionValue = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufMessageVarOptionValueElement)
                {
                    m_cache_MessageVarOptionValue = child as AProtobufMessageVarOptionValueElement;
                    break;
                }
            }
            return m_cache_MessageVarOptionValue;
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