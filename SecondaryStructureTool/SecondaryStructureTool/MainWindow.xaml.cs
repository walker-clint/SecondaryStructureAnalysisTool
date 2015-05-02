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
        #region Private Variables
        private Button selectedResidueButton;
        private SetupData setupData;
        private HydropathyPlotter hydroPlotter;
        private double height, width;
        private bool CustomWindowActive = false;
        private int ScaleFactor = 5;
        private int transMem = 19;
        private int surfMem = 9;
        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            setupData = new SetupData();
            Setup.DataContext = setupData;
            Results_Title.DataContext = setupData;
            hydroPlotter = new HydropathyPlotter();
            height = PrimaryWindow.ActualHeight;
            width = PrimaryWindow.ActualWidth;
            selectedResidueButton = new Button();
            try
            {
                setupData.GetDataBases();
            }
            catch (Exception e)
            {
                //check to see if there is an internet connection and display error
            }
            foreach (string db in setupData.Databases)
            {
                DataBaseDropdown.Items.Add(db);
            }
        } 
        #endregion

        #region Public Methods

        public void CreateGrid()
        {
            int seqLength = setupData.Sequence.Length;
            ClearGrid();
            int gridWidth = seqLength * 25;
            DataGridWrapper.Width = gridWidth;
            DataGrid.Width = gridWidth;
            GraphGrid.Width = gridWidth;
            ChouFasmanGrid.Width = gridWidth;
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
                ChouFasmanGrid.ColumnDefinitions.Add(columns[i]);
            }

            RowDefinition[] rows = new RowDefinition[3];
            for (int i = 0; i < rows.Length; ++i)
            {
                rows[i] = new RowDefinition();
            }
            rows[0].Height = new GridLength(20, GridUnitType.Pixel);
            rows[1].Height = new GridLength(20, GridUnitType.Pixel);
            rows[2].Height = new GridLength(20, GridUnitType.Pixel);
            foreach (RowDefinition r in rows)
            {
                DataGrid.RowDefinitions.Add(r);
                ChouFasmanGrid.RowDefinitions.Add(r);
            }
            //DataGrid.ShowGridLines = true;
            Button[] seq = new Button[seqLength];
            TextBlock[] indexNumbers = new TextBlock[seqLength];
            for (int i = 0; i < seq.Length; ++i)
            {
                seq[i] = new Button();
                indexNumbers[i] = new TextBlock();
            }

            for (var i = 0; i < seqLength; ++i)
            {
                string blockNum = "tBlock_" + i;
                seq[i].Content = setupData.Sequence[i] + "";
                seq[i].Width = 20;
                seq[i].Height = 20;
                seq[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                seq[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                seq[i].Foreground = Brushes.Red;
                seq[i].Background = Brushes.Black;
                seq[i].FontSize = 9;
                seq[i].BorderThickness = new Thickness(0.0);
                seq[i].FontWeight = FontWeights.Bold;
                seq[i].Name = blockNum;
                seq[i].SetValue(Grid.RowProperty, 0);
                seq[i].Click += SeqButton_Click;


                int colNum = (i * 2) + 1;
                seq[i].SetValue(Grid.ColumnProperty, (colNum));
                seq[i].Background = Brushes.Transparent;
                DataGrid.Children.Add(seq[i]);

                indexNumbers[i].Text = i + "";
                
                indexNumbers[i].FontSize = 9;
                indexNumbers[i].TextWrapping = TextWrapping.Wrap;
                indexNumbers[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                indexNumbers[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                indexNumbers[i].Foreground = Brushes.Red;
                indexNumbers[i].Background = Brushes.Black;
                indexNumbers[i].SetValue(Grid.RowProperty, 1);
                indexNumbers[i].SetValue(Grid.ColumnProperty, (colNum));
                DataGrid.Children.Add(indexNumbers[i]);
            }




        } 
        #endregion

        #region Private Methods

        private void SeqButton_Click(object sender, EventArgs e)
        {
            
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
                Results.Visibility = System.Windows.Visibility.Visible;
                Title.Visibility = System.Windows.Visibility.Collapsed;
                //start hydropathy plot method
                hydroPlotter.InitHydroPlotter(setupData.Sequence);
                //start chou-fasman method

                //get TMHMM data

                //run create Grid
                CreateGrid();
                DrawHydropathyPlot();
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
            Results.Visibility = System.Windows.Visibility.Collapsed;
            Title.Visibility = System.Windows.Visibility.Visible;
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
                SeqOrFASTA.Background = new SolidColorBrush(Color.FromArgb(255, 222, 222, 222));
            }
        }


        #region Hydropathy

        private void DrawHydropathyPlot()
        {
            Polyline transMem_KD = new Polyline();
            transMem_KD.StrokeThickness = 4;
            transMem_KD.Stroke = Brushes.BlueViolet;
            transMem_KD.Name = "transMem_KD";
            bool firstPoint = true;
            int offset = 25 * (transMem / 2);
            int location = 15 + offset;
            for (var i = (int)hydroPlotter.SequenceKDTransMembrane[0]; i < hydroPlotter.SequenceKDTransMembrane[hydroPlotter.SequenceKDTransMembrane.Length - 1]; ++i)
            {
                Point p = new Point(location, 300 - (hydroPlotter.SequenceKDTransMembrane[i] * ScaleFactor));
                transMem_KD.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(transMem_KD);

            Polyline surfaceRegion_KD = new Polyline();
            surfaceRegion_KD.StrokeThickness = 4;
            surfaceRegion_KD.Stroke = Brushes.BlanchedAlmond;
            surfaceRegion_KD.Name = "surfaceRegion_KD";
            firstPoint = true;
            offset = 25 * (surfMem / 2);
            location = 15 + offset; 
            for (var i = (int)hydroPlotter.SequenceKDSurfaceRegions[0]; i < hydroPlotter.SequenceKDSurfaceRegions[hydroPlotter.SequenceKDSurfaceRegions.Length - 1]; ++i)
            {
                Point p = new Point(location, 300 - (hydroPlotter.SequenceKDSurfaceRegions[i] * ScaleFactor));
                surfaceRegion_KD.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(surfaceRegion_KD);

            Polyline transMem_HW = new Polyline();
            transMem_HW.StrokeThickness = 4;
            transMem_HW.Stroke = Brushes.BlueViolet;
            transMem_HW.Name = "transMem_HW";
            firstPoint = true;
            offset = 25 * (transMem / 2);
            location = 15 + offset; 
            for (var i = (int)hydroPlotter.SequenceHWTransMembrane[0]; i < hydroPlotter.SequenceHWTransMembrane[hydroPlotter.SequenceHWTransMembrane.Length - 1]; ++i)
            {
                Point p = new Point(location, 300 - (hydroPlotter.SequenceHWTransMembrane[i] * ScaleFactor));
                transMem_HW.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(transMem_HW);

            Polyline surfaceRegion_HW = new Polyline();
            surfaceRegion_HW.StrokeThickness = 4;
            surfaceRegion_HW.Stroke = Brushes.BlanchedAlmond;
            surfaceRegion_HW.Name = "surfaceRegion_HW";
            firstPoint = true;
            offset = 25 * (surfMem / 2);
            location = 15 + offset;
            for (var i = (int)hydroPlotter.SequenceHWSurfaceRegions[0]; i < hydroPlotter.SequenceHWSurfaceRegions[hydroPlotter.SequenceHWSurfaceRegions.Length - 1]; ++i)
            {
                Point p = new Point(location, 300 - (hydroPlotter.SequenceHWSurfaceRegions[i] * ScaleFactor));
                surfaceRegion_HW.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(surfaceRegion_HW);




            SurfaceRegion_TransMembrane.IsChecked = true;
            Kyte_Doolittle.IsChecked = true;
        }

        /// <summary>
        /// Creates a custom window size hydropathy plot minimum size = 5 max size = 30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateCustomWindow_Clicked(object sender, RoutedEventArgs e)
        {
            // custom window exists, delete custom windows -- reset for new window size
            if (CustomWindowActive)
            {
                if (setupData.Sequence != "")
                {
                    for (int i = VisualTreeHelper.GetChildrenCount(this.GraphGrid) - 1; i > 0; --i)
                    {
                        var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                        if (child != null && child.Name == "customWindow_KD")
                        {
                            GraphGrid.Children.Remove(child);
                        }
                        if (child != null && child.Name == "customWindow_HW")
                        {
                            GraphGrid.Children.Remove(child);
                        }

                    }
                }

                hydroPlotter.SequenceCustomWindowHW = new float[hydroPlotter.Sequence.Length];
                hydroPlotter.SequenceCustomWindowKD = new float[hydroPlotter.Sequence.Length];
                CustomWindowActive = false;
                SurfaceRegion_TransMembrane.IsChecked = true;
                CustomWindowButton.Content = "Create";
                HydropathyWindowSize.Text = "";
            }
            else // no window exists, create new custome window for both KD and HW
            {
                CustomWindowActive = true;
                CustomWindowButton.Content = "Reset";
                int windowsize = Convert.ToInt32(this.HydropathyWindowSize.Text);
                hydroPlotter.RunCustomWindow(windowsize, hydroPlotter.SequenceCustomWindowKD, "KD"); // run custom window with Kyte Doolittle values
                hydroPlotter.RunCustomWindow(windowsize, hydroPlotter.SequenceCustomWindowHW, "HW"); // run custom window with Hopp Woods Values

                Polyline Custom_KD = new Polyline();
                Custom_KD.StrokeThickness = 4;
                Custom_KD.Stroke = Brushes.CornflowerBlue;
                Custom_KD.Name = "customWindow_KD";
                bool firstPoint = true;
                int offset = 25 * (windowsize / 2);
                int location = 15 + offset; 
                for (var i = (int)hydroPlotter.SequenceCustomWindowKD[0]; i < hydroPlotter.SequenceCustomWindowKD[hydroPlotter.SequenceCustomWindowKD.Length - 1]; ++i)
                {
                    Point p = new Point(location, 300 - (hydroPlotter.SequenceCustomWindowKD[i] * ScaleFactor));
                    Custom_KD.Points.Add(p);
                    firstPoint = false;
                    location += 25;
                }
                GraphGrid.Children.Add(Custom_KD);

                Polyline Custom_HW = new Polyline();
                Custom_HW.StrokeThickness = 4;
                Custom_HW.Stroke = Brushes.CornflowerBlue;
                Custom_HW.Name = "customWindow_HW";
                firstPoint = true;
                offset = 25 * (windowsize / 2);
                location = 15 + offset;
                for (var i = (int)hydroPlotter.SequenceCustomWindowHW[0]; i < hydroPlotter.SequenceCustomWindowHW[hydroPlotter.SequenceCustomWindowHW.Length - 1]; ++i)
                {
                    Point p = new Point(location, 300 - (hydroPlotter.SequenceCustomWindowHW[i] * ScaleFactor));
                    Custom_HW.Points.Add(p);
                    firstPoint = false;
                    location += 25;
                }
                GraphGrid.Children.Add(Custom_HW);
                ShowAllHydroPlots.IsChecked = true;
            }
        }

        private void TransMembraneOnly_Checked(object sender, RoutedEventArgs e)
        {
            Polyline transMemCntl, surfaceCntl;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                    if (Kyte_Doolittle.IsChecked == true)
                    {
                        if (child != null && child.Name == "transMem_KD")
                        {
                            transMemCntl = (Polyline)child;
                            transMemCntl.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "surfaceRegion_KD")
                        {
                            surfaceCntl = (Polyline)child;
                            surfaceCntl.Visibility = System.Windows.Visibility.Hidden;
                        }
                    }
                    else
                    {
                        if (child != null && child.Name == "transMem_HW")
                        {
                            transMemCntl = (Polyline)child;
                            transMemCntl.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "surfaceRegion_HW")
                        {
                            surfaceCntl = (Polyline)child;
                            surfaceCntl.Visibility = System.Windows.Visibility.Hidden;
                        }
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
                    if (Kyte_Doolittle.IsChecked == true)
                    {
                        if (child != null && child.Name == "transMem_KD")
                        {
                            transMemCntl = (Polyline)child;
                            transMemCntl.Visibility = System.Windows.Visibility.Hidden;
                        }
                        if (child != null && child.Name == "surfaceRegion_KD")
                        {
                            surfaceCntl = (Polyline)child;
                            surfaceCntl.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (child != null && child.Name == "transMem_HW")
                        {
                            transMemCntl = (Polyline)child;
                            transMemCntl.Visibility = System.Windows.Visibility.Hidden;
                        }
                        if (child != null && child.Name == "surfaceRegion_HW")
                        {
                            surfaceCntl = (Polyline)child;
                            surfaceCntl.Visibility = System.Windows.Visibility.Visible;
                        }
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
                    if (Kyte_Doolittle.IsChecked == true)
                    {
                        if (child != null && child.Name == "transMem_KD")
                        {
                            transMemCntl = (Polyline)child;
                            transMemCntl.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "surfaceRegion_KD")
                        {
                            surfaceCntl = (Polyline)child;
                            surfaceCntl.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (child != null && child.Name == "transMem_HW")
                        {
                            transMemCntl = (Polyline)child;
                            transMemCntl.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "surfaceRegion_HW")
                        {
                            surfaceCntl = (Polyline)child;
                            surfaceCntl.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void ShowAllHydroPlots_Checked(object sender, RoutedEventArgs e)
        {
            Polyline testLine;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                    if (Kyte_Doolittle.IsChecked == true)
                    {
                        if (child != null && child.Name == "transMem_KD")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "surfaceRegion_KD")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "customWindow_KD")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "transMem_HW")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Hidden;
                        }
                        if (child != null && child.Name == "surfaceRegion_HW")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Hidden;
                        }
                        if (child != null && child.Name == "customWindow_HW")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Hidden;
                        }
                    }
                    else
                    {
                        if (child != null && child.Name == "transMem_KD")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Hidden;
                        }
                        if (child != null && child.Name == "surfaceRegion_KD")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Hidden;
                        }
                        if (child != null && child.Name == "customWindow_KD")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Hidden;
                        }
                        if (child != null && child.Name == "transMem_HW")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "surfaceRegion_HW")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (child != null && child.Name == "customWindow_HW")
                        {
                            testLine = (Polyline)child;
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void HideAllHydroPlots_Checked(object sender, RoutedEventArgs e)
        {
            Polyline transMemCntl, surfaceCntl;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                    if (child != null && (child.Name == "transMem_KD" || child.Name == "transMem_HW"))
                    {
                        transMemCntl = (Polyline)child;
                        transMemCntl.Visibility = System.Windows.Visibility.Hidden;
                    }
                    if (child != null && (child.Name == "surfaceRegion_KD" || child.Name == "surfaceRegion_HW"))
                    {
                        surfaceCntl = (Polyline)child;
                        surfaceCntl.Visibility = System.Windows.Visibility.Hidden;
                    }
                    if (child != null && (child.Name == "customWindow_KD" || child.Name == "customWindow_HW"))
                    {
                        surfaceCntl = (Polyline)child;
                        surfaceCntl.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
        }

        private void CustomWindow_Checked(object sender, RoutedEventArgs e)
        {
            Polyline testLine;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;

                    if (child != null && (child.Name == "customWindow_KD" || child.Name == "customWindow_HW"))
                    {
                        testLine = (Polyline)child;
                        if (Kyte_Doolittle.IsChecked == true)
                        {
                            
                            if (child.Name == "customWindow_KD")
                            {
                                testLine.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                testLine.Visibility = System.Windows.Visibility.Hidden;
                            }
                        }
                        else
                        {
                            if (child.Name == "customWindow_HW")
                            {
                                testLine.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                testLine.Visibility = System.Windows.Visibility.Hidden;
                            }
                        }
                    }

                    
                    
                    if (child != null && (child.Name == "transMem_KD" || child.Name == "transMem_HW"))
                    {
                        testLine = (Polyline)child;
                        testLine.Visibility = System.Windows.Visibility.Hidden;
                    }
                    if (child != null && (child.Name == "surfaceRegion_KD" || child.Name == "surfaceRegion_HW"))
                    {
                        testLine = (Polyline)child;
                        testLine.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
        }


        private void KyteDoolittle_Checked(object sender, RoutedEventArgs e)
        {
            Polyline testLine;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                    if (child != null && child.Name == "transMem_KD")
                    {
                        testLine = (Polyline)child;
                        if (ShowAllHydroPlots.IsChecked == true || SurfaceRegion_TransMembrane.IsChecked == true || TransMembrane.IsChecked == true)
                        {
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    if (child != null && child.Name == "surfaceRegion_KD")
                    {
                        testLine = (Polyline)child;
                        if (ShowAllHydroPlots.IsChecked == true || SurfaceRegion_TransMembrane.IsChecked == true || SurfaceRegion.IsChecked == true)
                        {
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    if (child != null && child.Name == "customWindow_KD")
                    {
                        testLine = (Polyline)child;
                        if (ShowAllHydroPlots.IsChecked == true || SurfaceRegion_TransMembrane.IsChecked == true || SurfaceRegion.IsChecked == true)
                        {
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    if (child != null && child.Name == "transMem_HW")
                    {
                        testLine = (Polyline)child;
                        testLine.Visibility = System.Windows.Visibility.Hidden;
                    }
                    if (child != null && child.Name == "surfaceRegion_HW")
                    {
                        testLine = (Polyline)child;
                        testLine.Visibility = System.Windows.Visibility.Hidden;
                    }
                    if (child != null && child.Name == "customWindow_HW")
                    {
                        testLine = (Polyline)child;
                        testLine.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
        }

        private void HoppWoods_Checked(object sender, RoutedEventArgs e)
        {
            Polyline testLine;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;
                    if (child != null && child.Name == "transMem_HW")
                    {
                        testLine = (Polyline)child;
                        if (ShowAllHydroPlots.IsChecked == true || SurfaceRegion_TransMembrane.IsChecked == true || TransMembrane.IsChecked == true)
                        {
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    if (child != null && child.Name == "surfaceRegion_HW")
                    {
                        testLine = (Polyline)child;
                        if (ShowAllHydroPlots.IsChecked == true || SurfaceRegion_TransMembrane.IsChecked == true || SurfaceRegion.IsChecked == true)
                        {
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    if (child != null && child.Name == "customWindow_HW")
                    {
                        testLine = (Polyline)child;
                        if (ShowAllHydroPlots.IsChecked == true || SurfaceRegion_TransMembrane.IsChecked == true || SurfaceRegion.IsChecked == true)
                        {
                            testLine.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    if (child != null && child.Name == "transMem_KD")
                    {
                        testLine = (Polyline)child;
                        testLine.Visibility = System.Windows.Visibility.Hidden;
                    }
                    if (child != null && child.Name == "surfaceRegion_KD")
                    {
                        testLine = (Polyline)child;
                        testLine.Visibility = System.Windows.Visibility.Hidden;
                    }
                    if (child != null && child.Name == "customWindow_KD")
                    {
                        testLine = (Polyline)child;
                        testLine.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
        }


        #endregion

        private void UpdateSize(object sender, SizeChangedEventArgs e)
        {
            double newWidth = PrimaryWindow.ActualWidth;
            double newHeight = PrimaryWindow.ActualHeight;
            double expandVal = 0;
            double growValR = 0;
            double growValD = 0;
                       
            if (newWidth > width && width != 0)
            {
                expandVal = newWidth - width;
                growValR = Results.ActualWidth;
                growValD = DataGridScroller.ActualWidth;
                growValR += expandVal;
                growValD += expandVal;
                DataGridScroller.SetValue(Grid.WidthProperty, growValD);
                Results.SetValue(Grid.WidthProperty, growValR);
            }
            if (newWidth < width && width != 0)
            { 
                expandVal = newWidth - width;
                growValR = Results.ActualWidth;
                growValD = DataGridScroller.ActualWidth;
                growValR += expandVal;
                growValD += expandVal;
                DataGridScroller.SetValue(Grid.WidthProperty, growValD);
                Results.SetValue(Grid.WidthProperty, growValR);
            }
            width = PrimaryWindow.ActualWidth;
            height = PrimaryWindow.ActualHeight;

        }

        private void MouseDownSequence_Clicked(object sender, MouseButtonEventArgs e)
        {

        }
        #endregion

    }
}
