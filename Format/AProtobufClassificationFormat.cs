
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ALittle
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AProtobufKeyWord")]
    [Name("AProtobufKeyWord")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AProtobufKeyWordClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public AProtobufKeyWordClassificationFormatDefinition()
        {
            DisplayName = "AProtobuf关键字";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 86;
                color.G = 154;
                color.B = 214;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.Blue;
            }       
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AProtobufCustomName")]
    [Name("AProtobufCustomName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AProtobufCustomNameClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public AProtobufCustomNameClassificationFormatDefinition()
        {
            DisplayName = "AProtobuf消息枚举名";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 78;
                color.G = 201;
                color.B = 176;
                ForegroundColor = color;
            }
            else
            {
                var color = new Color();
                color.A = 0xFF;
                color.R = 0x21;
                color.G = 0x6F;
                color.B = 0x85;
                ForegroundColor = color;
            }
        } 
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AProtobufMessageVarName")]
    [Name("AProtobufMessageVarName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AProtobufMessageVarNameClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public AProtobufMessageVarNameClassificationFormatDefinition()
        {
            DisplayName = "AProtobuf消息字段名";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 189;
                color.G = 183;
                color.B = 107;
                ForegroundColor = color;
            }
            else
                ForegroundColor = Colors.Navy;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AProtobufEnumVarName")]
    [Name("AProtobufEnumVarName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AProtobufEnumVarNameClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public AProtobufEnumVarNameClassificationFormatDefinition()
        {
            DisplayName = "AProtobuf枚举字段名";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 185;
                color.G = 119;
                color.B = 30;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.DarkSlateGray;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AProtobufComment")]
    [Name("AProtobufComment")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AProtobufCommentClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public AProtobufCommentClassificationFormatDefinition()
        {
            DisplayName = "AProtobuf注释"; 
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 87;
                color.G = 166;
                color.B = 74;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.Green;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AProtobufText")]
    [Name("AProtobufText")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AProtobufTextClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public AProtobufTextClassificationFormatDefinition()
        {
            DisplayName = "AProtobuf字符串";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 214;
                color.G = 157; 
                color.B = 113;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.DarkRed;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AProtobufNumber")]
    [Name("AProtobufNumber")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AProtobufNumberClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public AProtobufNumberClassificationFormatDefinition()
        {
            DisplayName = "AProtobuf数字";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 181;
                color.G = 206;
                color.B = 168;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.Black;
            }
        }
    }
}
