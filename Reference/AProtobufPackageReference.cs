
using System.Collections.Generic;

namespace ALittle
{
    public class AProtobufPackageReference : AProtobufReferenceTemplate<AProtobufPackageElement>
    {
        public AProtobufPackageReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {   
            // 检查最后的分号
            if (m_element.GetString() == null)
                return new ABnfGuessError(m_element, "package语句必须以;结尾");
            return null;
        }
    }
}
