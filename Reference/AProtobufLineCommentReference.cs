
namespace ALittle
{
    public class AProtobufLineCommentReference : AProtobufReferenceTemplate<AProtobufLineCommentElement>
    {
        public AProtobufLineCommentReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufComment";
        }
    }
}

