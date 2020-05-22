
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufMessageVarElement : ABnfNodeElement
	{
		public AProtobufMessageVarElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_MessageVarModifier = false;
        private AProtobufMessageVarModifierElement m_cache_MessageVarModifier = null;
        public AProtobufMessageVarModifierElement GetMessageVarModifier()
        {
            if (m_flag_MessageVarModifier) return m_cache_MessageVarModifier;
            m_flag_MessageVarModifier = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufMessageVarModifierElement)
                {
                    m_cache_MessageVarModifier = child as AProtobufMessageVarModifierElement;
                    break;
                }
            }
            return m_cache_MessageVarModifier;
        }
        private bool m_flag_AllType = false;
        private AProtobufAllTypeElement m_cache_AllType = null;
        public AProtobufAllTypeElement GetAllType()
        {
            if (m_flag_AllType) return m_cache_AllType;
            m_flag_AllType = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufAllTypeElement)
                {
                    m_cache_AllType = child as AProtobufAllTypeElement;
                    break;
                }
            }
            return m_cache_AllType;
        }
        private bool m_flag_MessageVarName = false;
        private AProtobufMessageVarNameElement m_cache_MessageVarName = null;
        public AProtobufMessageVarNameElement GetMessageVarName()
        {
            if (m_flag_MessageVarName) return m_cache_MessageVarName;
            m_flag_MessageVarName = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufMessageVarNameElement)
                {
                    m_cache_MessageVarName = child as AProtobufMessageVarNameElement;
                    break;
                }
            }
            return m_cache_MessageVarName;
        }
        private bool m_flag_Number = false;
        private AProtobufNumberElement m_cache_Number = null;
        public AProtobufNumberElement GetNumber()
        {
            if (m_flag_Number) return m_cache_Number;
            m_flag_Number = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufNumberElement)
                {
                    m_cache_Number = child as AProtobufNumberElement;
                    break;
                }
            }
            return m_cache_Number;
        }
        private bool m_flag_MessageVarOption = false;
        private AProtobufMessageVarOptionElement m_cache_MessageVarOption = null;
        public AProtobufMessageVarOptionElement GetMessageVarOption()
        {
            if (m_flag_MessageVarOption) return m_cache_MessageVarOption;
            m_flag_MessageVarOption = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufMessageVarOptionElement)
                {
                    m_cache_MessageVarOption = child as AProtobufMessageVarOptionElement;
                    break;
                }
            }
            return m_cache_MessageVarOption;
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