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
using System.Collections;


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
        private Chou_Fasman chou_FasmanPlotter;
        private Frame FrameWithinGrid;
        private TMHMM newTMHMMJob;
        private int tMHMMJobNumberCounter = 0;
        private int TMHMMcurrentJobIndex = 0;
        private ArrayList TMHMMJobList = new ArrayList();
        private double height, width;
        private bool CustomWindowActive = false;
        private int ScaleFactor = 5;
        private int transMem = 19;
        private int surfMem = 9;
        private int tMHMMStart = -1; // set to -1 for error detection and to identify it has not been set
        private Button tMHMMStartButton = null;
        private int tMHMMStop = -1;  // set to -1 for error detection and to identify it has not been set
        private Button tMHMMStopButton = null;
        private Polyline StartMarker = null;
        private Polyline StopMarker = null;
        private bool tmhmmActive = false;
        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            setupData = new SetupData();
            Setup.DataContext = setupData;
            Results_Title.DataContext = setupData;
            InitializeWindow();
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


            #region Column and Row setup
            ColumnDefinition[] columnsSeq = new ColumnDefinition[seqLength * 2];
            ColumnDefinition[] columnsChouFasman = new ColumnDefinition[seqLength * 2];
            for (int i = 0; i < columnsSeq.Length; ++i)
            {
                columnsSeq[i] = new ColumnDefinition();
                columnsChouFasman[i] = new ColumnDefinition();
                if (i % 2 == 0) // even column is a spacer width 5
                {
                    columnsSeq[i].Width = new GridLength(5, GridUnitType.Pixel);
                    columnsChouFasman[i].Width = new GridLength(5, GridUnitType.Pixel);
                }
                else // odd column is a data column 30 width
                {
                    columnsSeq[i].Width = new GridLength(20, GridUnitType.Pixel);
                    columnsChouFasman[i].Width = new GridLength(20, GridUnitType.Pixel);
                }
                DataGrid.ColumnDefinitions.Add(columnsSeq[i]);
                ChouFasmanGrid.ColumnDefinitions.Add(columnsChouFasman[i]);
            }

            RowDefinition[] rowsSeq = new RowDefinition[2];
            RowDefinition[] rowsChouFasman = new RowDefinition[5];
            for (int i = 0; i < rowsSeq.Length; ++i)
            {
                rowsSeq[i] = new RowDefinition();
            }
            for (int i = 0; i < rowsChouFasman.Length; ++i)
            {
                rowsChouFasman[i] = new RowDefinition();
            }
            rowsSeq[0].Height = new GridLength(20, GridUnitType.Pixel);
            rowsSeq[1].Height = new GridLength(20, GridUnitType.Pixel);
            rowsChouFasman[0].Height = new GridLength(20, GridUnitType.Pixel);
            rowsChouFasman[1].Height = new GridLength(20, GridUnitType.Pixel);
            rowsChouFasman[2].Height = new GridLength(20, GridUnitType.Pixel);
            rowsChouFasman[3].Height = new GridLength(20, GridUnitType.Pixel);
            rowsChouFasman[4].Height = new GridLength(20, GridUnitType.Pixel);
            rowsChouFasman[0].Name = "HelixCharRow";
            rowsChouFasman[1].Name = "HelixDataRow";
            rowsChouFasman[2].Name = "BetaCharRow";
            rowsChouFasman[3].Name = "BetaDataRow";
            rowsChouFasman[4].Name = "CoilLoopRow";
            foreach (RowDefinition r in rowsSeq)
            {
                DataGrid.RowDefinitions.Add(r);
            }
            foreach (RowDefinition r in rowsChouFasman)
            {
                ChouFasmanGrid.RowDefinitions.Add(r);
            } 
            #endregion

            #region Sequence button creation and number labels
            //DataGrid.ShowGridLines = true;
            Button[] seq = new Button[seqLength];
            TextBlock[] indexNumbers = new TextBlock[seqLength];
            for (int i = 0; i < seq.Length; ++i)
            {
                seq[i] = new Button();
                indexNumbers[i] = new TextBlock();
            }


            
            //create sequence buttons and residue sequence numbers
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
            #endregion

            #region Chou Fasman Data
            //Helix data
            TextBlock[] HelixNumbers = new TextBlock[seqLength];
            TextBlock[] HelixChars = new TextBlock[seqLength];
            for (int i = 0; i < seq.Length; ++i)
            {
                HelixNumbers[i] = new TextBlock();
                HelixChars[i] = new TextBlock();
            }
            for (int i = 0; i < seqLength; ++i)
            {
                string dataNames = "HelixData_" + i;
                string charNames = "HelixChar_" + i;

                int colNum = (i * 2) + 1;
                HelixChars[i].Text = chou_FasmanPlotter.ChouFasmanResultCharAlphaHelix[i] + "";
                HelixChars[i].FontSize = 12;
                HelixChars[i].Name = charNames;
                HelixChars[i].TextWrapping = TextWrapping.Wrap;
                HelixChars[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                HelixChars[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                HelixChars[i].Foreground = Brushes.Gold ;
                HelixChars[i].Background = Brushes.Transparent;
                HelixChars[i].SetValue(Grid.RowProperty, 0);
                HelixChars[i].SetValue(Grid.ColumnProperty, (colNum));
                ChouFasmanGrid.Children.Add(HelixChars[i]);
                HelixNumbers[i].Text = chou_FasmanPlotter.ChouFasmanResultAlphaHelix[i].ToString("N2");
                HelixNumbers[i].FontSize = 9;
                HelixNumbers[i].Name = dataNames;
                HelixNumbers[i].TextWrapping = TextWrapping.Wrap;
                HelixNumbers[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                HelixNumbers[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                HelixNumbers[i].Foreground = Brushes.Gold;
                HelixNumbers[i].Background = Brushes.Transparent;
                HelixNumbers[i].SetValue(Grid.RowProperty, 1);
                HelixNumbers[i].SetValue(Grid.ColumnProperty, (colNum));
                ChouFasmanGrid.Children.Add(HelixNumbers[i]);
            } 
            
            TextBlock[] BetaNumbers = new TextBlock[seqLength];
            TextBlock[] BetaChars = new TextBlock[seqLength];
            for (int i = 0; i < seq.Length; ++i)
            {
                BetaNumbers[i] = new TextBlock();
                BetaChars[i] = new TextBlock();
            }
            for (int i = 0; i < seqLength; ++i)
            {
                string dataNames = "BetaData_" + i;
                string charNames = "BetaChar_" + i;
                int colNum = (i * 2) + 1;
                BetaChars[i].Text = chou_FasmanPlotter.ChouFasmanResultCharBetaSheet[i] + "";
                BetaChars[i].FontSize = 12;
                BetaChars[i].Name = charNames;
                BetaChars[i].TextWrapping = TextWrapping.Wrap;
                BetaChars[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                BetaChars[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                BetaChars[i].Foreground = Brushes.DarkMagenta;
                BetaChars[i].Background = Brushes.Transparent;
                BetaChars[i].SetValue(Grid.RowProperty, 2);
                BetaChars[i].SetValue(Grid.ColumnProperty, (colNum));
                ChouFasmanGrid.Children.Add(BetaChars[i]);
                BetaNumbers[i].Text = chou_FasmanPlotter.ChouFasmanResultBetaSheet[i].ToString("N2");
                BetaNumbers[i].FontSize = 9;
                BetaNumbers[i].Name = dataNames;
                BetaNumbers[i].TextWrapping = TextWrapping.Wrap;
                BetaNumbers[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                BetaNumbers[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                BetaNumbers[i].Foreground = Brushes.DarkMagenta;
                BetaNumbers[i].Background = Brushes.Transparent;
                BetaNumbers[i].SetValue(Grid.RowProperty, 3);
                BetaNumbers[i].SetValue(Grid.ColumnProperty, (colNum));
                ChouFasmanGrid.Children.Add(BetaNumbers[i]);
            }

            ChouFasmanShow_All.IsChecked = true;
            #endregion


        } 
        #endregion
        #region Private Methods
        private void InitializeWindow()
        {
            hydroPlotter = new HydropathyPlotter();
            height = PrimaryWindow.ActualHeight;
            width = PrimaryWindow.ActualWidth;
            selectedResidueButton = new Button();
            chou_FasmanPlotter = new Chou_Fasman();
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
        private void SeqButton_Click(object sender, EventArgs e)
        {
            if (tMHMMStartButton == null)
            {
                tMHMMStartButton = (Button)sender;
                tMHMMStartButton.Foreground = Brushes.Gold;
                tMHMMStartButton.Background = Brushes.Green;
                string name = tMHMMStartButton.Name;
                string[] tokens = name.Split('_');
                tMHMMStart = Convert.ToInt32(tokens[1]);
                GraphGrid.Children.Remove(StartMarker);
                int startPoint = 15 + (25 * tMHMMStart);
                StartMarker = new Polyline();
                StartMarker.StrokeThickness = 2;
                StartMarker.Stroke = Brushes.Green;
                Point MarkerStartGreen = new Point(startPoint, 0);
                Point MarkerStopGreen = new Point(startPoint, 525);
                StartMarker.Points.Add(MarkerStartGreen);
                StartMarker.Points.Add(MarkerStopGreen);
                StartMarker.Visibility = System.Windows.Visibility.Visible;
                GraphGrid.Children.Add(StartMarker);

            }
            else if (tMHMMStartButton == (Button)sender)
            {
                for (int i = tMHMMStart + 1; i < tMHMMStop; ++i)
                {
                    string blockNum = "tBlock_" + i;
                    Button b = (Button)(LogicalTreeHelper.FindLogicalNode(this.DataGrid, blockNum));
                    b.Foreground = Brushes.Red;
                    b.Background = Brushes.Black;
                }
                tMHMMStartButton.Foreground = Brushes.Red;
                tMHMMStartButton.Background = Brushes.Black;
                tMHMMStart = -1;
                tMHMMStartButton = null;
                GraphGrid.Children.Remove(StartMarker);
                if (tMHMMStopButton != null)
                {
                    tMHMMStopButton.Foreground = Brushes.Red;
                    tMHMMStopButton.Background = Brushes.Black;
                    tMHMMStop = -1;
                    tMHMMStopButton = null;
                    GraphGrid.Children.Remove(StopMarker);
                }
                TMHMMGrid.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (tMHMMStopButton == null)
            {
                tMHMMStopButton = (Button)sender;
                tMHMMStopButton.Foreground = Brushes.Gold;
                tMHMMStopButton.Background = Brushes.Red;
                string name = tMHMMStopButton.Name;
                string[] tokens = name.Split('_');
                tMHMMStop = Convert.ToInt32(tokens[1]);
                GraphGrid.Children.Remove(StopMarker);
                int stopPoint = 15 + (25 * tMHMMStop);
                StopMarker = new Polyline();
                StopMarker.StrokeThickness = 2;
                StopMarker.Stroke = Brushes.Red;
                Point MarkerStartRed = new Point(stopPoint, 0);
                Point MarkerStopRed = new Point(stopPoint, 525);
                StopMarker.Points.Add(MarkerStartRed);
                StopMarker.Points.Add(MarkerStopRed);
                StopMarker.Visibility = System.Windows.Visibility.Visible;
                GraphGrid.Children.Add(StopMarker);
            }
            else if (tMHMMStopButton == (Button)sender)
            {
                for (int i = tMHMMStart + 1; i < tMHMMStop; ++i)
                {
                    string blockNum = "tBlock_" + i;
                    Button b = (Button)(LogicalTreeHelper.FindLogicalNode(this.DataGrid, blockNum));
                    b.Foreground = Brushes.Red;
                    b.Background = Brushes.Black;
                }
                tMHMMStopButton.Foreground = Brushes.Red;
                tMHMMStopButton.Background = Brushes.Black;
                tMHMMStop = -1;
                tMHMMStopButton = null;
                GraphGrid.Children.Remove(StopMarker);
                TMHMMGrid.Visibility = System.Windows.Visibility.Hidden;
            }

            if (tMHMMStartButton != null && tMHMMStopButton != null)
            {
                if (tMHMMStop < tMHMMStart)
                {
                    //stop button is in front of start button swap buttons and fill in
                    Button temp = tMHMMStartButton;
                    tMHMMStartButton = tMHMMStopButton;
                    tMHMMStopButton = temp;
                    tMHMMStartButton.Foreground = Brushes.Gold;
                    tMHMMStartButton.Background = Brushes.Green;
                    tMHMMStopButton.Foreground = Brushes.Gold;
                    tMHMMStopButton.Background = Brushes.Red;
                    int tempInt = tMHMMStart;
                    tMHMMStart = tMHMMStop;
                    tMHMMStop = tempInt;
                    Polyline temp2 = StartMarker;
                    StartMarker = StopMarker;
                    StopMarker = temp2;
                    StartMarker.Stroke = Brushes.Green;
                    StopMarker.Stroke = Brushes.Red;
                    
                }
                //fill in buttons between start and stop and display popup to run tmhmm
                int seqLength = tMHMMStop - tMHMMStart;
                for (int i = tMHMMStart + 1; i < tMHMMStop; ++i)
                {
                    string blockNum = "tBlock_" + i;
                    Button b = (Button)(LogicalTreeHelper.FindLogicalNode(this.DataGrid, blockNum));
                    b.Foreground = Brushes.Gold;
                    b.Background = Brushes.LightSlateGray;
                }
                TMHMMGrid.Visibility = System.Windows.Visibility.Visible;
                seqLengthTmhmm.Content = seqLength;
            }
        }
        private void StartAnalysisTool_Click(object sender, RoutedEventArgs e)
        {
            InitializeWindow();
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
                chou_FasmanPlotter.RunChouFasman(setupData.Sequence);
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
            if (ChouFasmanGrid.Children.Count > 0)
            {
                ChouFasmanGrid.Children.Clear();
                ChouFasmanGrid.ColumnDefinitions.Clear();
                ChouFasmanGrid.RowDefinitions.Clear();
            }
            tMHMMJobNumberCounter = 0;
            if (TMHMMJobSelectionComboBox.Items.Count > 0)
            {
                for (int i = TMHMMJobSelectionComboBox.Items.Count - 1; i > 0; --i)
                {
                    TMHMMJobSelectionComboBox.Items.RemoveAt(i);
                }
            }
            if (TMHMMJobList.Count > 0)
            {
                TMHMMJobList.Clear();
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
            hydroPlotter = null;
            chou_FasmanPlotter = null;
            
            selectedResidueButton = null;
            newTMHMMJob = null;
            FrameWithinGrid = null;
            tMHMMStartButton = null;
            tMHMMStopButton = null;
            StartMarker = null;
            StopMarker = null;
            tmhmmActive = false;
            tMHMMStart = -1;
            tMHMMStop = -1;
            CustomWindowActive = false;
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
        private void UpdateSize(object sender, SizeChangedEventArgs e)
        {
            double newWidth = PrimaryWindow.ActualWidth;
            double newHeight = PrimaryWindow.ActualHeight;
            double expandVal = 0;
            double growValR = 0;
            double growValD = 0;
            if (setupData.Sequence == "")
            {
                if (newWidth > width && width != 0)
                {
                    expandVal = newWidth - width;
                    growValR = Results.ActualWidth;
                    //growValD = DataGridScroller.ActualWidth;
                    growValR += expandVal;
                    //growValD += expandVal;
                    //DataGridScroller.SetValue(Grid.WidthProperty, growValD);
                    Results.SetValue(Grid.WidthProperty, growValR);
                }
                if (newWidth < width && width != 0)
                {
                    expandVal = newWidth - width;
                    growValR = Results.ActualWidth;
                    //growValD = DataGridScroller.ActualWidth;
                    growValR += expandVal;
                    //growValD += expandVal;
                    //DataGridScroller.SetValue(Grid.WidthProperty, growValD);
                    Results.SetValue(Grid.WidthProperty, growValR);
                }
            }
            else
            {
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
            }
            width = PrimaryWindow.ActualWidth;
            height = PrimaryWindow.ActualHeight;
        }

        /// <summary>
        /// Catches mouse clicks on sequence buttons to set up TMHMM query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownSequence_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (tMHMMStart == null)
            {

            }
        }
        #endregion
        #region Hydropathy Conrols

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
        #region ChouFasman Controls
        private void AlphaHelix_Checked(object sender, RoutedEventArgs e)
        {
            TextBlock tb;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.ChouFasmanGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.ChouFasmanGrid, i) as FrameworkElement;
                    if (child != null && child.Name.Contains("HelixData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (child != null && child.Name.Contains("HelixChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (child != null && child.Name.Contains("BetaData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("BetaChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    //if (child != null && child.Name.Contains("LoopData_"))
                    //{
                    //    tb = (TextBlock)child;
                    //    tb.Visibility = System.Windows.Visibility.Collapsed;
                    //}
                }
            }
        }

        private void BetaSheet_Checked(object sender, RoutedEventArgs e)
        {
            TextBlock tb;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.ChouFasmanGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.ChouFasmanGrid, i) as FrameworkElement;
                    if (child != null && child.Name.Contains("HelixData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("HelixChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("BetaData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (child != null && child.Name.Contains("BetaChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Visible;
                    }
                    //if (child != null && child.Name.Contains("LoopData_"))
                    //{
                    //    tb = (TextBlock)child;
                    //    tb.Visibility = System.Windows.Visibility.Collapsed;
                    //}
                }
            }
        }

        private void LoopCoil_Checked(object sender, RoutedEventArgs e)
        {
            TextBlock tb;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.ChouFasmanGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.ChouFasmanGrid, i) as FrameworkElement;
                    if (child != null && child.Name.Contains("HelixData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("HelixChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("BetaData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("BetaChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    //if (child != null && child.Name.Contains("LoopData_"))
                    //{
                    //    tb = (TextBlock)child;
                    //    tb.Visibility = System.Windows.Visibility.Visible;
                    //}
                }
            }
        }

        private void ChouFasmanShow_All_Checked(object sender, RoutedEventArgs e)
        {
            TextBlock tb;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.ChouFasmanGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.ChouFasmanGrid, i) as FrameworkElement;
                    if (child != null && child.Name.Contains("HelixData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (child != null && child.Name.Contains("HelixChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (child != null && child.Name.Contains("BetaData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (child != null && child.Name.Contains("BetaChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Visible;
                    }
                    //if (child != null && child.Name.Contains("LoopData_"))
                    //{
                    //    tb = (TextBlock)child;
                    //    tb.Visibility = System.Windows.Visibility.Visible;
                    //}
                }
            }
        }
        private void ChouFasmanHide_All_Checked(object sender, RoutedEventArgs e)
        {
            TextBlock tb;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.ChouFasmanGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.ChouFasmanGrid, i) as FrameworkElement;
                    if (child != null && child.Name.Contains("HelixData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("HelixChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("BetaData_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    if (child != null && child.Name.Contains("BetaChar_"))
                    {
                        tb = (TextBlock)child;
                        tb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    //if (child != null && child.Name.Contains("LoopData_"))
                    //{
                    //    tb = (TextBlock)child;
                    //    tb.Visibility = System.Windows.Visibility.Collapsed;
                    //}
                }
            }
        }
        #endregion
        #region TMHMM Controls

        private void StartTMHMMButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (tmhmmActive)
            {
                //clear current tmhmm run
                SeqButton_Click(tMHMMStartButton, null);
                StartTmhmm_Button.Visibility = System.Windows.Visibility.Visible;
                TMHMMDataGrid.Visibility = System.Windows.Visibility.Collapsed;
                TMHMMSequenceTextBox.Text = "";
                TMHMMDataTextBox.Text = "";
                tmhmmActive = false;
                FrameWithinGrid = null;

            }
            else
            {
                string jobID = "Job " + tMHMMJobNumberCounter;
                TMHMMJobIDTextBox.Text = jobID;
                tMHMMJobNumberCounter++;
                string tmhmmSequence = "";
                for (int i = tMHMMStart; i < tMHMMStop; ++i)
                {
                    tmhmmSequence += setupData.Sequence[i];
                }
                newTMHMMJob = new TMHMM(tmhmmSequence, jobID, tMHMMStart, tMHMMStop);
                
                StartTmhmm_Button.Visibility = System.Windows.Visibility.Collapsed;
                TMHMMDataGrid.Visibility = System.Windows.Visibility.Visible;
                TMHMMSequenceTextBox.Text = tmhmmSequence;
                TMHMMDataTextBox.Text = "";
                webWindow.Visibility = System.Windows.Visibility.Visible;
                FrameWithinGrid = new Frame();
                webWindow.Children.Add(FrameWithinGrid);
                FrameWithinGrid.Source = new Uri("http://www.cbs.dtu.dk/services/TMHMM/", UriKind.RelativeOrAbsolute);


                tmhmmActive = true;
            }
        }

        private void TMHMMSubmitButton_Clicked(object sender, RoutedEventArgs e)
        {
            //parse data into TMHMM Class then display graphical data in the correct place on screen
            if (TMHMMDataTextBox.Text != "")
            {
                if (newTMHMMJob.ParseTMHMMData(TMHMMDataTextBox.Text))
                {
                    TMHMMJobList.Add(newTMHMMJob);
                    DrawTMHMMGraph(newTMHMMJob);
                    TMHMMJobSelectionComboBox.Items.Add(newTMHMMJob.JobID);
                    TMHMMJobSelectionComboBox.SelectedIndex = TMHMMJobSelectionComboBox.Items.Count - 1;

                    
                    //clear current tmhmm run
                    SeqButton_Click(tMHMMStartButton, null);
                    StartTmhmm_Button.Visibility = System.Windows.Visibility.Visible;
                    TMHMMDataGrid.Visibility = System.Windows.Visibility.Collapsed;
                    TMHMMSequenceTextBox.Text = "";
                    TMHMMDataTextBox.Text = "";
                    webWindow.Visibility = System.Windows.Visibility.Collapsed;
                    tmhmmActive = false;
                    FrameWithinGrid = null;
                    TMHMMShowAll_RadioButton.IsChecked = true;
                }
            }
            else
            {
                //TODO show error
            }
        }

        private void TMHMMCancelButton_Clicked(object sender, RoutedEventArgs e)
        {
            //Cancel the entire TMHMM process
            SeqButton_Click(tMHMMStartButton, null);
            StartTmhmm_Button.Visibility = System.Windows.Visibility.Visible;
            TMHMMDataGrid.Visibility = System.Windows.Visibility.Collapsed;
            TMHMMGrid.Visibility = System.Windows.Visibility.Collapsed;
            webWindow.Visibility = System.Windows.Visibility.Collapsed;
            TMHMMSequenceTextBox.Text = "";
            TMHMMDataTextBox.Text = "";
            tmhmmActive = false;
            newTMHMMJob = null;
        }

        private void TMHMMHelpButton_Clicked(object sender, RoutedEventArgs e)
        {
            //pop up window with directions
            PopUp popup = new PopUp();
            popup.ShowDialog();
            if (popup.OK)
            {
            }
        }

        private void DrawTMHMMGraph(TMHMM currentJob)
        {
            Polyline insideDataPolyline = new Polyline();
            insideDataPolyline.StrokeThickness = 4;
            insideDataPolyline.Stroke = Brushes.Cyan;
            string name = "TMHMM_" + currentJob.JobID + "_InsideData";
            insideDataPolyline.Name = name;
            bool firstPoint = true;
            int offset = 25 * (currentJob.Start);
            int location = 15 + offset;
            for (var i = 0; i < currentJob.InsideData.Length; ++i)
            {
                Point p = new Point(location, 600 - (currentJob.InsideData[i] * ScaleFactor * 100));
                insideDataPolyline.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(insideDataPolyline);

            Polyline outsideDataPolyline = new Polyline();
            outsideDataPolyline.StrokeThickness = 4;
            outsideDataPolyline.Stroke = Brushes.ForestGreen;
            name = "TMHMM_" + currentJob.JobID + "_OutsideData";
            outsideDataPolyline.Name = name;
            firstPoint = true;
            offset = 25 * (currentJob.Start);
            location = 15 + offset;
            for (var i = 0; i < currentJob.OutsideData.Length; ++i)
            {
                Point p = new Point(location, 600 - (currentJob.OutsideData[i] * ScaleFactor * 100));
                outsideDataPolyline.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(outsideDataPolyline);

            Polyline membrDataPolyline = new Polyline();
            membrDataPolyline.StrokeThickness = 4;
            membrDataPolyline.Stroke = Brushes.DarkRed;
            name = "TMHMM_" + currentJob.JobID + "_MembrData";
            membrDataPolyline.Name = name;
            firstPoint = true;
            offset = 25 * (currentJob.Start);
            location = 15 + offset;
            for (var i = 0; i < currentJob.MembrData.Length; ++i)
            {
                Point p = new Point(location, 600 - (currentJob.MembrData[i] * ScaleFactor * 100));
                membrDataPolyline.Points.Add(p);
                firstPoint = false;
                location += 25;
            }
            GraphGrid.Children.Add(membrDataPolyline);

        }

        private void DeleteTMHMMGraph()
        {
            TMHMMcurrentJobIndex = TMHMMJobSelectionComboBox.SelectedIndex;
            string name = TMHMMJobSelectionComboBox.Text;
            name = name;
            Polyline line;
            if (setupData.Sequence != "")
            {
                for (int i = VisualTreeHelper.GetChildrenCount(this.GraphGrid) - 1; i > 0; --i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;

                    if (child != null && child.Name.Contains(name))
                    {
                        GraphGrid.Children.Remove(child);
                    }
                }
                TMHMMJobList.RemoveAt(TMHMMcurrentJobIndex);
                TMHMMJobSelectionComboBox.Items.RemoveAt(TMHMMcurrentJobIndex);
                if (TMHMMJobSelectionComboBox.Items.Count > 0)
                {
                    TMHMMJobSelectionComboBox.SelectedIndex = 0;
                }

            }
        }

        private void TMHMMinnerRegions_Checked(object sender, RoutedEventArgs e)
        {

            TMHMMcurrentJobIndex = TMHMMJobSelectionComboBox.SelectedIndex;
            string name = TMHMMJobSelectionComboBox.Text;
            name = "TMHMM_" + name + "_InsideData";
            Polyline line;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;

                    if (child != null && child.Name == name)
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Visible;
                    }
                    else if (child != null && child.Name.Contains("_InsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_MembrData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_OutsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }

                }
            }
        }

        private void TMHMMouterRegions_Checked(object sender, RoutedEventArgs e)
        {
            TMHMMcurrentJobIndex = TMHMMJobSelectionComboBox.SelectedIndex;
            string name = TMHMMJobSelectionComboBox.Text;
            name = "TMHMM_" + name + "_OutsideData";
            Polyline line;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;

                    if (child != null && child.Name == name)
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Visible;
                    }
                    else if (child != null && child.Name.Contains("_InsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_MembrData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_OutsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }

                }
            }
        }

        private void TMHMMmembrRegions_Checked(object sender, RoutedEventArgs e)
        {
            TMHMMcurrentJobIndex = TMHMMJobSelectionComboBox.SelectedIndex;
            string name = TMHMMJobSelectionComboBox.Text;
            name = "TMHMM_" + name + "_MembrData";
            Polyline line;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;

                    if (child != null && child.Name == name)
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Visible;
                    }
                    else if (child != null && child.Name.Contains("_InsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_MembrData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_OutsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }

                }
            }
        }

        private void TMHMMshowAll_Checked(object sender, RoutedEventArgs e)
        {
            TMHMMcurrentJobIndex = TMHMMJobSelectionComboBox.SelectedIndex;
            string name = TMHMMJobSelectionComboBox.Text;
            name = "TMHMM_" + name;
            Polyline line;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;

                    if (child != null && child.Name.Contains(name))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Visible;
                    }
                    else if (child != null && child.Name.Contains("_InsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_MembrData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_OutsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }

                }
            }
        }

        private void TMHMMhideAll_Checked(object sender, RoutedEventArgs e)
        {
            TMHMMcurrentJobIndex = TMHMMJobSelectionComboBox.SelectedIndex;
            string name = TMHMMJobSelectionComboBox.Text;
            name = "TMHMM_" + name;
            Polyline line;
            if (setupData.Sequence != "")
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                {
                    var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;

                    if (child != null && child.Name.Contains(name))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_InsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_MembrData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else if (child != null && child.Name.Contains("_OutsideData"))
                    {
                        line = (Polyline)child;
                        line.Visibility = System.Windows.Visibility.Hidden;
                    }

                }
            }
        }

        private void TMHMMDeleteJobButton_Clicked(object sender, RoutedEventArgs e)
        {
            DeleteTMHMMGraph();
        }

        private void TMHMMJobSelectionComboBoxSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            
            ComboBox selectedItem = (ComboBox)sender;
            if (selectedItem.SelectedValue != null)
            {
                TMHMMcurrentJobIndex = selectedItem.SelectedIndex;
                string name = selectedItem.SelectedValue.ToString();
                Polyline line;
                if (setupData.Sequence != "")
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.GraphGrid); ++i)
                    {
                        var child = VisualTreeHelper.GetChild(this.GraphGrid, i) as FrameworkElement;

                        if (child != null && child.Name.Contains(name))
                        {
                            line = (Polyline)child;
                            line.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (child != null && child.Name.Contains("_InsideData"))
                        {
                            line = (Polyline)child;
                            line.Visibility = System.Windows.Visibility.Hidden;
                        }
                        else if (child != null && child.Name.Contains("_MembrData"))
                        {
                            line = (Polyline)child;
                            line.Visibility = System.Windows.Visibility.Hidden;
                        }
                        else if (child != null && child.Name.Contains("_OutsideData"))
                        {
                            line = (Polyline)child;
                            line.Visibility = System.Windows.Visibility.Hidden;
                        }

                    }
                }
            }
        } 
        #endregion

    }
}
