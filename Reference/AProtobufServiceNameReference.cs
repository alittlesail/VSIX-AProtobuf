

namespace ALittle
{
    public class AProtobufServiceNameReference : AProtobufReferenceTemplate<AProtobufServiceNameElement>
    {
        public AProtobufServiceNameReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufCustomName";
        }
    }
}

