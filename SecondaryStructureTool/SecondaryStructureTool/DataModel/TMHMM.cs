using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecondaryStructureTool;
using System.ComponentModel; 
using System.Runtime.CompilerServices;
using System.Xml;
using SecondaryStructureTool.dk.dtu.cbs.www;

namespace SecondaryStructureTool.DataModel
{
    class TMHMM : INotifyPropertyChanged
    {
        #region Private Variables
        private string seq;
        private string jobID;
        private WSTMHMM_2_0_ws1 wSTMHMM;
        private int start;
        private int stop;
        private runServiceResponseQueueentry queryResponse;
        private runServiceParameters parameterValues;
        private bool seqError = false;
        private float[] insideData;
        private float[] membrData;
        private float[] outsideData;
        #endregion

        #region Public Properties
        public string Seq
        {
            get { return seq; }
            set
            {
                if (value.Length > 3999)
                {
                    seq = "";
                    SeqError = true;
                }
                else
                {
                    seq = value;
                    SeqError = false;
                }
                NotifyPropertyChanged("Seq");
            }
        }
        public bool SeqError
        {
            get { return seqError; }
            set
            {
                seqError = value;
                NotifyPropertyChanged("SeqError");
            }
        }
        public string JobID
        {
            get { return jobID; }
            set
            {
                value = RemoveWhitespace(value);
                jobID = value;
                NotifyPropertyChanged("JobID");
            }
        }
        public int Start
        {
            get { return start; }
            set
            {
                start = value;
                NotifyPropertyChanged("Start");
            }
        }
        public int Stop
        {
            get { return stop; }
            set
            {
                stop = value;
                NotifyPropertyChanged("Stop");
            }
        }
        public WSTMHMM_2_0_ws1 WSTMHMM
        {
            get { return wSTMHMM; }
            set
            {
                wSTMHMM = value;
                NotifyPropertyChanged("WSTMHMM");
            }
        }
        public runServiceResponseQueueentry QueryResponse
        {
            get { return queryResponse; }
            set
            {
                queryResponse = value;
                NotifyPropertyChanged("QueryResponse");
            }
        }
        public runServiceParameters ParameterValues
        {
            get { return parameterValues; }
            set
            {
                parameterValues = value;
                NotifyPropertyChanged("ParameterValues");
            }
        }
        public float[] InsideData
        {
            get { return insideData; }
            set
            {
                insideData = value;
                NotifyPropertyChanged("InsideData");
            }
        }
        public float[] MembrData
        {
            get { return membrData; }
            set
            {
                membrData = value;
                NotifyPropertyChanged("MembrData");
            }
        }
        public float[] OutsideData
        {
            get { return outsideData; }
            set
            {
                outsideData = value;
                NotifyPropertyChanged("OutsideData");
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor for Webdata parser input
        /// </summary>
        public TMHMM(string _seq, string _jobID, int _start, int _stop)
        {
            Seq = _seq;
            JobID = _jobID;
            InsideData = new float[Seq.Length];
            MembrData = new float[Seq.Length];
            OutsideData = new float[Seq.Length];

        }
        /// <summary>
        /// This constructor is for the webservice that has an issue now and thus this is not currently used.
        /// </summary>
        /// <param name="_seq"></param>
        /// <param name="_jobID"></param>
        public TMHMM(string _seq, string _jobID, string run)
        {
            Seq = _seq;
            JobID = _jobID;

            //QueryResponse = new runServiceResponseQueueentry();
            //string url = "http://www.cbs.dtu.dk/services/TMHMM/";
            //string address = string.Format(
            //    "http://www.cbs.dtu.dk/services/TMHMM/"

            //    );
            WSTMHMM = new WSTMHMM_2_0_ws1();
        }
  
  
   
        #endregion

        #region Public Methods
        /// <summary>
        /// Parser to format data pasted from TMHMM website
        /// </summary>
        /// <param name="data"></param>
        public bool ParseTMHMMData(string data)
        {
            int index = 0;
            string[] LineTokens = data.Split('\n');
            foreach (string s in LineTokens)
            {
                string[] tokens = s.Split(' ');
                if (tokens[0] == "#" || tokens.Length < 5) 
                {
 
                }
                else
                {
                    InsideData[index] = Convert.ToSingle(tokens[2]);
                    MembrData[index] = Convert.ToSingle(tokens[3]);
                    OutsideData[index] = Convert.ToSingle(tokens[4]);
                    index++;
                }
            }
            if (Seq.Length < index)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region Private Methods
        private  string RemoveWhitespace(string input)
        {
            int j = 0, inputlen = input.Length;
            char[] newarr = new char[inputlen];

            for (int i = 0; i < inputlen; ++i)
            {
                char tmp = input[i];

                if (!char.IsWhiteSpace(tmp))
                {
                    newarr[j] = tmp;
                    ++j;
                }
            }

            return new String(newarr, 0, j);
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

//jobID = _jobID;
//Seq = _seq;
//if (!SeqError)
//{
//   // try
//   // {
//        WSTMHMM = new WSTMHMM_2_0_ws1();
//        QueryResponse = new runServiceResponseQueueentry();
//        parameterValues = new runServiceParameters();
//        sequence[] ss = new sequence[1];
//        sequence s = new sequence();
//        s.id = jobID;
//        s.seq = Seq;
//        ParameterValues.sequencedata = ss;

//        QueryResponse = wSTMHMM.runService(ParameterValues);
//  //  }
//  //  catch (Exception e)
//  //  {
//        //TODO do something with error
// //   }
//}



//// try
//// {
//     //runService --You need to change the sequences and ids as per your requirement. 
//     //organism and method are optional as per wsdl document

//     runServiceResponseQueueentry obj2;
//     sequence[] ss = new sequence[6];

//     sequence obj3 = new sequence();
//     obj3.id = "IPI:IPI00000006.1";
//     obj3.seq = "MTEYKLVVVGAGGVGKSALTIQLIQNHFVDEYDPTIEDSYRKQVVIDGETCLLDILDTAGQEEYSAMRDQYMRTGEGFLCVFAINNTKSFEDIHQYREQIKRVKDSDDVPMVLVGNKCDLAARTVESRQAQDLARSYGIPYIETSAKTRQGVEDAFYTLVREIRQHKLRKLNPPDESGPGCMSCKCVLS";
//     ss[0] = obj3;

//     sequence obj4 = new sequence();
//     obj4.id = "IPI:IPI00000005.1";
//     obj4.seq = "MTEYKLVVVGAGGVGKSALTIQLIQNHFVDEYDPTIEDSYRKQVVIDGETCLLDILDTAGQEEYSAMRDQYMRTGEGFLCVFAINNSKSFADINLYREQIKRVKDSDDVPMVLVGNKCDLPTRTVDTKQAHELAKSYGIPFIETSAKTRQGVEDAFYTLVREIRQYRMKKLNSSDDGTQGCMGLPCVVM";
//     ss[1] = obj4;


//     runServiceParameters obj1 = new runServiceParameters();
//     obj1.sequencedata = ss;

//     WSTMHMM_2_0_ws1 obj = new WSTMHMM_2_0_ws1();
//     obj2 = obj.runService(obj1);
//     Console.Write(obj2);

//     //pollQueue --jobid needs to be changed as which you got from runService 	
//     pollQueueResponse pollObj;
//     pollQueueJob job = new pollQueueJob();
//     string id = "D81A87A0-4030-11E1-AF58-C0FF935B49F0";
//     job.jobid = id;
//     obj = new WSTMHMM_2_0_ws1();
//     pollQueue job1 = new pollQueue();
//     job1.job = job;
//     pollObj = obj.pollQueue(job1);
//     Console.Write(pollObj);

//     //fetchResults  --jobid needs to be changed as which you got from runService
//     anndata ann;
//     fetchResultJob jobfetch = new fetchResultJob();
//     id = "4EBA8F9A-727C-11E1-A913-B5DEB25BD92E";
//     jobfetch.jobid = id;
//     obj = new WSTMHMM_2_0_ws1();
//     ann = obj.fetchResult(jobfetch);
//     Console.Write(ann);
//   }
//       catch (Exception ex)
//    {
//        Console.WriteLine("{0} Exception caught.", ex);
//    }