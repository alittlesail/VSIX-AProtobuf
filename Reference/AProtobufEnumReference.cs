
namespace ALittle
{
    public class AProtobufEnumReference : AProtobufReferenceTemplate<AProtobufEnumElement>
    {
        public AProtobufEnumReference(ABnfElement element) : base(element) { }

        public override int GetDesiredIndentation(int offset, ABnfElement select)
        {
            ABnfElement parent = m_element.GetParent();
            if (parent is AProtobufMessageBodyElement)
                return parent.GetReference().GetDesiredIndentation(offset, null);
            return 0;
        }

        public override int GetFormateIndentation(int offset, ABnfElement select)
        {
            ABnfElement parent = m_element.GetParent();
            if (parent is AProtobufMessageBodyElement)
                return parent.GetReference().GetFormateIndentation(offset, null);
            return 0;
        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetEnumName() == null)
                return new ABnfGuessError(m_element, "没有定义枚举名");

            if (m_element.GetEnumBody() == null)
                return new ABnfGuessError(m_element, "没有定义协议内容");
            return null;
        }
    }
}

