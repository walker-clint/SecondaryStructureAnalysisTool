/* $Id: wsdbfetch.cs 2011 2011-09-02 08:40:08Z hpm $
* ======================================================================
* WSDbfetch C# client.
*
* See:
* http://www.ebi.ac.uk/Tools/webservices/services/dbfetch
* http://www.ebi.ac.uk/Tools/webservices/clients/dbfetch
* http://www.ebi.ac.uk/Tools/webservices/tutorials/csharp
* ====================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecondaryStructureTool.uk.ac.ebi.www;
using SecondaryStructureTool.uk.ac.ebi.www1;
using SecondaryStructureTool.uk.ac.ebi.www.fetch;
using SecondaryStructureTool.dk.dtu.cbs.www;

namespace SecondaryStructureTool.DataModel
{
    class QueryNCBIDatabase
    {

        public QueryNCBIDatabase()
        {

        }

        #region Public Methods
        public string GetFASTA(string accessionNumber)
        {
            WSDBFetchServerService fetch = new WSDBFetchServerService();
            try
            {
                string result = fetch.fetchData("UniProtKB: Accession: " + accessionNumber, "FASTA", "RAW");
                return result;
            }
            catch (Exception e)
            {
                string problem = "There was a Problem with your request. Please check that  your accession number is correct and that the record is on the UNIprotKB database.";
                return problem;
            }
            
        }

        public string[] GetDatabases()
        {
            string[] result = null;
            WSDBFetchServerService fetch = new WSDBFetchServerService();
            try
            {
                result = fetch.getSupportedDBs();
                return result;
            }
            catch (Exception e)
            {
                result = null;
            }
            return result;
        }
        #endregion

    }
}
