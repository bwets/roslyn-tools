using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynTreeView
{
    public partial class Main : Form
    {
        private string _filename = @"";
        public Main()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //await Reload(_filename);
            await SelectFile();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag == null)
            {
                return;
            }
            NodeData data = (e.Node.Tag as NodeData)!;
            StringBuilder sb = new StringBuilder();
            if (data.Node != null)
            {
                SyntaxNode node = data.Node;
                sb.AppendLine("Node Type: " + node.GetType().Name);
                if (node is FieldDeclarationSyntax field)
                {

                }

                sb.AppendLine("===============================");
                sb.AppendLine(node.ToFullString());
            }
            else
            {
                SyntaxToken? token = data.Token;
                if (token == null)
                {
                    return;
                }
                sb.AppendLine("Token Type: " + token.GetType().Name);
                sb.AppendLine("Content: " + token.ToString());

            }
            richTextBox1.Text = sb.ToString();

        }

        private async Task Reload(string filename)
        {
            linkLabel1.Text = filename;

            _filename = filename;
            await Visitor.Start(treeView1, _filename);
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            await SelectFile();
        }

        private async Task SelectFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "CSharp|*.cs";
            dialog.Title = "Select code file...";
            dialog.Multiselect = false;
            dialog.CheckFileExists = true;
            DialogResult r = dialog.ShowDialog(this);
            if (r == DialogResult.OK)
            {
                await Reload(dialog.FileName);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await Reload(_filename);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var info = new ProcessStartInfo
            {
                FileName = _filename,
                UseShellExecute = true
            };
            Process.Start(info);
        }
    }
}
