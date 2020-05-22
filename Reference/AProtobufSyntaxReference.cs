
namespace ALittle
{
    public class AProtobufSyntaxReference : AProtobufReferenceTemplate<AProtobufSyntaxElement>
    {
        public AProtobufSyntaxReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            var child = m_element.GetText();
            if (child == null) return null;

            var value = child.GetElementString();
            if (value == "proto2" || value == "proto3")
                return null;

            return new ABnfGuessError(child, "这里只能填写proto2或者proto3");
        }
    }
}

