
namespace ALittle
{
    public class AProtobufServiceReference : AProtobufReferenceTemplate<AProtobufServiceElement>
    {
        public AProtobufServiceReference(ABnfElement element) : base(element) { }

		public override ABnfGuessError CheckError()
		{
            if (m_element.GetServiceName() == null)
                return new ABnfGuessError(m_element, "没有定义协议名");

            if (m_element.GetServiceBody() == null)
                return new ABnfGuessError(m_element, "没有定义协议内容");
            return null;
		}
	}
}
