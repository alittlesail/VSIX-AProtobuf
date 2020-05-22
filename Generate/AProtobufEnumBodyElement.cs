
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufEnumBodyElement : ABnfNodeElement
	{
		public AProtobufEnumBodyElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<AProtobufEnumVarElement> m_list_EnumVar = null;
        public List<AProtobufEnumVarElement> GetEnumVarList()
        {
            var list = new List<AProtobufEnumVarElement>();
            if (m_list_EnumVar == null)
            {
                m_list_EnumVar = new List<AProtobufEnumVarElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufEnumVarElement)
                        m_list_EnumVar.Add(child as AProtobufEnumVarElement);
                }   
            }
            list.AddRange(m_list_EnumVar);
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