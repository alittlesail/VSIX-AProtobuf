
namespace ALittle
{
    public class AProtobufBlockCommentReference : AProtobufReferenceTemplate<AProtobufBlockCommentElement>
    {
        public AProtobufBlockCommentReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufComment";
        }

        public override bool CanGotoDefinition()
        {
            return false;
        }
    }
}
