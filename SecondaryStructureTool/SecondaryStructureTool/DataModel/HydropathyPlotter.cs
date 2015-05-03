using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecondaryStructureTool.DataModel;
using SecondaryStructureTool;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

/*
 References:
 http://gcat.davidson.edu/DGPB/kd/kyte-doolittle-background.htm#window
 */


namespace SecondaryStructureTool.DataModel
{
    class HydropathyPlotter : INotifyPropertyChanged
    {

        #region Private Variables
         
        private string sequence;
        private char[] AA = { 'I', 'V', 'L', 'F', 'C', 'M', 'A', 'G', 'T', 'W', 'S', 'Y', 'P', 'H', 'E', 'Q', 'D', 'N', 'K', 'R' };
        private float[] kyteDoolittleValues = { 4.5f, 4.2f, 3.8f, 2.8f, 2.5f, 1.9f, 1.8f, -0.4f, -0.7f, -0.9f, -0.8f, -1.3f, -1.6f, -3.2f, -3.5f, -3.5f, -3.5f, -3.5f, -3.9f, -4.5f };
        private float[] hoppWoodsValues = { -1.8f, -1.5f, -1.8f, -2.5f, -1.0f, -1.3f, -0.5f, 0.0f, -0.4f, -3.4f, 0.3f, -2.3f, 0.0f, -0.5f, 3.0f, 0.2f, 3.0f, 0.2f, 3.0f, 3.0f };
        private Dictionary<char, float> kyteDoolittleScale;
        private Dictionary<char, float> hoppWoodsScale;
        private float[] sequenceKyteDoolittleScores;
        private float[] sequenceHoppWoodsScores;
        private float[] sequenceKDTransMembrane;  //window size 19
        private float[] sequenceKDSurfaceRegions; //window size 9
        private float[] sequenceHWTransMembrane;  //window size 19
        private float[] sequenceHWSurfaceRegions; //window size 9
        private float[] sequenceCustomWindowKD;  //Custom window 
        private float[] sequenceCustomWindowHW;  //Custom window 
        #endregion

        #region Public Properties
        public string Sequence
        {
            get { return sequence; }
            set
            {
                sequence = value;
                NotifyPropertyChanged("Sequence");
            }
        }
        public float[] SequenceCustomWindowKD
        {
            get { return sequenceCustomWindowKD; }
            set
            {
                sequenceCustomWindowKD = value;
                NotifyPropertyChanged("SequenceCustomWindowKD");
            }
        }

        public float[] SequenceCustomWindowHW
        {
            get { return sequenceCustomWindowHW; }
            set
            {
                sequenceCustomWindowHW = value;
                NotifyPropertyChanged("SequenceCustomWindowHW");
            }
        }

        public float[] SequenceKDTransMembrane
        {
            get { return sequenceKDTransMembrane; }
            set
            {
                sequenceKDTransMembrane = value;
                NotifyPropertyChanged("SequenceKDTransMembrane");
            }
        }

        public float[] SequenceKDSurfaceRegions
        {
            get { return sequenceKDSurfaceRegions; }
            set
            {
                sequenceKDSurfaceRegions = value;
                NotifyPropertyChanged("SequenceKDSurfaceRegions");
            }
        }

        public float[] SequenceHWTransMembrane
        {
            get { return sequenceHWTransMembrane; }
            set
            {
                sequenceHWTransMembrane = value;
                NotifyPropertyChanged("SequenceHWTransMembrane");
            }
        }

        public float[] SequenceHWSurfaceRegions
        {
            get { return sequenceHWSurfaceRegions; }
            set
            {
                sequenceHWSurfaceRegions = value;
                NotifyPropertyChanged("SequenceHWSurfaceRegions");
            }
        }
        public float[] SequenceKyteDoolittleScores
        {
            get { return sequenceKyteDoolittleScores; }
            set
            {
                sequenceKyteDoolittleScores = value;
                NotifyPropertyChanged("SequenceKyteDoolittleScores");
            }
        }

        public float[] SequenceHoppWoodsScores
        {
            get { return sequenceHoppWoodsScores; }
            set
            {
                sequenceHoppWoodsScores = value;
                NotifyPropertyChanged("SequenceHoppWoodsScores");
            }
        }
        #endregion

        public HydropathyPlotter()
        {

        }

        #region Public Methods

