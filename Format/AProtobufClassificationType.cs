
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ALittle
{
    internal static class AProtobufClassificationTypeDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AProtobufKeyWord")]
        internal static ClassificationTypeDefinition APROTOBUFKEYWORD = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AProtobufCustomName")]
        internal static ClassificationTypeDefinition APROTOBUFCUSTOMNAME = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AProtobufMessageVarName")]
        internal static ClassificationTypeDefinition APROTOBUFMESSAGEVARNAME = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AProtobufEnumVarName")]
        internal static ClassificationTypeDefinition APROTOBUFENUMVARNAME = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AProtobufComment")]
        internal static ClassificationTypeDefinition APROTOBUFCOMMENT = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AProtobufText")]
        internal static ClassificationTypeDefinition APROTOBUFTEXT = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AProtobufNumber")]
        internal static ClassificationTypeDefinition APROTOBUFNUMBER = null;
    }
}
