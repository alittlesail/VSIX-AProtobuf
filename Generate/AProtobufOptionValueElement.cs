
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufOptionValueElement : ABnfNodeElement
	{
		public AProtobufOptionValueElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Const = false;
        private AProtobufConstElement m_cache_Const = null;
        public AProtobufConstElement GetConst()
        {
            if (m_flag_Const) return m_cache_Const;
            m_flag_Const = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufConstElement)
                {
                    m_cache_Const = child as AProtobufConstElement;
                    break;
                }
            }
            return m_cache_Const;
        }
        private bool m_flag_Id = false;
        private AProtobufIdElement m_cache_Id = null;
        public AProtobufIdElement GetId()
        {
            if (m_flag_Id) return m_cache_Id;
            m_flag_Id = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufIdElement)
                {
                    m_cache_Id = child as AProtobufIdElement;
                    break;
                }
            }
            return m_cache_Id;
        }

	}
}