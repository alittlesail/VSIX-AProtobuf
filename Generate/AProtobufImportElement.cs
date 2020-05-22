
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufImportElement : ABnfNodeElement
	{
		public AProtobufImportElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Text = false;
        private AProtobufTextElement m_cache_Text = null;
        public AProtobufTextElement GetText()
        {
            if (m_flag_Text) return m_cache_Text;
            m_flag_Text = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufTextElement)
                {
                    m_cache_Text = child as AProtobufTextElement;
                    break;
                }
            }
            return m_cache_Text;
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
        private bool m_flag_String = false;
        private AProtobufStringElement m_cache_String = null;
        public AProtobufStringElement GetString()
        {
            if (m_flag_String) return m_cache_String;
            m_flag_String = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufStringElement)
                {
                    m_cache_String = child as AProtobufStringElement;
                    break;
                }
            }
            return m_cache_String;
        }

	}
}