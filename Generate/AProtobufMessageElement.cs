
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufMessageElement : ABnfNodeElement
	{
		public AProtobufMessageElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_MessageName = false;
        private AProtobufMessageNameElement m_cache_MessageName = null;
        public AProtobufMessageNameElement GetMessageName()
        {
            if (m_flag_MessageName) return m_cache_MessageName;
            m_flag_MessageName = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufMessageNameElement)
                {
                    m_cache_MessageName = child as AProtobufMessageNameElement;
                    break;
                }
            }
            return m_cache_MessageName;
        }
        private bool m_flag_MessageBody = false;
        private AProtobufMessageBodyElement m_cache_MessageBody = null;
        public AProtobufMessageBodyElement GetMessageBody()
        {
            if (m_flag_MessageBody) return m_cache_MessageBody;
            m_flag_MessageBody = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufMessageBodyElement)
                {
                    m_cache_MessageBody = child as AProtobufMessageBodyElement;
                    break;
                }
            }
            return m_cache_MessageBody;
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