        public void InitHydroPlotter(string _sequence)
        {
            Sequence = _sequence;
            SequenceKyteDoolittleScores = new float[sequence.Length];
            SequenceHoppWoodsScores = new float[sequence.Length];
            SequenceKDTransMembrane = new float[sequence.Length];
            SequenceKDSurfaceRegions = new float[sequence.Length];
            SequenceHWSurfaceRegions = new float[sequence.Length];
            SequenceHWTransMembrane = new float[sequence.Length];
            SequenceCustomWindowKD = new float[sequence.Length];
            SequenceCustomWindowHW = new float[sequence.Length];
            kyteDoolittleScale = new Dictionary<char, float>();
            hoppWoodsScale = new Dictionary<char, float>();
            PopulateScales();
            SetValues();
            RunTransMembraneAndSurface();
            
        }

        /// <summary>
        /// Runs standard window size calculation for Kyte Doolittle and Hopp Woods scales
        /// Standard window sizes are 19 (Trans Membrane) and 9 (Surface Region)
        /// </summary>
        public void RunTransMembraneAndSurface()
        {
            int transMem = 19; //Best size for transmembrane detection
            int surf = 9; //Best size for surface region detection

            RunCustomWindow(surf, SequenceKDSurfaceRegions, "KD");
            RunCustomWindow(transMem, SequenceKDTransMembrane, "KD");
            RunCustomWindow(surf, SequenceHWSurfaceRegions, "HW");
            RunCustomWindow(transMem, SequenceHWTransMembrane, "HW");
        }

        public void RunCustomWindow(int windowSize, float[] WindowData, string scale)
        {
            
            int halfWindow = 0, start = 0, stop = 0, iStop = 0;
            if (windowSize >= 5 && windowSize <= 40)
            {
                if (windowSize % 2 == 0)
                {
                    start = windowSize / 2;
                    halfWindow = windowSize / 2;
                    stop = halfWindow + 1;
                    iStop = sequence.Length - stop;
                }
                else
                {
                    start = windowSize / 2;
                    halfWindow = windowSize / 2 + 1;
                    stop = halfWindow;
                    iStop = sequence.Length - stop;
                }

                //sets start and stop points for graph. used to draw graph in MainWindow [0] us start [last entry] is stop.
                WindowData[0] = start;
                WindowData[WindowData.Length - 1] = iStop;

                for (var i = start; i < iStop; i++)
                {
                    //check to see if 19 AA are left in sequence from i
                  
                    int jStart = i - start, jStop = i + stop;
                    int testWidth = jStop - jStart;
                    if (jStop < iStop )
                    {
                        
                        for (int j = jStart; j <= jStop; ++j)
                        {
                            //Kyte Doolittle Scale
                            if (scale == "KD")
                            {
                                WindowData[i] += SequenceKyteDoolittleScores[j];
                            }
                            //Hopp Wood Scale
                            else if (scale == "HW")
                            {
                                WindowData[i] += SequenceHoppWoodsScores[j];
                            }
                            else
                            { 
                                //TODO -- error
                            }
                        }
                    }
                }
            }
            else
            {
                //CREATE ERROR
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates lookup tables for Kyte Doolittle and Hopp Woods Hydropathy plot scales for each amino acid
        /// </summary>
        private void PopulateScales()
        {
            for (int i = 0; i < 20; ++i)
            {
                kyteDoolittleScale.Add(AA[i], kyteDoolittleValues[i]);
                hoppWoodsScale.Add(AA[i], hoppWoodsValues[i]);
            }
        }

        /// <summary>
        /// Sets the values for each amino acid for both Kyte Doolitte and Hopp Woods scale values.
        /// </summary>
        private void SetValues()
        {
            for (var i = 0; i < sequence.Length; ++i)
            {
                sequenceKyteDoolittleScores[i] = kyteDoolittleScale[sequence[i]];
                sequenceHoppWoodsScores[i] = hoppWoodsScale[sequence[i]];
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// To use INotify Property changed for databinding to the GUI
        /// reference: using System.ComponentModel; and using System.Runtime.CompilerServices;
        /// add attribute ClassName : INotifyPropertyChanged
        /// copy Events and Event Method Regions into your class
        /// set up all private variables to be bound to GUI with a property:
        ///     public string VariablePropertyName
        ///     {
        ///         get { return variableFieldName; }
        ///         set { 
        ///             variableFieldName = value;
        ///             NotifyPropertyChanged("VariablePropertyName");
        ///         }
        ///     }
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        #region Event Methods

        /// <summary>
        /// Handles events in WPF Projects
        /// </summary>
        /// <param name="propertyName">the name of the property that's calling this event</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                //Console.WriteLine("IMMA FIRING MY REACTORVESSEL EVENT!!!");
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            /*
             * For more information, chenck this website:
             * http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged%28v=vs.110%29.aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2
             */
        }
        #endregion
    }
}
