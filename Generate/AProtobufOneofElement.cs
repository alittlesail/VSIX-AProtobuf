
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufOneofElement : ABnfNodeElement
	{
		public AProtobufOneofElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_OneofName = false;
        private AProtobufOneofNameElement m_cache_OneofName = null;
        public AProtobufOneofNameElement GetOneofName()
        {
            if (m_flag_OneofName) return m_cache_OneofName;
            m_flag_OneofName = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufOneofNameElement)
                {
                    m_cache_OneofName = child as AProtobufOneofNameElement;
                    break;
                }
            }
            return m_cache_OneofName;
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