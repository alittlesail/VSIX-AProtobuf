
namespace ALittle
{
    public class AProtobufKeyReference : AProtobufReferenceTemplate<AProtobufKeyElement>
    {
        public AProtobufKeyReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "AProtobufKeyWord";
        }

        public override bool CanGotoDefinition()
        {
            return false;
        }
    }
}

