
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufPrimitiveTypeElement : ABnfNodeElement
	{
		public AProtobufPrimitiveTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
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