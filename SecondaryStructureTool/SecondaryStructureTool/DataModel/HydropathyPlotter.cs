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
        private float[] hoppWoodsValues;
        private Dictionary<char, float> kyteDoolittleScale;
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
            sequence = _sequence;
            sequenceKyteDoolittleScores = new float[sequence.Length];
            //sequenceHoppWoodsScores = new float[sequence.Length];
            sequenceKDTransMembrane = new float[sequence.Length];
            sequenceKDSurfaceRegions = new float[sequence.Length];
            SequenceHWSurfaceRegions = new float[sequence.Length];
            SequenceHWTransMembrane = new float[sequence.Length];
            kyteDoolittleScale = new Dictionary<char, float>();
            PopulateScales();
            SetValues();
            RunTransMembraneAndSurface();
            
        }

        public void RunTransMembraneAndSurface()
        {
            int transMem = 19;
            int surf = 9;
            
            SequenceHWSurfaceRegions[0] = 5;
            SequenceHWTransMembrane[0] = 10;
            SequenceKDSurfaceRegions[0] = 5;
            SequenceKDTransMembrane[0] = 10;
            SequenceHWSurfaceRegions[sequence.Length-1] = sequence.Length - 5;
            SequenceHWTransMembrane[sequence.Length-1] = sequence.Length - 9;
            SequenceKDSurfaceRegions[sequence.Length-1] = sequence.Length - 5;
            SequenceKDTransMembrane[sequence.Length-1] = sequence.Length - 9;
          


            for (var i = 5; i < sequence.Length - 5; i++)
            {
                //check to see if 19 AA are left in sequence from i

                int index = i, index2 = i, transStop = sequence.Length -9, surfStop = sequence.Length -5;
                if (i < transStop && i > 9){
                    sequenceKDTransMembrane[i] = sequenceKyteDoolittleScores[index - 9] + sequenceKyteDoolittleScores[index - 8]
                        + sequenceKyteDoolittleScores[index - 7] + sequenceKyteDoolittleScores[index - 6] + sequenceKyteDoolittleScores[index - 5]
                        + sequenceKyteDoolittleScores[index - 4] + sequenceKyteDoolittleScores[index - 3] + sequenceKyteDoolittleScores[index - 2]
                        + sequenceKyteDoolittleScores[index - 1] + sequenceKyteDoolittleScores[index] + sequenceKyteDoolittleScores[index + 1]
                        + sequenceKyteDoolittleScores[index + 2] + sequenceKyteDoolittleScores[index + 3] + sequenceKyteDoolittleScores[index + 4]
                        + sequenceKyteDoolittleScores[index + 5] + sequenceKyteDoolittleScores[index + 6] + sequenceKyteDoolittleScores[index + 7]
                        + sequenceKyteDoolittleScores[index + 8] + sequenceKyteDoolittleScores[index + 9];
                    sequenceKDSurfaceRegions[i] = sequenceKyteDoolittleScores[index2 - 4] + sequenceKyteDoolittleScores[index2 - 3]
                        + sequenceKyteDoolittleScores[index2 - 2] + sequenceKyteDoolittleScores[index2 - 1] + sequenceKyteDoolittleScores[index2]
                        + sequenceKyteDoolittleScores[index2 + 1] + sequenceKyteDoolittleScores[index2 + 2] + sequenceKyteDoolittleScores[index2 + 3]
                        + sequenceKyteDoolittleScores[index2 + 4];
                }
                else if (i < surfStop)
                {
                    sequenceKDSurfaceRegions[i] = sequenceKyteDoolittleScores[index2 - 4] + sequenceKyteDoolittleScores[index2 - 3]
                        + sequenceKyteDoolittleScores[index2 - 2] + sequenceKyteDoolittleScores[index2 - 1] + sequenceKyteDoolittleScores[index2]
                        + sequenceKyteDoolittleScores[index2 + 1] + sequenceKyteDoolittleScores[index2 + 2] + sequenceKyteDoolittleScores[index2 + 3]
                        + sequenceKyteDoolittleScores[index2 + 4];
                }
                else
                {
                    break;
                }
            }
            

        }

        public void RunCustomWindow(int windowSize)
        {

        }
        #endregion

        #region Private Methods
        private void PopulateScales()
        {
            for (int i = 0; i < 20; ++i)
            {
               kyteDoolittleScale.Add(AA[i], kyteDoolittleValues[i]);
            }
        }

        private void SetValues()
        {
            for (var i = 0; i < sequence.Length; ++i)
            {
                sequenceKyteDoolittleScores[i] = kyteDoolittleScale[sequence[i]];
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// To use INotify Property changed for databinding to the GUI
        /// reference: using System.ComponentModel;
        /// add attribute ClassName : INotifyPropertyChanged
        /// copy Events and Event Method Regions into your class
        /// set up all private variables to be bound to GUI with a property:
        ///     public string VariablePropertyName
        ///     {
        ///         get { return variableFieldName; }
        ///         set { 
        ///             variableFieldName = value.ToUpper();
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
