
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufPackageNameElement : ABnfNodeElement
	{
		public AProtobufPackageNameElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<AProtobufIdElement> m_list_Id = null;
        public List<AProtobufIdElement> GetIdList()
        {
            var list = new List<AProtobufIdElement>();
            if (m_list_Id == null)
            {
                m_list_Id = new List<AProtobufIdElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufIdElement)
                        m_list_Id.Add(child as AProtobufIdElement);
                }   
            }
            list.AddRange(m_list_Id);
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