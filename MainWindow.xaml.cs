using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Markup;
using System.Xml;

namespace WpfApp15
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XAML Format (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                using (FileStream fs = File.Open(dlg.FileName, FileMode.Open))
                flowDoc.Document = XamlReader.Load(fs) as FlowDocument;
            }
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XAML Format (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                using (FileStream fs = File.Open(dlg.FileName, FileMode.Create))
                XamlWriter.Save(flowDoc.Document, fs);
            }
        }
        private void Clear(object sender, RoutedEventArgs e)
        {
            flowDoc.ClearValue(FlowDocumentScrollViewer.DocumentProperty);
        }
        private void Print(object sender, RoutedEventArgs e)
        {
            var str = XamlWriter.Save(flowDoc.Document);
            var stringReader = new System.IO.StringReader(str);
            var xmlReader = XmlReader.Create(stringReader);
            var CloneDoc = XamlReader.Load(xmlReader) as FlowDocument;
            
            //Now print using PrintDialog
            var pd = new PrintDialog();

            if (pd.ShowDialog().Value)
            {
                CloneDoc.PageHeight = pd.PrintableAreaHeight;
                CloneDoc.PageWidth = pd.PrintableAreaWidth;
                //System.Printing.PrintDocumentImageableArea ia = null;
                //XpsDocumentWriter docWriter = System.Printing.PrintQueue.CreateXpsDocumentWriter(ref ia);
                //Thickness t = new Thickness(72);  // CloneDoc.PagePadding;
                //CloneDoc.PagePadding = new Thickness(
                //                 Math.Max(ia.OriginWidth, t.Left),
                //                   Math.Max(ia.OriginHeight, t.Top),
                //                   Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), t.Right),
                //                   Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), t.Bottom));
                //CloneDoc.ColumnWidth = double.PositiveInfinity;
                IDocumentPaginatorSource idocument = CloneDoc as IDocumentPaginatorSource;

                pd.PrintDocument(idocument.DocumentPaginator, "Printing FlowDocument");
            }
        }
    }
}
