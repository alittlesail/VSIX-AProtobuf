
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufServiceRpcBodyElement : ABnfNodeElement
	{
		public AProtobufServiceRpcBodyElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<AProtobufServiceOptionElement> m_list_ServiceOption = null;
        public List<AProtobufServiceOptionElement> GetServiceOptionList()
        {
            var list = new List<AProtobufServiceOptionElement>();
            if (m_list_ServiceOption == null)
            {
                m_list_ServiceOption = new List<AProtobufServiceOptionElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufServiceOptionElement)
                        m_list_ServiceOption.Add(child as AProtobufServiceOptionElement);
                }   
            }
            list.AddRange(m_list_ServiceOption);
            return list;
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