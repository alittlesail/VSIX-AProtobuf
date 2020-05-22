
namespace ALittle
{
    public class AProtobufMessageReference : AProtobufReferenceTemplate<AProtobufMessageElement>
    {
        public AProtobufMessageReference(ABnfElement element) : base(element) { }

        public override int GetDesiredIndentation(int offset, ABnfElement select)
        {
            ABnfElement parent = m_element.GetParent();
            if (parent is AProtobufMessageBodyElement)
            {
                return parent.GetReference().GetDesiredIndentation(offset, null);
            }
            return 0;
        }
    }
}
