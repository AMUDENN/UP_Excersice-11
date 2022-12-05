using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace UP_Excersice_11
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += ShowCloseMessage;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        private void ShowCloseMessage(object sender, CancelEventArgs e)
        {
            if (ActionConfirmation("Вы уверены, что хотите закрыть приложение?") == MessageBoxResult.No) e.Cancel = true;
        }
        private static MessageBoxResult ActionConfirmation(string question) => MessageBox.Show(question, "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
        private void ChangeFile(string fileName, string currentCharset)
        {
            FileName.Content = fileName.Split('\\').Last();
            Charset.Content = currentCharset;
        }
        private static string? OpenFileDialogName(string filter)
        {
            OpenFileDialog ofd = new();
            ofd.Filter = filter;
            if (ofd.ShowDialog() == false) return null;
            return ofd.FileName;
        }
        private static string? SaveFileDialogName(string filter)
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
            ChangeFile(fileName, "Unicode");
        }
        private void LoadWin1251Click(object sender, RoutedEventArgs e)
        {
            string? fileName = OpenFileDialogName("Text Files (*.txt)|*.txt");
            if (fileName == null) return;
            MainRTB.Document.Blocks.Clear();
            using FileStream fs = new(fileName, FileMode.Open);
            StreamReader f = new(fs, Encoding.GetEncoding("windows-1251"));
            MainRTB.AppendText(f.ReadToEnd());
            ChangeFile(fileName, "Win1251");
        }
        private void LoadRTFClick(object sender, RoutedEventArgs e)
        {
            string? fileName = OpenFileDialogName("RichText Files (*.rtf)|*.rtf");
            if (fileName == null) return;
            MainRTB.Document.Blocks.Clear();
            using FileStream fs = new(fileName, FileMode.Open);
            new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Load(fs, DataFormats.Rtf);
            ChangeFile(fileName, "RTF");
        }
        private void LoadBinaryClick(object sender, RoutedEventArgs e)
        {
            string? fileName = OpenFileDialogName("Binary Files (*.dat,*.bin)|*.dat;*.bin");
            if (fileName == null) return;
            MainRTB.Document.Blocks.Clear();
            MainRTB.AppendText(Encoding.Default.GetString(File.ReadAllBytes(fileName)));
            ChangeFile(fileName, "Binary");
        }
        private void SaveUnicodeClick(object sender, RoutedEventArgs e)
        {
            string? fileName = SaveFileDialogName("Text Files (*.txt)|*.txt");
            if (fileName == null) return;
            using StreamWriter writer = new(fileName, false, Encoding.UTF8);
            writer.Write(new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Text);
            ChangeFile(fileName, "Unicode");
        }
        private void SaveWin1251Click(object sender, RoutedEventArgs e)
        {
            string? fileName = SaveFileDialogName("Text Files (*.txt)|*.txt");
            if (fileName == null) return;
            using StreamWriter writer = new(fileName, false, Encoding.GetEncoding("Windows-1251"));
            writer.Write(new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Text);
            ChangeFile(fileName, "Win1251");
        }
        private void SaveRTFClick(object sender, RoutedEventArgs e)
        {
            string? fileName = SaveFileDialogName("RichText Files (*.rtf)|*.rtf");
            if (fileName == null) return;
            new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Save(File.Create(fileName), DataFormats.Rtf);
            ChangeFile(fileName, "RTF");
        }
        private void SaveBinaryClick(object sender, RoutedEventArgs e)
        {
            string? fileName = SaveFileDialogName("Binary Files dat (*.dat)|*.dat|Binary Files bin (*.bin)|*.bin");
            if (fileName == null) return;
            using BinaryWriter writer = new (File.Open(fileName, FileMode.Create));
            writer.Write(new TextRange(MainRTB.Document.ContentStart, MainRTB.Document.ContentEnd).Text);
            ChangeFile(fileName, "Binary");
        }
        private void PrintClick(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new();
            if(pd.ShowDialog() == false) return;

            FlowDocument doc = MainRTB.Document;

            double pageHeight = doc.PageHeight;
            double pageWidth = doc.PageWidth;
            Thickness pagePadding = doc.PagePadding;
            double columnGap = doc.ColumnGap;
            double columnWidth = doc.ColumnWidth;

            doc.PageHeight = pd.PrintableAreaHeight;
            doc.PageWidth = pd.PrintableAreaWidth;
            doc.PagePadding = new Thickness(30);

            pd.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Печать документа");

            doc.PageHeight = pageHeight;
            doc.PageWidth = pageWidth;
            doc.PagePadding = pagePadding;
            doc.ColumnGap = columnGap;
            doc.ColumnWidth = columnWidth;
        }
    }
}
