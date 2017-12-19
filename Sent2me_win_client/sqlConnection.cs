using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sent2me_win_client {
    class sqlConnection {
        public SqlConnection sqlCon = new SqlConnection("Data Source=.;Initial Catalog=sent2me;Persist Security Info=True;User ID=sa;Password=1234");
        public SqlCommand cmd;
        public SqlDataAdapter da;
    }
}
