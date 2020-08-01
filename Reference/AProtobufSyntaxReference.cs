
namespace ALittle
{
    public class AProtobufSyntaxReference : AProtobufReferenceTemplate<AProtobufSyntaxElement>
    {
        public AProtobufSyntaxReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            // 检查最后的分号
            if (m_element.GetStringList().Count < 2)
                return new ABnfGuessError(m_element, "syntax语句必须以;结尾");

            var child = m_element.GetText();
            if (child == null) return null;

            var value = child.GetElementString();
            if (value == "proto2" || value == "proto3")
                return null;

            return new ABnfGuessError(child, "这里只能填写proto2或者proto3");
        }
    }
}

