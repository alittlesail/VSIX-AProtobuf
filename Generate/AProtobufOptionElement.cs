
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufOptionElement : ABnfNodeElement
	{
		public AProtobufOptionElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        private bool m_flag_OptionValue = false;
        private AProtobufOptionValueElement m_cache_OptionValue = null;
        public AProtobufOptionValueElement GetOptionValue()
        {
            if (m_flag_OptionValue) return m_cache_OptionValue;
            m_flag_OptionValue = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufOptionValueElement)
                {
                    m_cache_OptionValue = child as AProtobufOptionValueElement;
                    break;
                }
            }
            return m_cache_OptionValue;
        }
        private bool m_flag_Key = false;
        private AProtobufKeyElement m_cache_Key = null;
        public AProtobufKeyElement GetKey()
        {
            if (m_flag_Key) return m_cache_Key;
            m_flag_Key = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufKeyElement)
                {
                    m_cache_Key = child as AProtobufKeyElement;
                    break;
                }
            }
            return m_cache_Key;
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