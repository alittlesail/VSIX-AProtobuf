
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ALittle
{
    public class AProtobufFactory : ABnfFactory
    {   
        Dictionary<string, Func<ABnfFactory, ABnfFile, int, int, int, string, ABnfNodeElement>> m_create_map = new Dictionary<string, Func<ABnfFactory, ABnfFile, int, int, int, string, ABnfNodeElement>>();

        public AProtobufFactory()
        {
            m_create_map["Root"] = (factory, file, line, col, offset, type) => { return new AProtobufRootElement(factory, file, line, col, offset, type); };
            m_create_map["LineComment"] = (factory, file, line, col, offset, type) => { return new AProtobufLineCommentElement(factory, file, line, col, offset, type); };
            m_create_map["BlockComment"] = (factory, file, line, col, offset, type) => { return new AProtobufBlockCommentElement(factory, file, line, col, offset, type); };
            m_create_map["Text"] = (factory, file, line, col, offset, type) => { return new AProtobufTextElement(factory, file, line, col, offset, type); };
            m_create_map["Id"] = (factory, file, line, col, offset, type) => { return new AProtobufIdElement(factory, file, line, col, offset, type); };
            m_create_map["Number"] = (factory, file, line, col, offset, type) => { return new AProtobufNumberElement(factory, file, line, col, offset, type); };
            m_create_map["Bool"] = (factory, file, line, col, offset, type) => { return new AProtobufBoolElement(factory, file, line, col, offset, type); };
            m_create_map["Const"] = (factory, file, line, col, offset, type) => { return new AProtobufConstElement(factory, file, line, col, offset, type); };
            m_create_map["Syntax"] = (factory, file, line, col, offset, type) => { return new AProtobufSyntaxElement(factory, file, line, col, offset, type); };
            m_create_map["Package"] = (factory, file, line, col, offset, type) => { return new AProtobufPackageElement(factory, file, line, col, offset, type); };
            m_create_map["PackageName"] = (factory, file, line, col, offset, type) => { return new AProtobufPackageNameElement(factory, file, line, col, offset, type); };
            m_create_map["PackageSplitName"] = (factory, file, line, col, offset, type) => { return new AProtobufPackageSplitNameElement(factory, file, line, col, offset, type); };
            m_create_map["Import"] = (factory, file, line, col, offset, type) => { return new AProtobufImportElement(factory, file, line, col, offset, type); };
            m_create_map["Option"] = (factory, file, line, col, offset, type) => { return new AProtobufOptionElement(factory, file, line, col, offset, type); };
            m_create_map["OptionValue"] = (factory, file, line, col, offset, type) => { return new AProtobufOptionValueElement(factory, file, line, col, offset, type); };
            m_create_map["CustomType"] = (factory, file, line, col, offset, type) => { return new AProtobufCustomTypeElement(factory, file, line, col, offset, type); };
            m_create_map["PrimitiveType"] = (factory, file, line, col, offset, type) => { return new AProtobufPrimitiveTypeElement(factory, file, line, col, offset, type); };
            m_create_map["RepeatedType"] = (factory, file, line, col, offset, type) => { return new AProtobufRepeatedTypeElement(factory, file, line, col, offset, type); };
            m_create_map["MapType"] = (factory, file, line, col, offset, type) => { return new AProtobufMapTypeElement(factory, file, line, col, offset, type); };
            m_create_map["AllType"] = (factory, file, line, col, offset, type) => { return new AProtobufAllTypeElement(factory, file, line, col, offset, type); };
            m_create_map["Extend"] = (factory, file, line, col, offset, type) => { return new AProtobufExtendElement(factory, file, line, col, offset, type); };
            m_create_map["Oneof"] = (factory, file, line, col, offset, type) => { return new AProtobufOneofElement(factory, file, line, col, offset, type); };
            m_create_map["OneofName"] = (factory, file, line, col, offset, type) => { return new AProtobufOneofNameElement(factory, file, line, col, offset, type); };
            m_create_map["Message"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageElement(factory, file, line, col, offset, type); };
            m_create_map["MessageName"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageNameElement(factory, file, line, col, offset, type); };
            m_create_map["MessageBody"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageBodyElement(factory, file, line, col, offset, type); };
            m_create_map["MessageVarModifier"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageVarModifierElement(factory, file, line, col, offset, type); };
            m_create_map["MessageVar"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageVarElement(factory, file, line, col, offset, type); };
            m_create_map["MessageVarName"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageVarNameElement(factory, file, line, col, offset, type); };
            m_create_map["MessageVarOption"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageVarOptionElement(factory, file, line, col, offset, type); };
            m_create_map["MessageVarOptionValue"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageVarOptionValueElement(factory, file, line, col, offset, type); };
            m_create_map["MessageOption"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageOptionElement(factory, file, line, col, offset, type); };
            m_create_map["MessageExtensions"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageExtensionsElement(factory, file, line, col, offset, type); };
            m_create_map["MessageReserved"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageReservedElement(factory, file, line, col, offset, type); };
            m_create_map["MessageReservedValue"] = (factory, file, line, col, offset, type) => { return new AProtobufMessageReservedValueElement(factory, file, line, col, offset, type); };
            m_create_map["Enum"] = (factory, file, line, col, offset, type) => { return new AProtobufEnumElement(factory, file, line, col, offset, type); };
            m_create_map["EnumBody"] = (factory, file, line, col, offset, type) => { return new AProtobufEnumBodyElement(factory, file, line, col, offset, type); };
            m_create_map["EnumName"] = (factory, file, line, col, offset, type) => { return new AProtobufEnumNameElement(factory, file, line, col, offset, type); };
            m_create_map["EnumVar"] = (factory, file, line, col, offset, type) => { return new AProtobufEnumVarElement(factory, file, line, col, offset, type); };
            m_create_map["EnumVarName"] = (factory, file, line, col, offset, type) => { return new AProtobufEnumVarNameElement(factory, file, line, col, offset, type); };
            m_create_map["Service"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceElement(factory, file, line, col, offset, type); };
            m_create_map["ServiceBody"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceBodyElement(factory, file, line, col, offset, type); };
            m_create_map["ServiceName"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceNameElement(factory, file, line, col, offset, type); };
            m_create_map["ServiceRpc"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceRpcElement(factory, file, line, col, offset, type); };
            m_create_map["ServiceRpcName"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceRpcNameElement(factory, file, line, col, offset, type); };
            m_create_map["ServiceRpcReq"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceRpcReqElement(factory, file, line, col, offset, type); };
            m_create_map["ServiceRpcRsp"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceRpcRspElement(factory, file, line, col, offset, type); };
            m_create_map["ServiceRpcBody"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceRpcBodyElement(factory, file, line, col, offset, type); };
            m_create_map["ServiceOption"] = (factory, file, line, col, offset, type) => { return new AProtobufServiceOptionElement(factory, file, line, col, offset, type); };

        }

        public override ABnfNodeElement CreateNodeElement(ABnfFile file, int line, int col, int offset, string type)
        {
            Func<ABnfFactory, ABnfFile, int, int, int, string, ABnfNodeElement> func;
            if (!m_create_map.TryGetValue(type, out func)) return null;
            return func(this, file, line, col, offset, type);
        }

        public override ABnfKeyElement CreateKeyElement(ABnfFile file, int line, int col, int offset, string type)
        {
            return new AProtobufKeyElement(this, file, line, col, offset, type);
        }

        public override ABnfStringElement CreateStringElement(ABnfFile file, int line, int col, int offset, string type)
        {
            return new AProtobufStringElement(this, file, line, col, offset, type);
        }

        public override ABnfRegexElement CreateRegexElement(ABnfFile file, int line, int col, int offset, string type, Regex regex)
        {
            return new AProtobufRegexElement(this, file, line, col, offset, type, regex);
        }
    }
}

