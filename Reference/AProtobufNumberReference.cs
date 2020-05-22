
namespace ALittle
{
    public class AProtobufNumberReference : AProtobufReferenceTemplate<AProtobufNumberElement>
    {
        public AProtobufNumberReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufNumber";
        }
    }
}

