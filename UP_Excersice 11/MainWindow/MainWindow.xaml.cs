using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace UP_Excersice_11
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        private string? OpenFileDialogName(string filter)
        {
            OpenFileDialog ofd = new();
            ofd.Filter = filter;
            if (ofd.ShowDialog() == false) return null;
            return ofd.FileName;
        }
        private string? SaveFileDialogName(string filter)
        {
            SaveFileDialog sfd = new();
            sfd.Filter = filter;
            if (sfd.ShowDialog() == false) return null;
            return sfd.FileName;
        }
        private void LoadUnicodeClick(object sender, RoutedEventArgs e)
        {
            string? fileName = OpenFileDialogName("Text Files (*.txt)|*.txt");
            if (fileName == null) return;
            MainRTB.Document.Blocks.Clear();
            MainRTB.AppendText(File.ReadAllText(fileName));
        }
        private void LoadWin1251Click(object sender, RoutedEventArgs e)
        {
            string? fileName = OpenFileDialogName("Text Files (*.txt)|*.txt");
            if (fileName == null) return;
            MainRTB.Document.Blocks.Clear();
            using FileStream fs = new(fileName, FileMode.Open);
            StreamReader f = new(fs, Encoding.GetEncoding("windows-1251"));
            MainRTB.AppendText(f.ReadToEnd());
        }
        private void LoadRTFClick(object sender, RoutedEventArgs e)
        {
            string? fileName = OpenFileDialogName("RichText Files (*.rtf)|*.rtf");
            if (fileName == null) return;
            MainRTB.Document.Blocks.Clear();
            using FileStream fs = new(fileName, FileMode.Open);
            new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Load(fs, DataFormats.Rtf);
        }
        private void LoadBinaryClick(object sender, RoutedEventArgs e)
        {
            string? fileName = OpenFileDialogName("Binary Files (*.dat)|*.dat");
            if (fileName == null) return;
            MainRTB.Document.Blocks.Clear();
            MainRTB.AppendText(Encoding.Default.GetString(File.ReadAllBytes(fileName)));
        }
        private void SaveUnicodeClick(object sender, RoutedEventArgs e)
        {
            string? fileName = SaveFileDialogName("Text Files (*.txt)|*.txt");
            if (fileName == null) return;
            using StreamWriter writer = new(fileName, false, Encoding.UTF8);
            writer.Write(new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Text);
        }
        private void SaveWin1251Click(object sender, RoutedEventArgs e)
        {
            string? fileName = SaveFileDialogName("Text Files (*.txt)|*.txt");
            if (fileName == null) return;
            using StreamWriter writer = new(fileName, false, Encoding.GetEncoding("Windows-1251"));
            writer.Write(new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Text);
        }
        private void SaveRTFClick(object sender, RoutedEventArgs e)
        {
            string? fileName = SaveFileDialogName("RichText Files (*.rtf)|*.rtf");
            if (fileName == null) return;
            new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Save(File.Create(fileName), DataFormats.Rtf);
        }
        private void SaveBinaryClick(object sender, RoutedEventArgs e)
        {
            string? fileName = SaveFileDialogName("Binary Files (*.dat)|*.dat");
            if (fileName == null) return;
            using BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create));
            writer.Write(new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Text);
        }
    }
}
