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
using SecondaryStructureTool.DataModel;



namespace SecondaryStructureTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SetupData setupData;
        private HydropathyPlotter hydroPlotter;
        public MainWindow()
        {
            InitializeComponent();
            setupData = new SetupData();
            Setup.DataContext = setupData;
            Results_Title.DataContext = setupData;
            hydroPlotter = new HydropathyPlotter();
        }


        public void CreateGrid(){
            int seqLength = setupData.Sequence.Length;
            ClearGrid();
            int gridWidth = seqLength * 25;
            DataGridWrapper.Width = gridWidth;
            DataGrid.Width = gridWidth;
            GraphGrid.Width = gridWidth;
            #region GraphZeroLine
            Polyline GraphZeroLine = new Polyline();
            GraphZeroLine.StrokeThickness = 4;
            GraphZeroLine.Stroke = Brushes.Blue;
            Point ZeroStart = new Point(0, 300);
            Point ZeroEnd = new Point(gridWidth, 300);
            GraphZeroLine.Points.Add(ZeroStart);
            GraphZeroLine.Points.Add(ZeroEnd);
            GraphGrid.Children.Add(GraphZeroLine);

            Polyline GraphMaxLine = new Polyline();
            GraphMaxLine.StrokeThickness = 4;
            GraphMaxLine.Stroke = Brushes.Red;
            Point MaxStart = new Point(0, 0);
            Point MaxStop = new Point(gridWidth, 0);
            GraphMaxLine.Points.Add(MaxStart);
            GraphMaxLine.Points.Add(MaxStop);
            GraphGrid.Children.Add(GraphMaxLine);

            Polyline GraphMinLine = new Polyline();
            GraphMinLine.StrokeThickness = 4;
            GraphMinLine.Stroke = Brushes.Green;
            Point MinStart = new Point(0, 600);
            Point MinStop = new Point(gridWidth, 600);
            GraphMinLine.Points.Add(MinStart);
            GraphMinLine.Points.Add(MinStop);
            GraphGrid.Children.Add(GraphMinLine);

            #endregion


            ColumnDefinition[] columns = new ColumnDefinition[seqLength * 2];
            for (int i = 0; i < columns.Length; ++i)
            {
                columns[i] = new ColumnDefinition();
                if (i % 2 == 0) // even column is a spacer width 5
                {
                    columns[i].Width = new GridLength(5, GridUnitType.Pixel);
                }
                else // odd column is a data column 30 width
                {
                    columns[i].Width = new GridLength(20, GridUnitType.Pixel);
                }
                DataGrid.ColumnDefinitions.Add(columns[i]);
            }

            RowDefinition[] rows = new RowDefinition[1];
            for (int i = 0; i < rows.Length; ++i )
            {
                rows[i] = new RowDefinition();
            }
            rows[0].Height = new GridLength(20, GridUnitType.Pixel);
            //rows[1].Height = new GridLength(20, GridUnitType.Pixel);
            foreach (RowDefinition r in rows){
                DataGrid.RowDefinitions.Add(r);
            }
            //DataGrid.ShowGridLines = true;
            TextBlock[] seq = new TextBlock[seqLength];
            for (int i = 0; i < seq.Length; ++i)
            {
                seq[i] = new TextBlock();
            }
            
            for (var i = 0; i < seqLength; ++i)
            {
                string blockNum = "tBlock_" + i;
                seq[i].Text = setupData.Sequence[i] + "";
                seq[i].Width = 20;
                seq[i].Height = 20;
                seq[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                seq[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                seq[i].Foreground = Brushes.Red;
                seq[i].Background = Brushes.Black;
                seq[i].FontSize = 16;
                seq[i].Name = blockNum;
                seq[i].SetValue(Grid.RowProperty, 1);
                seq[i].TextAlignment = TextAlignment.Center;
                int colNum = (i * 2) + 1;
                seq[i].SetValue(Grid.ColumnProperty, (colNum));
                seq[i].Background = Brushes.Transparent;
                DataGrid.Children.Add(seq[i]);
            }
            
            for (var i = 0; i < seqLength; ++i)
            {
                string hydroName = "hydroPlot_" + i;

            }

            DrawHydropathyPlot();
            
        }

        private void StartAnalysisTool_Click(object sender, RoutedEventArgs e)
        {
            if (setupData.Name == "")
            {
                if (SeqOrFASTA.Text != "")
                {
                    setupData.SequenceOrFASTAInput = SeqOrFASTA.Text;
                }
            }
            if (!setupData.BadSequence)
            {
                Setup.Visibility = System.Windows.Visibility.Collapsed;
                DataManipulationGrid.Visibility = System.Windows.Visibility.Visible;

                //start hydropathy plot method

                hydroPlotter.InitHydroPlotter(setupData.Sequence);
                //start chou-fasman method
                //get TMHMM data
                //run create Grid
                CreateGrid();
            }
        }

        private void ClearGrid()
        {
            if (DataGrid.Children.Count > 0)
            {
                DataGrid.Children.Clear();
                DataGrid.ColumnDefinitions.Clear();
                DataGrid.RowDefinitions.Clear();
            }
            if (GraphGrid.Children.Count > 0)
            {
                GraphGrid.Children.Clear();
                GraphGrid.ColumnDefinitions.Clear();
                GraphGrid.RowDefinitions.Clear();
            }
        }


        private void Reset_Clicked(object sender, RoutedEventArgs e)
        {
            Setup.Visibility = System.Windows.Visibility.Visible;
            DataManipulationGrid.Visibility = System.Windows.Visibility.Collapsed;
            //remove data from setup data variables
            setupData.Name = "";
            setupData.Sequence = "";
            ClearGrid();
        }

        private void BadSequenceData_Checked(object sender, RoutedEventArgs e)
        {
            if (setupData.BadSequence)
            {
                SeqOrFASTA.Background = Brushes.Red;
            }
            else
            {
                SeqOrFASTA.Background = new SolidColorBrush(Color.FromArgb(255,222,222,222));
            }
        }

        private void DrawHydropathyPlot()
        {
            Polyline transMem = new Polyline();
            transMem.StrokeThickness = 4;
            transMem.Stroke = Brushes.BlueViolet;
            transMem.Name = "transMem";
            bool firstPoint = true;
            int location = 15;
            foreach (float f in hydroPlotter.SequenceKDTransMembrane)
            {
                Point p = new Point(location, 300 - (f * 6));
                transMem.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(transMem);

            Polyline surfaceRegion = new Polyline();
            surfaceRegion.StrokeThickness = 4;
            surfaceRegion.Stroke = Brushes.BlanchedAlmond;
            surfaceRegion.Name = "surfaceRegion";
            firstPoint = true;
            location = 15;
            foreach (float f in hydroPlotter.SequenceKDSurfaceRegions)
            {
                Point p = new Point(location, 300 - (f * 6));
                surfaceRegion.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(surfaceRegion);
            SurfaceRegion_TransMembrane.IsChecked = true;
        }

        private void TransMembraneOnly_Checked(object sender, RoutedEventArgs e)
        {
            Polyline transMemCntl, surfaceCntl;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                    if (child != null && child.Name == "transMem")
                    {
                        transMemCntl = (Polyline)child;
                        transMemCntl.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (child != null && child.Name == "surfaceRegion")
                    {
                        surfaceCntl = (Polyline)child;
                        surfaceCntl.Visibility = System.Windows.Visibility.Hidden;
                    }  
                }
            }
        }

        private void SurfaceRegionOnly_Checked(object sender, RoutedEventArgs e)
        {
           Polyline transMemCntl, surfaceCntl;
           if (setupData.Sequence != "")
           {
               for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
               {
                   var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                   if (child != null && child.Name == "transMem")
                   {
                       transMemCntl = (Polyline)child;
                       transMemCntl.Visibility = System.Windows.Visibility.Hidden;
                   }
                   if (child != null && child.Name == "surfaceRegion")
                   {
                       surfaceCntl = (Polyline)child;
                       surfaceCntl.Visibility = System.Windows.Visibility.Visible;
                   }
               }
           }
        }

        private void TransMemAndSurface_Checked(object sender, RoutedEventArgs e)
        {
            Polyline transMemCntl, surfaceCntl;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                    if (child != null && child.Name == "transMem")
                    {
                        transMemCntl = (Polyline)child;
                        transMemCntl.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (child != null && child.Name == "surfaceRegion")
                    {
                        surfaceCntl = (Polyline)child;
                        surfaceCntl.Visibility = System.Windows.Visibility.Visible;
                    }  
                } 
            }
        }
    }
}
