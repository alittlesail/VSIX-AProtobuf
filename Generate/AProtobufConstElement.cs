
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufConstElement : ABnfNodeElement
	{
		public AProtobufConstElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        private bool m_flag_Bool = false;
        private AProtobufBoolElement m_cache_Bool = null;
        public AProtobufBoolElement GetBool()
        {
            if (m_flag_Bool) return m_cache_Bool;
            m_flag_Bool = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufBoolElement)
                {
                    m_cache_Bool = child as AProtobufBoolElement;
                    break;
                }
            }
            return m_cache_Bool;
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

	}
}