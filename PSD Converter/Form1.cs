using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSD_Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            MagickNET.Magick.Init();
            MagicNet.Image img = new MagicNet.Image("file.psd");
            img.Resize(System.Drawing.Size(100, 100));
            img.Write("test.png");
            MagickNet.Magick.Term();
            */

            /*
            ImageMagick.MagickReadSettings settings =
                  new ImageMagick.MagickReadSettings();

            settings.BackgroundColor = new MagickColor();

            MagickImageCollection imgs =
                new ImageMagick.MagickImageCollection("読み込みたいPSDファイル.psd", settings);

            imgs.Coalesce();
            imgs.RemoveAt(0);

            imgs.Write("1.png");

            imgs.Dispose();

            */
            /*
            using (MagickImageCollection imgs = new MagickImageCollection("1.psd"))
            {
                int width = imgs[0].Width;
                int height = imgs[0].Height;

                //imgs[0]は個人の好みで入れていない。
                for (int i = 1; i < imgs.Count; i++)
                {
                    //レイヤーの大きさを統一している。for_coalesceを外に置いておくとエラーが発生する。
                    using (MagickImageCollection result = new MagickImageCollection())
                    {
                        MagickColor transparent = new MagickColor(0, 0, 0, 0);
                        MagickImage for_coalesce = new MagickImage(transparent, width, height);
                        for_coalesce.BackgroundColor = transparent;

                        result.Add(for_coalesce);
                        result.Add(imgs[i]);

                        result.Coalesce();

                        result[1].Write($"psd\\{String.Format("{0:D6}", i - 1)}.png");
                    }

                    //定期的にGCを入れないとメモリの使用量が大変なことになる。
                    if (i % 20 == 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
                }
            }
            */


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            /*
            ImageMagick.MagickImage img =
                new ImageMagick.MagickImage("1.psd");
            //BMP形式で保存する
            img.Write("1.png");
            //後始末
            img.Dispose();
            */
            //PictureBox1に表示する
            //pictureBox1.ImageLocation = "1.png";


        }


        public void unzip_Start()
        {
            var task = Converter();
        }
        private async Task unzip()
        {
            var task = Task.Run(() =>
            {
                //string[] subFolders = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
                string[] subFolders = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
                //listBox1.Items.Add(subFolders);
                //string[] aa = { };

                foreach (var test in subFolders)
                {
                    //MessageBox.Show(test);

                    //listBox1.Items.Add(test);

                    //string paa = System.IO.Path.GetFileName(test);

                    //listBox1.Items.Add(paa);
                    //aa = paa;
                    /*
                    Array.Resize(ref aa, aa.Length + 1);
                    aa[aa.Length - 1] = paa;
                    */
                    string[] psdfiles = System.IO.Directory.GetFiles(test, "*.zip", System.IO.SearchOption.AllDirectories);

                    foreach (var zip in psdfiles)
                    {
                        progressBar1.Value = 0;
                        if (Directory.Exists($@"{test}\ZIP"))
                        {
                            if (!File.Exists($@"{test}\ZIP\{System.IO.Path.GetFileNameWithoutExtension(zip)}.zip"))
                            {
                                try
                                {
                                    progressBar1.Value = 50;
                                    FileStream fs = new FileStream(zip, FileMode.Open);
                                    ZipArchive za = new ZipArchive(fs, ZipArchiveMode.Read);

                                    foreach (ZipArchiveEntry ze in za.Entries)
                                    {
                                        ze.ExtractToFile($@"{test}\ZIP\{System.IO.Path.GetFileNameWithoutExtension(zip)} + {ze.FullName}");
                                        label1.Text = $@"名前: {ze.FullName}を展開しました";
                                    }
                                    za.Dispose();
                                    fs.Close();

                                    //pictureBox1.ImageLocation = ($@"{test}\ZIP\{System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}");
                                    
                                    progressBar1.Value = 100;
                                }
                                catch
                                {

                                }
                            }
                        }
                        else
                        {
                            if (!File.Exists($@"{test}\ZIP\{System.IO.Path.GetFileNameWithoutExtension(zip)}.zip"))
                            {
                                try
                                {
                                    progressBar1.Value = 50;
                                    Directory.CreateDirectory($@"{test}\ZIP");

                                    progressBar1.Value = 50;
                                    FileStream fs = new FileStream(zip, FileMode.Open);
                                    ZipArchive za = new ZipArchive(fs, ZipArchiveMode.Read);

                                    foreach (ZipArchiveEntry ze in za.Entries)
                                    {
                                        ze.ExtractToFile($@"{test}\ZIP\{System.IO.Path.GetFileNameWithoutExtension(zip)} + {ze.FullName}");
                                        label1.Text = $@"名前: {ze.FullName}を展開しました";
                                    }
                                    za.Dispose();
                                    fs.Close();

                                    progressBar1.Value = 100;
                                }
                                catch
                                {

                                }
                            }
                        }

                    }
                }
                progressBar1.Value = 100;
                MessageBox.Show("変換が全て終わりました。", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //listBox1.Items.Add(aa);
            });
        }


        public void Converter_Start()
        {
            var task = Converter();
        }
        private async Task Converter()
        {
            var task = Task.Run(() =>
            {
                //string[] subFolders = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
                string[] subFolders = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
                //listBox1.Items.Add(subFolders);
                //string[] aa = { };

                //MessageBox.Show(subFolders.ToString());
                foreach (var test in subFolders)
                {

                    //listBox1.Items.Add(test);

                    //string paa = System.IO.Path.GetFileName(test);

                    //listBox1.Items.Add(paa);
                    //aa = paa;
                    /*
                    Array.Resize(ref aa, aa.Length + 1);
                    aa[aa.Length - 1] = paa;
                    */
                    string[] psdfiles = System.IO.Directory.GetFiles(test, "*.psd", System.IO.SearchOption.AllDirectories);

                    foreach (var psd in psdfiles)
                    {
                        progressBar1.Value = 0;
                        if (Directory.Exists($@"{test}\PSD"))
                        {
                            if (!File.Exists($@"{test}\PSD\{System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}"))
                            {
                                try
                                {
                                    progressBar1.Value = 50;
                                    ImageMagick.MagickImage img = new ImageMagick.MagickImage(psd);
                                    img.Write($@"{test}\PSD\{System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}");
                                    img.Dispose();
                                    pictureBox1.ImageLocation = ($@"{test}\PSD\{System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}");
                                    label1.Text = $@"名前: {System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}";
                                    progressBar1.Value = 100;
                                }
                                catch
                                {

                                }
                            }
                        }
                        else
                        {
                            if (!File.Exists($@"{test}\PSD\{System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}"))
                            {
                                try
                                {
                                    progressBar1.Value = 50;
                                    Directory.CreateDirectory($@"{test}\PSD");
                                    ImageMagick.MagickImage img = new ImageMagick.MagickImage(psd);
                                    img.Write($@"{test}\PSD\{System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}");
                                    img.Dispose();
                                    pictureBox1.ImageLocation = ($@"{test}\PSD\{System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}");
                                    label1.Text = $@"名前: {System.IO.Path.GetFileNameWithoutExtension(psd)}.{extensions}";
                                    progressBar1.Value = 100;
                                }
                                catch
                                {

                                }
                            }
                        }

                    }
                }
                progressBar1.Value = 100;
                MessageBox.Show("変換が全て終わりました。", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //listBox1.Items.Add(aa);
            });
        }
            string path = "";
        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new FolderSelectDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Title = "フォルダーパスを選択してください。"
            };
            
            if (dialog.Show(Handle))
            {
                //this.textBox1.Text = dialog.FileName;
                path = (dialog.FileName);
                textBox1.Text = path;
            }
            
            /*
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
            }
            */
        }
        public class FolderSelectDialog
        {
            private string _initialDirectory;
            private string _title;
            private string _fileName = "";

            public string InitialDirectory
            {
                get { return string.IsNullOrEmpty(_initialDirectory) ? Environment.CurrentDirectory : _initialDirectory; }
                set { _initialDirectory = value; }
            }
            public string Title
            {
                get { return _title ?? "Select a folder"; }
                set { _title = value; }
            }
            public string FileName { get { return _fileName; } }

            public bool Show() { return Show(IntPtr.Zero); }

            /// <param name="hWndOwner">Handle of the control or window to be the parent of the file dialog</param>
            /// <returns>true if the user clicks OK</returns>
            public bool Show(IntPtr hWndOwner)
            {
                var result = Environment.OSVersion.Version.Major >= 6
                    ? VistaDialog.Show(hWndOwner, InitialDirectory, Title)
                    : ShowXpDialog(hWndOwner, InitialDirectory, Title);
                _fileName = result.FileName;
                return result.Result;
            }

            private struct ShowDialogResult
            {
                public bool Result { get; set; }
                public string FileName { get; set; }
            }

            private static ShowDialogResult ShowXpDialog(IntPtr ownerHandle, string initialDirectory, string title)
            {
                var folderBrowserDialog = new FolderBrowserDialog
                {
                    Description = title,
                    SelectedPath = initialDirectory,
                    ShowNewFolderButton = false
                };
                var dialogResult = new ShowDialogResult();
                if (folderBrowserDialog.ShowDialog(new WindowWrapper(ownerHandle)) == DialogResult.OK)
                {
                    dialogResult.Result = true;
                    dialogResult.FileName = folderBrowserDialog.SelectedPath;
                }
                return dialogResult;
            }

            private static class VistaDialog
            {
                private const string c_foldersFilter = "Folders|\n";

                private const BindingFlags c_flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                private readonly static Assembly s_windowsFormsAssembly = typeof(FileDialog).Assembly;
                private readonly static Type s_iFileDialogType = s_windowsFormsAssembly.GetType("System.Windows.Forms.FileDialogNative+IFileDialog");
                private readonly static MethodInfo s_createVistaDialogMethodInfo = typeof(OpenFileDialog).GetMethod("CreateVistaDialog", c_flags);
                private readonly static MethodInfo s_onBeforeVistaDialogMethodInfo = typeof(OpenFileDialog).GetMethod("OnBeforeVistaDialog", c_flags);
                private readonly static MethodInfo s_getOptionsMethodInfo = typeof(FileDialog).GetMethod("GetOptions", c_flags);
                private readonly static MethodInfo s_setOptionsMethodInfo = s_iFileDialogType.GetMethod("SetOptions", c_flags);
                private readonly static uint s_fosPickFoldersBitFlag = (uint)s_windowsFormsAssembly
                    .GetType("System.Windows.Forms.FileDialogNative+FOS")
                    .GetField("FOS_PICKFOLDERS")
                    .GetValue(null);
                private readonly static ConstructorInfo s_vistaDialogEventsConstructorInfo = s_windowsFormsAssembly
                    .GetType("System.Windows.Forms.FileDialog+VistaDialogEvents")
                    .GetConstructor(c_flags, null, new[] { typeof(FileDialog) }, null);
                private readonly static MethodInfo s_adviseMethodInfo = s_iFileDialogType.GetMethod("Advise");
                private readonly static MethodInfo s_unAdviseMethodInfo = s_iFileDialogType.GetMethod("Unadvise");
                private readonly static MethodInfo s_showMethodInfo = s_iFileDialogType.GetMethod("Show");

                public static ShowDialogResult Show(IntPtr ownerHandle, string initialDirectory, string title)
                {
                    var openFileDialog = new OpenFileDialog
                    {
                        AddExtension = false,
                        CheckFileExists = false,
                        DereferenceLinks = true,
                        Filter = c_foldersFilter,
                        InitialDirectory = initialDirectory,
                        Multiselect = false,
                        Title = title
                    };

                    var iFileDialog = s_createVistaDialogMethodInfo.Invoke(openFileDialog, new object[] { });
                    s_onBeforeVistaDialogMethodInfo.Invoke(openFileDialog, new[] { iFileDialog });
                    s_setOptionsMethodInfo.Invoke(iFileDialog, new object[] { (uint)s_getOptionsMethodInfo.Invoke(openFileDialog, new object[] { }) | s_fosPickFoldersBitFlag });
                    var adviseParametersWithOutputConnectionToken = new[] { s_vistaDialogEventsConstructorInfo.Invoke(new object[] { openFileDialog }), 0U };
                    s_adviseMethodInfo.Invoke(iFileDialog, adviseParametersWithOutputConnectionToken);

                    try
                    {
                        int retVal = (int)s_showMethodInfo.Invoke(iFileDialog, new object[] { ownerHandle });
                        return new ShowDialogResult
                        {
                            Result = retVal == 0,
                            FileName = openFileDialog.FileName
                        };
                    }
                    finally
                    {
                        s_unAdviseMethodInfo.Invoke(iFileDialog, new[] { adviseParametersWithOutputConnectionToken[1] });
                    }
                }
            }

            // Wrap an IWin32Window around an IntPtr
            private class WindowWrapper : IWin32Window
            {
                private readonly IntPtr _handle;
                public WindowWrapper(IntPtr handle) { _handle = handle; }
                public IntPtr Handle { get { return _handle; } }
            }
        }
        //int psdcount = 1;
        private void button2_Click(object sender, EventArgs e)
        {

            Converter_Start();


        }

        string extensions = "png";
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            extensions = comboBox1.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //path = textBox1.Text;
        }

        private void 最前面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if(trackBar1.Value > 0)
            {
                Opacity = trackBar1.Value * 0.1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            var dialog = new FolderSelectDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Title = "保存フォルダーパスを選択してください。"
            };

            if (dialog.Show(Handle))
            {
                //this.textBox1.Text = dialog.FileName;
                path = (dialog.FileName);
                textBox1.Text = path;
            }
            */
            unzip_Start();
        }
    }
}
