
namespace ALittle
{
    public class AProtobufMessageReference : AProtobufReferenceTemplate<AProtobufMessageElement>
    {
        public AProtobufMessageReference(ABnfElement element) : base(element) { }

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
            if (m_element.GetMessageName() == null)
                return new ABnfGuessError(m_element, "没有定义协议名");

            if (m_element.GetMessageBody() == null)
                return new ABnfGuessError(m_element, "没有定义协议内容");
            return null;
		}
	}
}
