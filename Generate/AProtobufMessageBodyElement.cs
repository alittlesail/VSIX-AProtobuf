
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufMessageBodyElement : ABnfNodeElement
	{
		public AProtobufMessageBodyElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<AProtobufEnumElement> m_list_Enum = null;
        public List<AProtobufEnumElement> GetEnumList()
        {
            var list = new List<AProtobufEnumElement>();
            if (m_list_Enum == null)
            {
                m_list_Enum = new List<AProtobufEnumElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufEnumElement)
                        m_list_Enum.Add(child as AProtobufEnumElement);
                }   
            }
            list.AddRange(m_list_Enum);
            return list;
        }
        List<AProtobufMessageElement> m_list_Message = null;
        public List<AProtobufMessageElement> GetMessageList()
        {
            var list = new List<AProtobufMessageElement>();
            if (m_list_Message == null)
            {
                m_list_Message = new List<AProtobufMessageElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufMessageElement)
                        m_list_Message.Add(child as AProtobufMessageElement);
                }   
            }
            list.AddRange(m_list_Message);
            return list;
        }
        List<AProtobufOneofElement> m_list_Oneof = null;
        public List<AProtobufOneofElement> GetOneofList()
        {
            var list = new List<AProtobufOneofElement>();
            if (m_list_Oneof == null)
            {
                m_list_Oneof = new List<AProtobufOneofElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufOneofElement)
                        m_list_Oneof.Add(child as AProtobufOneofElement);
                }   
            }
            list.AddRange(m_list_Oneof);
            return list;
        }
        List<AProtobufMessageOptionElement> m_list_MessageOption = null;
        public List<AProtobufMessageOptionElement> GetMessageOptionList()
        {
            var list = new List<AProtobufMessageOptionElement>();
            if (m_list_MessageOption == null)
            {
                m_list_MessageOption = new List<AProtobufMessageOptionElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufMessageOptionElement)
                        m_list_MessageOption.Add(child as AProtobufMessageOptionElement);
                }   
            }
            list.AddRange(m_list_MessageOption);
            return list;
        }
        List<AProtobufMessageExtensionsElement> m_list_MessageExtensions = null;
        public List<AProtobufMessageExtensionsElement> GetMessageExtensionsList()
        {
            var list = new List<AProtobufMessageExtensionsElement>();
            if (m_list_MessageExtensions == null)
            {
                m_list_MessageExtensions = new List<AProtobufMessageExtensionsElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufMessageExtensionsElement)
                        m_list_MessageExtensions.Add(child as AProtobufMessageExtensionsElement);
                }   
            }
            list.AddRange(m_list_MessageExtensions);
            return list;
        }
        List<AProtobufMessageReservedElement> m_list_MessageReserved = null;
        public List<AProtobufMessageReservedElement> GetMessageReservedList()
        {
            var list = new List<AProtobufMessageReservedElement>();
            if (m_list_MessageReserved == null)
            {
                m_list_MessageReserved = new List<AProtobufMessageReservedElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufMessageReservedElement)
                        m_list_MessageReserved.Add(child as AProtobufMessageReservedElement);
                }   
            }
            list.AddRange(m_list_MessageReserved);
            return list;
        }
        List<AProtobufMessageVarElement> m_list_MessageVar = null;
        public List<AProtobufMessageVarElement> GetMessageVarList()
        {
            var list = new List<AProtobufMessageVarElement>();
            if (m_list_MessageVar == null)
            {
                m_list_MessageVar = new List<AProtobufMessageVarElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufMessageVarElement)
                        m_list_MessageVar.Add(child as AProtobufMessageVarElement);
                }   
            }
            list.AddRange(m_list_MessageVar);
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