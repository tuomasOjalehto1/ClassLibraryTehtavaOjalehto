using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace ClassLibraryTehtavaOjalehto
{
    public class db
    {

        public SQLiteConnection sqlite_conn = new SQLiteConnection("Data source = tehtava.db; Version=3; New=False; Compression = True");

    }
}
