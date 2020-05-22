
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufAllTypeElement : ABnfNodeElement
	{
		public AProtobufAllTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_RepeatedType = false;
        private AProtobufRepeatedTypeElement m_cache_RepeatedType = null;
        public AProtobufRepeatedTypeElement GetRepeatedType()
        {
            if (m_flag_RepeatedType) return m_cache_RepeatedType;
            m_flag_RepeatedType = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufRepeatedTypeElement)
                {
                    m_cache_RepeatedType = child as AProtobufRepeatedTypeElement;
                    break;
                }
            }
            return m_cache_RepeatedType;
        }
        private bool m_flag_MapType = false;
        private AProtobufMapTypeElement m_cache_MapType = null;
        public AProtobufMapTypeElement GetMapType()
        {
            if (m_flag_MapType) return m_cache_MapType;
            m_flag_MapType = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufMapTypeElement)
                {
                    m_cache_MapType = child as AProtobufMapTypeElement;
                    break;
                }
            }
            return m_cache_MapType;
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

	}
}