
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufTextElement : ABnfNodeElement
	{
		public AProtobufTextElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Regex = false;
        private AProtobufRegexElement m_cache_Regex = null;
        public AProtobufRegexElement GetRegex()
        {
            if (m_flag_Regex) return m_cache_Regex;
            m_flag_Regex = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufRegexElement)
                {
                    m_cache_Regex = child as AProtobufRegexElement;
                    break;
                }
            }
            return m_cache_Regex;
        }

	}
}