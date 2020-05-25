
namespace ALittle
{
    public class AProtobufRegexReference : AProtobufReferenceTemplate<AProtobufRegexElement>
    {
        public AProtobufRegexReference(ABnfElement element) : base(element)
        {

        }

        public override bool CanGotoDefinition()
        {
            var parent = m_element.GetParent();
            if (parent == null) return false;
            return parent.GetReference().CanGotoDefinition();
        }
    }
}

