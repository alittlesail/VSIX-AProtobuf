

namespace ALittle
{
    public class AProtobufServiceRpcNameReference : AProtobufReferenceTemplate<AProtobufServiceRpcNameElement>
    {
        public AProtobufServiceRpcNameReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufCustomName";
        }
    }
}

