using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecondaryStructureTool.DataModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SecondaryStructureTool.DataModel
{
    class Chou_Fasman : INotifyPropertyChanged
    {

        #region private variables
        private string sequence;
        private char[] AA =                   {   'I',   'V',   'L',   'F',   'C',   'M',   'A',   'G',   'T',   'W',   'S',   'Y',   'P',   'H',   'E',   'Q',   'D',   'N',   'K',   'R' };
        private float[] valuesAlphaHelix =    { 1.08f, 1.06f, 1.21f, 1.13f, 0.70f, 1.54f, 1.42f, 0.57f, 0.83f, 1.08f, 0.77f, 0.69f, 0.57f, 1.00f, 1.51f, 1.11f, 1.01f, 0.67f, 1.16f, 0.98f };
        private char[] charValuesAlphaHelix = {   'h',   'h',   'H',   'h',   'i',   'H',   'H',   'B',   'i',   'h',   'i',   'b',   'B',   'I',   'H',   'h',   'I',   'b',   'h',   'i' };
        private float[] valuesBetaSheet =     { 1.60f, 1.70f, 1.30f, 1.38f, 1.19f, 1.05f, 0.83f, 0.75f, 1.19f, 1.37f, 0.75f, 1.47f, 0.55f, 0.87f, 0.37f, 1.10f, 0.54f, 0.89f, 0.74f, 0.93f };
        private char[] charValuesBetaSheet =  {   'H',   'H',   'h',   'h',   'h',   'h',   'i',   'b',   'h',   'h',   'b',   'H',   'B',   'i',   'B',   'h',   'B',   'i',   'b',   'i' };
        private float[] valuesCoilLoop =      { 0.47f, 0.50f, 0.59f, 0.60f, 1.19f, 0.60f, 0.66f, 1.56f, 0.96f, 0.96f, 1.43f, 1.14f, 1.52f, 0.95f, 0.74f, 0.98f, 1.46f, 1.56f, 1.01f, 0.95f };

        private Dictionary<char, int> ResidueIndexLookup = new Dictionary<char, int>();
        //the following arrays are used to store individual residue values
        private float[] sequenceValuesHelix;
        private float[] sequenceValuesBeta;
        private char[] sequenceCharsHelix;
        private char[] sequenceCharsBeta;
        private float[] sequenceValuesCoil;
        //the following arrays are the results of the chou-fasman algorithm for each alpha helix, beta sheet, and coil/loop
        private float[] chouFasmanResultAlphaHelix;
        private float[] chouFasmanResultBetaSheet;
        private float[] chouFasmanResultCoilLoop;
        private char[] chouFasmanResultCharAlphaHelix;
        private char[] chouFasmanResultCharBetaSheet;
        #endregion
        #region public properties
        public string Sequence
        {
            get { return sequence; }
            set
            {
                sequence = value;
                NotifyPropertyChanged("Sequence");
            }
        }
        public char[] ChouFasmanResultCharAlphaHelix
        {
            get { return chouFasmanResultCharAlphaHelix; }
            set
            {
                chouFasmanResultCharAlphaHelix = value;
                NotifyPropertyChanged("ChouFasmanResultCharAlphaHelix");
            }
        }
        public char[] ChouFasmanResultCharBetaSheet
        {
            get { return chouFasmanResultCharBetaSheet; }
            set
            {
                chouFasmanResultCharBetaSheet = value;
                NotifyPropertyChanged("ChouFasmanResultCharBetaSheet");
            }
        }
        public float[] ChouFasmanResultAlphaHelix
        {
            get { return chouFasmanResultAlphaHelix; }
            set
            {
                chouFasmanResultAlphaHelix = value;
                NotifyPropertyChanged("ChouFasmanResultAlphaHelix");
            }
        }
        public float[] ChouFasmanResultBetaSheet
        {
            get { return chouFasmanResultBetaSheet; }
            set
            {
                chouFasmanResultBetaSheet = value;
                NotifyPropertyChanged("ChouFasmanResultBetaSheet");
            }
        }
        public float[] ChouFasmanResultCoilLoop
        {
            get { return chouFasmanResultCoilLoop; }
            set
            {
                chouFasmanResultCoilLoop = value;
                NotifyPropertyChanged("ChouFasmanResultCoilLoop");
            }
        }
        public float[] ValuesAlphaHelix
        {
            get { return valuesAlphaHelix; }
            set
            {
                valuesAlphaHelix = value;
                NotifyPropertyChanged("ValuesAlphaHelix");
            }
        }
        public char[] CharValuesAlphaHelix
        {
            get { return charValuesAlphaHelix; }
            set
            {
                charValuesAlphaHelix = value;
                NotifyPropertyChanged("CharValuesAlphaHelix");
            }
        }
        public float[] ValuesBetaSheet
        {
            get { return valuesBetaSheet; }
            set
            {
                valuesBetaSheet = value;
                NotifyPropertyChanged("ValuesBetaSheet");
            }
        }
        public char[] CharValuesBetaSheet
        {
            get { return charValuesBetaSheet; }
            set
            {
                charValuesBetaSheet = value;
                NotifyPropertyChanged("CharValuesBetaSheet");
            }
        }
        public float[] ValuesCoilLoop
        {
            get { return valuesCoilLoop; }
            set
            {
                valuesCoilLoop = value;
                NotifyPropertyChanged("ValuesCoilLoop");
            }
        }
        public float[] SequenceValuesHelix
        {
            get { return sequenceValuesHelix; }
            set
            {
                sequenceValuesHelix = value;
                NotifyPropertyChanged("SequenceValuesHelix");
            }
        }
        public float[] SequenceValuesBeta
        {
            get { return sequenceValuesBeta; }
            set
            {
                sequenceValuesBeta = value;
                NotifyPropertyChanged("SequenceValuesBeta");
            }
        }
        public char[] SequenceCharsHelix
        {
            get { return sequenceCharsHelix; }
            set
            {
                sequenceCharsHelix = value;
                NotifyPropertyChanged("SequenceCharsHelix");
            }
        }
        public char[] SequenceCharsBeta
        {
            get { return sequenceCharsBeta; }
            set
            {
                sequenceCharsBeta = value;
                NotifyPropertyChanged("SequenceCharsBeta");
            }
        }
        public float[] SequenceValuesCoil
        {
            get { return sequenceValuesCoil; }
            set
            {
                sequenceValuesCoil = value;
                NotifyPropertyChanged("SequenceValuesCoil");
            }
        }

        #endregion

        public Chou_Fasman(){
            for (int i = 0; i < 20; ++i)
            {
                ResidueIndexLookup.Add(AA[i], i);
            }
        }

        #region Public Methods
        public void RunChouFasman(string _sequence) {

            Sequence = _sequence;
            int length = sequence.Length;

            #region Setup chars and values for calculations
            SequenceCharsBeta = new char[length];
            SequenceCharsHelix = new char[length];
            SetResidueChars(SequenceCharsBeta, CharValuesBetaSheet);
            SetResidueChars(SequenceCharsHelix, CharValuesAlphaHelix);
            
                

            SequenceValuesBeta = new float[length];
            SequenceValuesCoil = new float[length];
            SequenceValuesHelix = new float[length];
            SetResidueValues(SequenceValuesBeta, ValuesBetaSheet);
            SetResidueValues(SequenceValuesCoil, ValuesCoilLoop);
            SetResidueValues(SequenceValuesHelix, ValuesAlphaHelix);

            ChouFasmanResultAlphaHelix = new float[length];
            ChouFasmanResultBetaSheet = new float[length];
            ChouFasmanResultCoilLoop = new float[length];
            ChouFasmanResultCharAlphaHelix = new char[length];
            ChouFasmanResultCharBetaSheet = new char[length];
            for (int i = 0; i < Sequence.Length; ++i)
            {
                ChouFasmanResultCharAlphaHelix[i] = '\r';
                ChouFasmanResultCharBetaSheet[i] = '\r';
            }
            #endregion

            CalculateHelixData();
            CalculateBetaSheet();
        }
        #endregion

        #region Private Methods
        private void SetResidueValues(float[] result, float[] values)
        {
            
            for (int i = 0; i < Sequence.Length; ++i )
            {
                result[i] = values[ResidueIndexLookup[sequence[i]]];
            }
        }

        private void SetResidueChars(char[] result, char[] chars)
        {
            for (int i = 0; i < Sequence.Length; ++i)
            {
                result[i] = chars[ResidueIndexLookup[Sequence[i]]];
            }
        }

        private void CalculateHelixData()
        {
            //calculate 6 size window values for sequence
            for (int i = 0; i < Sequence.Length - 6; ++i)
            {
                ChouFasmanResultAlphaHelix[i] = (SequenceValuesHelix[i] + SequenceValuesHelix[i + 1] + SequenceValuesHelix[i + 2] + SequenceValuesHelix[i + 3] + SequenceValuesHelix[i + 4] + SequenceValuesHelix[i + 5]) / 6;
                if (ChouFasmanResultAlphaHelix[i] >= 1.0f)
                {
                    ChouFasmanResultCharAlphaHelix[i] = '=';
                }
            }
            int residueCount = 0;
            for (int i = 0; i < Sequence.Length; ++i)
            {
                if (residueCount == 0 && ChouFasmanResultCharAlphaHelix[i] == '=')
                {
                    ChouFasmanResultCharAlphaHelix[i] = '<';
                    residueCount++;
                }
                else if (ChouFasmanResultCharAlphaHelix[i] == '=')
                {
                    residueCount++;
                }                    
                else if (ChouFasmanResultCharAlphaHelix[i] == '\r')
                {
                    //if there are less than 6 residues considered to be a helix then they are not a helix remove helix symbols
                    if (residueCount < 6)
                    {
                        for (int j = 0; j < residueCount; ++j)
                        {
                            int testint = i - j - 1;
                            ChouFasmanResultCharAlphaHelix[i-j-1] = '\r';
                        }
                    }
                    else
                    {
                        ChouFasmanResultCharAlphaHelix[i - 1] = '>';
                    }

                    
                    residueCount = 0;
                }
            }
            for (int i = 0; i < Sequence.Length; ++i)
            {
                if (i > 0 && i < Sequence.Length - 1)
                {
                    if (ChouFasmanResultCharAlphaHelix[i - 1] == '>' && ChouFasmanResultCharAlphaHelix[i + 1] == '<')
                    {
                        ChouFasmanResultCharAlphaHelix[i] = '-';
                    }
                }
            }
            for (int i = 0; i < Sequence.Length; ++i)
            {
                if (i > 0 && i < Sequence.Length - 1)
                {
                    if (ChouFasmanResultCharAlphaHelix[i] == '<' && ChouFasmanResultCharAlphaHelix[i - 1] == '\r' )
                    {
                        if (CheckSequenceBackward(i - 1, 4, ChouFasmanResultAlphaHelix, ChouFasmanResultCharAlphaHelix, SequenceValuesHelix, SequenceCharsHelix))
                        {
                            ChouFasmanResultCharAlphaHelix[i] = '=';
                        }
                    }
                    if (ChouFasmanResultCharAlphaHelix[i] == '>' && ChouFasmanResultCharAlphaHelix[i + 1] == '\r')
                    {
                        if (CheckSequenceForward(i + 1, 4, ChouFasmanResultAlphaHelix, ChouFasmanResultCharAlphaHelix, SequenceValuesHelix, SequenceCharsHelix))
                        {
                            ChouFasmanResultCharAlphaHelix[i] = '=';
                        }
                    }
                }
            }
           
        }

        private void CalculateBetaSheet()
        {
            //calculate 5 size window values for sequence
            for (int i = 0; i < Sequence.Length - 5; ++i)
            {
                ChouFasmanResultBetaSheet[i] = (SequenceValuesBeta[i] + SequenceValuesBeta[i + 1] + SequenceValuesBeta[i + 2] + SequenceValuesBeta[i + 3] + SequenceValuesBeta[i + 4]) / 5;
                if (ChouFasmanResultBetaSheet[i] >= 1.0f)
                {
                    ChouFasmanResultCharBetaSheet[i] = '=';
                }
            }
            int residueCount = 0;
            for (int i = 0; i < Sequence.Length; ++i)
            {
                if (residueCount == 0 && ChouFasmanResultCharBetaSheet[i] == '=')
                {
                    ChouFasmanResultCharBetaSheet[i] = '<';
                    residueCount++;
                }
                else if (ChouFasmanResultCharBetaSheet[i] == '=')
                {
                    residueCount++;
                }
                else if (ChouFasmanResultCharBetaSheet[i] == '\r')
                {
                    //if there are less than 6 residues considered to be a helix then they are not a helix remove helix symbols
                    if (residueCount < 6)
                    {
                        for (int j = 0; j < residueCount; ++j)
                        {
                            int testint = i - j - 1;
                            ChouFasmanResultCharBetaSheet[i - j - 1] = '\r';
                        }
                    }
                    else
                    {
                        ChouFasmanResultCharBetaSheet[i - 1] = '>';
                    }


                    residueCount = 0;
                }
            }
            for (int i = 0; i < Sequence.Length; ++i)
            {
                if (i > 0 && i < Sequence.Length - 1)
                {
                    if (ChouFasmanResultCharBetaSheet[i - 1] == '>' && ChouFasmanResultCharBetaSheet[i + 1] == '<')
                    {
                        ChouFasmanResultCharBetaSheet[i] = '-';
                    }
                }
            }
            for (int i = 0; i < Sequence.Length; ++i)
            {
                if (i > 0 && i < Sequence.Length - 1)
                {
                    if (ChouFasmanResultCharBetaSheet[i] == '<' && ChouFasmanResultCharBetaSheet[i - 1] == '\r')
                    {
                        if (CheckSequenceBackward(i - 1, 4, ChouFasmanResultBetaSheet, ChouFasmanResultCharBetaSheet, SequenceValuesBeta, SequenceCharsBeta))
                        {
                            ChouFasmanResultCharBetaSheet[i] = '=';
                        }
                    }
                    if (ChouFasmanResultCharBetaSheet[i] == '>' && ChouFasmanResultCharBetaSheet[i + 1] == '\r')
                    {
                        if (CheckSequenceForward(i + 1, 4, ChouFasmanResultBetaSheet, ChouFasmanResultCharBetaSheet, SequenceValuesBeta, SequenceCharsBeta))
                        {
                            ChouFasmanResultCharBetaSheet[i] = '=';
                        }
                    }
                }
            }
        }

        private int FindHighestValue(int start, int stop, float[] values)
        {
            float highest = 0;
            int index = 0;
            for (int i = start; i < stop; ++i)
            {
                if (highest < values[i])
                {
                    index = i;
                    highest = values[i];
                }
            }
            return index;
        }

        private bool CheckSequenceBackward(int windowBegin, int windowSize, float[] result, char[] charsResult, float[] values, char[] charValues)
        {
            float average = 0;
            //check window sizes to the beginning of the sequence
            int testTopStop = 0;
            bool extended = false;
            for (int i = windowBegin; i > windowSize; --i)
            {
                testTopStop = i - windowSize;
                average = 0;
                for (int j = 0; j < windowSize; ++j)
                {
                    average += values[i - j];
                }
                average = average / windowSize;
                if (average < 1.0f)
                {
                    //break no longer part of an Alpha Helix
                    
                    charsResult[i + 1] = '<';
                    return extended;
                }
                else
                {
                    //assign i as helix
                    charsResult[i] = '-';
                    extended = true;
                }
            }
            return extended;
        }

        private bool CheckSequenceForward(int windowEnd, int windowSize, float[] result, char[] charsResult, float[] values, char[] charValues)
        {
            //check window sizes to the end of the sequence
            int testStop = Sequence.Length - windowSize;
            float average = 0;
            bool extended = false;
            for (int i = windowEnd; i < Sequence.Length - windowSize; ++i)
            {
                average = 0;
                for (int j = 0; j < windowSize; ++j)
                {
                    average += values[i + j];
                }
                average = average / windowSize;
                if (average < 1.0f)
                {
                    //break no longer part of an Alpha Helix
                    
                    charsResult[i - 1] = '>';
                    return extended;
                }
                else
                {
                    //assign i as helix
                    charsResult[i] = '-';
                    extended = true;
                }
            }
            return extended;
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
