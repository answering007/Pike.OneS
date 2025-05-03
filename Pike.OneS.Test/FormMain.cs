using System;
using System.Data.Common;
using System.Diagnostics;
using System.Windows.Forms;

namespace Pike.OneS.Test
{
    public sealed partial class FormMain : Form
    {
        const string DbCommandText = "ВЫБРАТЬ 1";
        const string FactoryName = "Pike.OneS.Data.OneSDbProviderFactory";
        const string SeparatorLine = "==================================";

        public FormMain()
        {
            InitializeComponent();
            Text = string.Concat(Text, ", Is64BitProcess = ", Environment.Is64BitProcess);
            textBoxConnection.Text = SettingsMain.Default.ConnectionString;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            ClearText();
            var sw = new Stopwatch();
            sw.Start();
            AddText("Starting...");
            try
            {
                AddText("Creating DbProviderFactory...");
                var factory = DbProviderFactories.GetFactory(FactoryName);
                AddText($"Factory created = {sw.Elapsed}");

                AddText($"Creating connection to = [{textBoxConnection.Text}]");
                using (var dbConnection = factory.CreateConnection())
                {
                    AddText($"Connection created = {sw.Elapsed}");
                    if (dbConnection == null) throw new NullReferenceException();

                    dbConnection.ConnectionString = textBoxConnection.Text;
                    AddText("Opening connection...");
                    dbConnection.Open();
                    AddText($"Connection opened = {sw.Elapsed}");

                    AddText("Creating command...");
                    using (var dbCommand = factory.CreateCommand())
                    {
                        AddText($"Create command = {sw.Elapsed}");
                        if (dbCommand == null) throw new NullReferenceException();

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = DbCommandText;
                        using (var dbReader = dbCommand.ExecuteReader())
                        {
                            var cnt = 0;
                            AddText($"Command executed = {sw.Elapsed}");
                            while (dbReader.Read())
                            {
                                var values = new object[dbReader.FieldCount];
                                dbReader.GetValues(values);

                                cnt++;
                                AddText($"Row = {cnt}");
                            }
                            AddText($"Number of rows = {cnt}");
                            AddText($"Read = {sw.Elapsed}");
                        }
                        AddText("Reader disposed");
                    }
                    AddText("Command disposed");
                }
                AddText("Connection disposed");

                AddText(SeparatorLine);
                AddText("CONNECTION SUCCEEDED!");
                AddText(SeparatorLine);
            }
            catch (Exception exception)
            {
                AddText(SeparatorLine);
                AddText(exception.ToString());
                AddText(SeparatorLine);
            }
            sw.Stop();
            AddText($"Done in = {sw.Elapsed}");
        }

        void ClearText()
        {
            textBoxMain.Text = string.Empty;
        }

        void AddText(string line)
        {
            if (textBoxMain.Text.Length == 0)
                textBoxMain.Text = line;
            else
                textBoxMain.AppendText("\r\n" + line);
        }
    }
}
