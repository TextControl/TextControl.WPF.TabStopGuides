using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace TabStop_Guides
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            lines.BringIntoView();
        }

        private bool guidesEnabled = true;
        public void CreateLine(double start1, double start2, double end1, double end2)
        {
            // Create a Line
            Line blackLine = new Line();

            double upperOffset = rulerBar1.ActualHeight;
            double rulerBarOffset = 5;
            
            blackLine.X1 = start1 + 1;
            blackLine.Y1 = start2 + upperOffset - rulerBarOffset;
            blackLine.X2 = end1 + 1;
            blackLine.Y2 = end2 + upperOffset;

            // Create a black Brush
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            // Set Line's width and color
            blackLine.StrokeThickness = 0.1;
            blackLine.Stroke = blackBrush;

            // Add line to the Grid.
            lines.Children.Add(blackLine);
            
        }

        private void textControl_Loaded(object sender, RoutedEventArgs e)
        {
            textControl.RulerBar = "rulerBar1";
            int[] clearTabs = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            textControl.Paragraphs.GetItem(textControl.InputPosition.TextPosition).Format.TabPositions = clearTabs;
            GetTabStops(textControl.Paragraphs.GetItem(textControl.InputPosition.TextPosition));
        }

        private void GetTabStops(TXTextControl.Paragraph par)
        {
            lines.Children.Clear();
            if (guidesEnabled)
            {
                
                foreach (int i in par.Format.TabPositions)
                {
                    if (i > 0)
                    {
                        double currentTabStop = (i / 15) + (Math.Abs(textControl.ScrollLocation.X) / 15) + textControl.Sections.GetItem().Format.PageMargins.Left + 1;
                        CreateLine(currentTabStop, 0, currentTabStop, textControl.ActualHeight);
                    }
                }
            }
        }

        private void textControl_ParagraphFormatChanged(object sender, EventArgs e)
        {

            GetTabStops(textControl.Paragraphs.GetItem(textControl.InputPosition.TextPosition));

        }

        private void textControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (textControl.Paragraphs != null)
                GetTabStops(textControl.Paragraphs.GetItem(textControl.InputPosition.TextPosition));

        }

        private void GuideMenu_ShowGuides_Click(object sender, RoutedEventArgs e)
        {
            guidesEnabled = !(guidesEnabled);
            GetTabStops(textControl.Paragraphs.GetItem(textControl.InputPosition.TextPosition));
        }

        private void FileMenu_Load_Click(object sender, RoutedEventArgs e)
        {
            TXTextControl.LoadSettings ls = new TXTextControl.LoadSettings
            {
                ApplicationFieldFormat = TXTextControl.ApplicationFieldFormat.MSWord,
                LoadHypertextLinks = true,
                LoadImages = true,
                ValidateFormat = true
            };
            textControl.Load(TXTextControl.StreamType.All, ls);
        }

        private void GuideMenu_ShowGuides_LayoutUpdated(object sender, EventArgs e)
        {
            GuideMenu_ShowGuides.IsChecked = guidesEnabled;
        }
    }
}
