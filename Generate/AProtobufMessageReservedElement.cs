
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufMessageReservedElement : ABnfNodeElement
	{
		public AProtobufMessageReservedElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<AProtobufMessageReservedValueElement> m_list_MessageReservedValue = null;
        public List<AProtobufMessageReservedValueElement> GetMessageReservedValueList()
        {
            var list = new List<AProtobufMessageReservedValueElement>();
            if (m_list_MessageReservedValue == null)
            {
                m_list_MessageReservedValue = new List<AProtobufMessageReservedValueElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufMessageReservedValueElement)
                        m_list_MessageReservedValue.Add(child as AProtobufMessageReservedValueElement);
                }   
            }
            list.AddRange(m_list_MessageReservedValue);
            return list;
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