
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufEnumElement : ABnfNodeElement
	{
		public AProtobufEnumElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_EnumName = false;
        private AProtobufEnumNameElement m_cache_EnumName = null;
        public AProtobufEnumNameElement GetEnumName()
        {
            if (m_flag_EnumName) return m_cache_EnumName;
            m_flag_EnumName = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufEnumNameElement)
                {
                    m_cache_EnumName = child as AProtobufEnumNameElement;
                    break;
                }
            }
            return m_cache_EnumName;
        }
        private bool m_flag_EnumBody = false;
        private AProtobufEnumBodyElement m_cache_EnumBody = null;
        public AProtobufEnumBodyElement GetEnumBody()
        {
            if (m_flag_EnumBody) return m_cache_EnumBody;
            m_flag_EnumBody = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufEnumBodyElement)
                {
                    m_cache_EnumBody = child as AProtobufEnumBodyElement;
                    break;
                }
            }
            return m_cache_EnumBody;
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

	}
}