
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufRepeatedTypeElement : ABnfNodeElement
	{
		public AProtobufRepeatedTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_PrimitiveType = false;
        private AProtobufPrimitiveTypeElement m_cache_PrimitiveType = null;
        public AProtobufPrimitiveTypeElement GetPrimitiveType()
        {
            if (m_flag_PrimitiveType) return m_cache_PrimitiveType;
            m_flag_PrimitiveType = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufPrimitiveTypeElement)
                {
                    m_cache_PrimitiveType = child as AProtobufPrimitiveTypeElement;
                    break;
                }
            }
            return m_cache_PrimitiveType;
        }
        private bool m_flag_CustomType = false;
        private AProtobufCustomTypeElement m_cache_CustomType = null;
        public AProtobufCustomTypeElement GetCustomType()
        {
            if (m_flag_CustomType) return m_cache_CustomType;
            m_flag_CustomType = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufCustomTypeElement)
                {
                    m_cache_CustomType = child as AProtobufCustomTypeElement;
                    break;
                }
            }
            return m_cache_CustomType;
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