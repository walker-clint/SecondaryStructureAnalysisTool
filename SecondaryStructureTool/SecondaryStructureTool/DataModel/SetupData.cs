using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.IO;
using SecondaryStructureTool.DataModel;

namespace SecondaryStructureTool.DataModel
{
    class SetupData : INotifyPropertyChanged
    {

        #region Private Variables
        private bool setupFromAccession = false;
        private bool setupFromFASTAorSeq = false;
        private string sequence;
        private string sequenceOrFASTAInput;
        private string accessionNumber;
        private string jobID;
        private string description;
        private string name;
        private char[] AA = { 'I', 'V', 'L', 'F', 'C', 'M', 'A', 'G', 'T', 'W', 'S', 'Y', 'P', 'H', 'E', 'Q', 'D', 'N', 'K', 'R' };
        private bool badSequence;
        private List<int> badSequenceLocations = new List<int>();

        enum FASTASeqID
        {
            gi, emb, djb, pir, prf, sp, pdb, pat, bbs, gnl, Ref, lcl 
        }
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
                if (setupFromAccession)
                {
                    sequenceOrFASTAInput = value;
                }
                else
                {
                    setupFromFASTAorSeq = true;
                    setupFromAccession = false;
                    sequenceOrFASTAInput = value;
                    DecomposeFASTA(value);
                    setupFromFASTAorSeq = false;
                }
                
                NotifyPropertyChanged("SequenceOrFASTAInput");
            }
        }
       
        public string AccessionNumber
        {
            get { return accessionNumber; }
            set
            {
                if (setupFromFASTAorSeq)
                {
                    //accession number being set from input FASTA
                    accessionNumber = value;
                }
                else
                {
                    setupFromAccession = true;
                    setupFromFASTAorSeq = false;
                    accessionNumber = value;
                    QueryNCBIDatabase Query = new QueryNCBIDatabase();
                    SequenceOrFASTAInput = Query.GetFASTA(accessionNumber);
                    DecomposeFASTA(SequenceOrFASTAInput);
                    setupFromAccession = false;
                }   
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
            int sequenceStart = 1;
            //Rip the name, description and sequence from FASTA entry
            if (FASTA[0] == '>')
            {
                int holder = 1, pipes = 0;
                foreach (char c in FASTA)
                {
                    if (c == '|')
                    {
                        ++pipes;
                    }

                }
                
                //'|',
                char[] delimiterChars = { '\n', '\r' };
                string[] tokens = FASTA.Split(delimiterChars);
                bool decoding = true;
                foreach (string s in tokens){
                    if (!decoding)
                    {
                        break;
                    } 
                    System.Console.WriteLine(s);
                    int j = 0;
                    string[] databases = { "gi", "emb", "djb", "pir", "prf", "sp", "pdb", "pat", "bbs", "gnl", "ref", "lcl" };
                    
                    foreach (string d in databases)
                    {
                        if (!decoding)
                        {
                            break;
                        } 
                        else if (tokens[0].Contains(d))
                        {
                            string db = d;
                            int index = Array.IndexOf(databases, d);
                            
                            while (decoding) {
                                switch (index)
                                {
                                    case 0:
                                        //GenBank                           gb|accession|locus
                                        if (tokens[2] == "sp")
                                        {
                                            AccessionNumber = tokens[1];
                                            index = Array.IndexOf(databases, "sp");
                                            break;
                                        }
                                        decoding = false;
                                        break;

                                    case 1:
                                        //EMBL Data Library                 emb|accession|locus
                                        break;
                                    case 2:
                                        //DDBJ, DNA Database of Japan       dbj|accession|locus
                                        //not in amino acid format?
                                        break;
                                    case 3:
                                        //NBRF PIR                          pir||entry
                                        break;
                                    case 4:
                                        //Protein Research Foundation       prf||name
                                        break;
                                    case 5:
                                        //SWISS-PROT                        sp|accession|entry name
                                        if (pipes == 2)
                                        {
                                            string[] tokens2 = tokens[0].Split('|');
                                            if (tokens2.Length >= 3)
                                            {
                                                if (setupFromFASTAorSeq)
                                                {
                                                    AccessionNumber = tokens2[1];
                                                }
                                                Name = tokens2[2];
                                                decoding = false;
                                                break;
                                            }
                                            else if (tokens2.Length == 2)
                                            {
                                                AccessionNumber = tokens[1];
                                                decoding = false;
                                                break;
                                            }
                                            else
                                            {
                                                // no accession number or name
                                                decoding = false;
                                                break;
                                            }
                                        }
                                        break;
                                    case 6:
                                        //Brookhaven Protein Data Bank      pdb|entry|chain
                                        break;
                                    case 7:
                                        //Patents                           pat|country|number
                                        break;
                                    case 8:
                                        //GenInfo Backbone Id               bbs|number
                                        break;
                                    case 9:
                                        //GenInfo Backbone Id               bbs|number
                                        break;
                                    case 10:
                                        //General database identifier       gnl|database|identifier
                                        break;
                                    case 11:
                                        //NCBI Reference Sequence           ref|accession|locus
                                        break;
                                    case 12:
                                        //Local Sequence identifier         lcl|identifier
                                        break;
                                    default:
                                        break;

                                }
                            }
                        }
                    }


                }




                BadSequence = false;
                string seqString = "";
                for (var i = sequenceStart; i < tokens.Length; ++i)
                {
                    foreach (char c in tokens[i])
                    {
                        if (!AA.Contains(c))
                        {
                            badSequenceLocations.Add(sequence.IndexOf(c));
                            BadSequence = true;
                        }
                    }
                    if (!BadSequence)
                    {
                        if (tokens[i] == "")
                        {

                        }
                        else
                        {
                            seqString += tokens[i];
                        }
                    }


                }
                Sequence = seqString;

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
