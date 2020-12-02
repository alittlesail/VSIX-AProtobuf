
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufServiceBodyElement : ABnfNodeElement
	{
		public AProtobufServiceBodyElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<AProtobufServiceRpcElement> m_list_ServiceRpc = null;
        public List<AProtobufServiceRpcElement> GetServiceRpcList()
        {
            var list = new List<AProtobufServiceRpcElement>();
            if (m_list_ServiceRpc == null)
            {
                m_list_ServiceRpc = new List<AProtobufServiceRpcElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufServiceRpcElement)
                        m_list_ServiceRpc.Add(child as AProtobufServiceRpcElement);
                }   
            }
            list.AddRange(m_list_ServiceRpc);
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