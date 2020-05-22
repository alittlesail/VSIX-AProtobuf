

namespace ALittle
{
    public class AProtobufOneofNameReference : AProtobufReferenceTemplate<AProtobufOneofNameElement>
    {
        public AProtobufOneofNameReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufCustomName";
        }
    }
}

