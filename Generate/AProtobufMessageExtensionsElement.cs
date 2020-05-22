
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufMessageExtensionsElement : ABnfNodeElement
	{
		public AProtobufMessageExtensionsElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<AProtobufNumberElement> m_list_Number = null;
        public List<AProtobufNumberElement> GetNumberList()
        {
            var list = new List<AProtobufNumberElement>();
            if (m_list_Number == null)
            {
                m_list_Number = new List<AProtobufNumberElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufNumberElement)
                        m_list_Number.Add(child as AProtobufNumberElement);
                }   
            }
            list.AddRange(m_list_Number);
            return list;
        }
        List<AProtobufKeyElement> m_list_Key = null;
        public List<AProtobufKeyElement> GetKeyList()
        {
            var list = new List<AProtobufKeyElement>();
            if (m_list_Key == null)
            {
                m_list_Key = new List<AProtobufKeyElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufKeyElement)
                        m_list_Key.Add(child as AProtobufKeyElement);
                }   
            }
            list.AddRange(m_list_Key);
            return list;
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