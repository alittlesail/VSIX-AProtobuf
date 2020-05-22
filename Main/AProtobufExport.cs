
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Tagging;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Classification;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;

namespace ALittle
{
    // 文件绑定
    public class AProtobufContentTypeDefinition
    {
        [Export]
        [Name("proto")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition AProtobufContentType = null;

        [Export]
        [FileExtension(".proto")]
        [ContentType("proto")]
        internal static FileExtensionToContentTypeDefinition AProtobufFileType = null;
    }

    // 编辑器管理
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("proto")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    public class AProtobufVsTextViewCreationListener : ALanguageVsTextViewCreationListener
    {
        public AProtobufVsTextViewCreationListener()
        {
            m_factory = AProtobufFactoryClass.inst;
        }
    }

    // 预测列表
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("proto")]
    [Name("AProtobufCompletionSourceProvider")]
    public sealed class AProtobufCompletionSourceProvider : ALanguageCompletionSourceProvider { }

    // 预览定义
    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("AProtobufControllerProvider")]
    [ContentType("proto")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.Analyzable)]
    public class AProtobufControllerProvider : ALanguageControllerProvider { }

    [Export(typeof(IQuickInfoSourceProvider))]
    [ContentType("proto")]
    [Name("AProtobufQuickInfoSourceProvider")]
    public class AProtobufQuickInfoSourceProvider : ALanguageQuickInfoSourceProvider { }

    // 配色
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("proto")]
    [TagType(typeof(ClassificationTag))]
    public class AProtobufClassifierProvider : ALanguageClassifierProvider { public AProtobufClassifierProvider() : base("AProtobufGotoDefinition") { } }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AProtobufGotoDefinition")]
    [Name("AProtobufGotoDefinition")]
    [Order(After = Priority.High)]
    public class AProtobufGotoDefinitionClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public AProtobufGotoDefinitionClassificationFormatDefinition()
        {
            this.DisplayName = "AProtobufGotoDefinition";
            this.TextDecorations = System.Windows.TextDecorations.Underline;

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
                this.ForegroundColor = Colors.Blue;
            }
        }
    }

    public class ALanguageClassificationDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AProtobufGotoDefinition")]
        internal static ClassificationTypeDefinition APROTOBUFGOTODEFINITION = null;
    }

    // 缩进
    [Export(typeof(ISmartIndentProvider))]
    [ContentType("proto")]
    public class AProtobufSmartIndentProvider : ALanguageSmartIndentProvider { }

    // 错误元素提示
    [Export(typeof(IViewTaggerProvider))]
    [TagType(typeof(IErrorTag))]
    [ContentType("proto")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.Analyzable)]
    public class AProtobufErrorTaggerProvider : ALanguageErrorTaggerProvider { }

    // 引用个数提示
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("proto")]
    [TagType(typeof(IntraTextAdornmentTag))]
    public class AProtobufReferenceTaggerProvider : IViewTaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextView view, ITextBuffer buffer) where T : ITag
        {
            if (!view.Properties.TryGetProperty(nameof(ALanguageReferenceTagger), out ALanguageReferenceTagger tagger))
            {
                tagger = new ALanguageReferenceTagger(view);
                view.Properties.AddProperty(nameof(ALanguageReferenceTagger), tagger);
            }
            return tagger as ITagger<T>;
        }
    }

    // 高亮元素提示
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("proto")]
    [TagType(typeof(TextMarkerTag))]
    public class AProtobufHighlightWordTaggerProvider : ALanguageHighlightWordTaggerProvider { }

    [Export(typeof(EditorFormatDefinition))]
    [Name("AProtobufHighlightWordFormatDefinition")]
    [UserVisible(true)]
    public class AProtobufHighlightWordFormatDefinition : MarkerFormatDefinition
    {
        public AProtobufHighlightWordFormatDefinition()
        {
            DisplayName = "AProtobuf高亮";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 14;
                color.G = 69;
                color.B = 131;
                BackgroundColor = color;

                color = new Color();
                color.A = 255;
                color.R = 173;
                color.G = 192;
                color.B = 211;
                ForegroundColor = color;
            }
            else
            {
                BackgroundColor = Colors.LightBlue;
            }
        }
    }

    public class AProtobufHighlightWordTag : TextMarkerTag
    {
        public AProtobufHighlightWordTag() : base("AProtobufHighlightWordFormatDefinition") { }
    }

    // 鼠标按键处理
    [Export(typeof(IMouseProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.EmbeddedPeekTextView)]
    [ContentType("proto")]
    [Name("AProtobufGoToDefinitionMouseHandlerProvider")]
    [Order(Before = "WordSelection")]
    public class AProtobufGoToDefinitionMouseHandlerProvider : ALanguageGoToDefinitionMouseHandlerProvider { }

    // 键盘按键处理
    [Export(typeof(IKeyProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.EmbeddedPeekTextView)]
    [ContentType("proto")]
    [Name("AProtobufGoToDefinitionKeyProcessorProvider")]
    [Order(Before = "VisualStudioKeyboardProcessor")]
    public class AProtobufGoToDefinitionKeyProcessorProvider : ALanguageGoToDefinitionKeyProcessorProvider { }
}

