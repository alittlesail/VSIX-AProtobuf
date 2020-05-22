using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Task = System.Threading.Tasks.Task;

namespace ALittle
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class FastGotoCommand
    {
        private IVsTextManager m_text_manager;
        private IVsEditorAdaptersFactoryService m_adapters_factory;
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0101;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("c1e6da4c-016f-4d54-a612-b135ffefcea7");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastGotoCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private FastGotoCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static FastGotoCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in FastGotoCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new FastGotoCommand(package, commandService);

            Instance.m_text_manager = await package.GetServiceAsync(typeof(SVsTextManager)) as IVsTextManager;

            var model = await package.GetServiceAsync(typeof(SComponentModel)) as IComponentModel;
            if (model != null)
                Instance.m_adapters_factory = model.GetService<IVsEditorAdaptersFactoryService>();
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solution = AProtobufFactoryClass.inst.GetSolution();
            if (solution == null) return;
            var server = solution.GetServer();

            // 向当前文件窗口发送一个格式化命令
            if (m_text_manager == null) return;
            m_text_manager.GetActiveView(1, null, out IVsTextView view);
            if (view == null) return;
            if (m_adapters_factory == null) return;
            var text_view = m_adapters_factory.GetWpfTextView(view);
            if (text_view == null) return;

            text_view.Properties.TryGetProperty(nameof(UIViewItem), out UIViewItem view_item);
            // 如果是proto，那么就
            if (view_item != null)
            {
                ProtoToEMsgTypes(server, text_view, view_item);
                return;
            }

            OtherToProto(server, text_view);
        }

        private void ProtoToEMsgTypes(ALanguageServer server, IWpfTextView text_view, UIViewItem view_item)
        {
            int offset = text_view.Caret.Position.BufferPosition.Position;
            string project_path = view_item.GetProjectPath();
            string full_path = view_item.GetFullPath();
            server.AddTask(() => AProtobufFactoryClass.inst.GotoEMsgTypes(server, project_path, full_path, offset));
        }

        private void OtherToProto(ALanguageServer server, IWpfTextView text_view)
        {
            int offset = text_view.Caret.Position.BufferPosition.Position;
            int length = text_view.TextBuffer.CurrentSnapshot.Length;
            // 往前找
            int start = offset;
            for (int i = offset - 1; i >= 0; --i)
            {
                var value = text_view.TextBuffer.CurrentSnapshot[i];
                if (char.IsDigit(value)) continue;
                if (char.IsLetter(value)) continue;
                if (value == '_') continue;
                if (value == ':') continue;
                if (value == '.') continue;

                start = i + 1;
                break;
            }

            // 往后找
            int end = offset;
            for (int i = offset; i < length; ++i)
            {
                var value = text_view.TextBuffer.CurrentSnapshot[i];
                if (char.IsDigit(value)) continue;
                if (char.IsLetter(value)) continue;
                if (value == '_') continue;
                if (value == ':') continue;
                if (value == '.') continue;

                end = i;
                break;
            }

            // 获取文本
            var text = text_view.TextBuffer.CurrentSnapshot.GetText(start, end - start);
            server.AddTask(() => server.FastGoto(text));
        }
    }
}
