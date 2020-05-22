
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufPackageElement : ABnfNodeElement
	{
		public AProtobufPackageElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_PackageName = false;
        private AProtobufPackageNameElement m_cache_PackageName = null;
        public AProtobufPackageNameElement GetPackageName()
        {
            if (m_flag_PackageName) return m_cache_PackageName;
            m_flag_PackageName = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufPackageNameElement)
                {
                    m_cache_PackageName = child as AProtobufPackageNameElement;
                    break;
                }
            }
            return m_cache_PackageName;
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