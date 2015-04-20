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
        #endregion

        #region Public Properties

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
            kyteDoolittleScale = new Dictionary<char, float>();
            PopulateScales();
            SetValues();
            RunKyteDoolittle();
        }

        public void RunKyteDoolittle()
        {
            int transMem = 19;
            int surf = 9;

            for (var i = 0; i < sequence.Length; i++)
            {
                //check to see if 19 AA are left in sequence from i
                int index = i, index2 = i;
                if (i+19 < sequence.Length){
                    sequenceKDTransMembrane[i] = sequenceKyteDoolittleScores[index] + sequenceKyteDoolittleScores[++index]
                        + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index]
                        + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index]
                        + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index]
                        + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index]
                        + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index]
                        + sequenceKyteDoolittleScores[++index] + sequenceKyteDoolittleScores[++index];
                    sequenceKDSurfaceRegions[i] = sequenceKyteDoolittleScores[index2] + sequenceKyteDoolittleScores[++index2]
                        + sequenceKyteDoolittleScores[++index2] + sequenceKyteDoolittleScores[++index2] + sequenceKyteDoolittleScores[++index2]
                        + sequenceKyteDoolittleScores[++index2] + sequenceKyteDoolittleScores[++index2] + sequenceKyteDoolittleScores[++index2]
                        + sequenceKyteDoolittleScores[++index2];
                }
                else if (i+9 < sequence.Length)
                {
                    sequenceKDSurfaceRegions[i] = sequenceKyteDoolittleScores[index2] + sequenceKyteDoolittleScores[++index2]
                         + sequenceKyteDoolittleScores[++index2] + sequenceKyteDoolittleScores[++index2] + sequenceKyteDoolittleScores[++index2]
                         + sequenceKyteDoolittleScores[++index2] + sequenceKyteDoolittleScores[++index2] + sequenceKyteDoolittleScores[++index2]
                         + sequenceKyteDoolittleScores[++index2];
                }
            }
            

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
