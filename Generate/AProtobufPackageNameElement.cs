
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

        List<AProtobufPackageSplitNameElement> m_list_PackageSplitName = null;
        public List<AProtobufPackageSplitNameElement> GetPackageSplitNameList()
        {
            var list = new List<AProtobufPackageSplitNameElement>();
            if (m_list_PackageSplitName == null)
            {
                m_list_PackageSplitName = new List<AProtobufPackageSplitNameElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufPackageSplitNameElement)
                        m_list_PackageSplitName.Add(child as AProtobufPackageSplitNameElement);
                }   
            }
            list.AddRange(m_list_PackageSplitName);
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