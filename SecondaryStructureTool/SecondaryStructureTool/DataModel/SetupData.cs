using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.IO;

namespace SecondaryStructureTool.DataModel
{
    class SetupData : INotifyPropertyChanged
    {

        #region Private Variables
        private string sequence;
        private string sequenceOrFASTAInput;
        private string accessionNumber;
        private string jobID;
        private string description;
        private string name;
        private char[] AA = { 'I', 'V', 'L', 'F', 'C', 'M', 'A', 'G', 'T', 'W', 'S', 'Y', 'P', 'H', 'E', 'Q', 'D', 'N', 'K', 'R' };
        private bool badSequence;
        private List<int> badSequenceLocations = new List<int>();
        #endregion

        #region Public Variables
        public string Sequence
        {
            get { return sequence; }
            set { 
                sequence = value.ToUpper();
                BadSequence = false;
                foreach (char c in sequence)
                {
                    
                    if (!AA.Contains(c))
                    {
                        badSequenceLocations.Add(sequence.IndexOf(c));
                        BadSequence = true;
                    }
                }
                NotifyPropertyChanged("Sequence");
            }
        }
        public bool BadSequence
        {
            get { return badSequence; }
            set
            {
                badSequence = value;
                NotifyPropertyChanged("BadSequence");
            }
        }

        public string SequenceOrFASTAInput
        {
            get { return sequenceOrFASTAInput; }
            set
            {
                sequenceOrFASTAInput = value;
                DecomposeFASTA(value);
                NotifyPropertyChanged("SequenceOrFASTAInput");
            }
        }
       
        public string AccessionNumber
        {
            get { return accessionNumber; }
            set
            {
                accessionNumber = value;
                NotifyPropertyChanged("AccessionNumber");
            }
        }
        public string JobID
        {
            get { return jobID; }
            set
            {
                jobID = value;
                NotifyPropertyChanged("JobID");
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyPropertyChanged("Description");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        #endregion

        public SetupData()
        {
            sequence = "";
            sequenceOrFASTAInput = "";
            accessionNumber = "";
            jobID = "";
        }

        #region Public Methods
        

        #endregion

        #region Private Methods

        private void GetAccessionNumberData()
        {
            //query NCBI or other source for FASTA data on Accession Number
        }

        private void DecomposeFASTA(string FASTA)
        {
            bool seqStart = false, nameStart = false;
            //Rip the name, description and sequence from FASTA entry
            if (FASTA[0] == '>')
            {
                int holder = 1;
                char[] delimiterChars = {'|', '\n', '\r' };
                string[] tokens = FASTA.Split(delimiterChars);
                foreach (string s in tokens){
                    System.Console.WriteLine(s);
                    AccessionNumber = tokens[3];
                    Name = tokens[4];
                    for (var i = 5; i < tokens.Length; ++i)
                    {
                        if (tokens[i] == "")
                        {
                            
                        }
                        else
                        {
                            Sequence += tokens[i];
                        }
                       
                    }
                }

            } else {
                Sequence = FASTA;

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
