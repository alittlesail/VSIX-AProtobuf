
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufServiceElement : ABnfNodeElement
	{
		public AProtobufServiceElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ServiceName = false;
        private AProtobufServiceNameElement m_cache_ServiceName = null;
        public AProtobufServiceNameElement GetServiceName()
        {
            if (m_flag_ServiceName) return m_cache_ServiceName;
            m_flag_ServiceName = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufServiceNameElement)
                {
                    m_cache_ServiceName = child as AProtobufServiceNameElement;
                    break;
                }
            }
            return m_cache_ServiceName;
        }
        private bool m_flag_ServiceBody = false;
        private AProtobufServiceBodyElement m_cache_ServiceBody = null;
        public AProtobufServiceBodyElement GetServiceBody()
        {
            if (m_flag_ServiceBody) return m_cache_ServiceBody;
            m_flag_ServiceBody = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufServiceBodyElement)
                {
                    m_cache_ServiceBody = child as AProtobufServiceBodyElement;
                    break;
                }
            }
            return m_cache_ServiceBody;
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