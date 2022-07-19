using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Teleprogramma
{

    class Sqlcon
    {
        SqlConnection sqlConnect = null;

        SqlCommand sqlCommand;

        public async Task Open()
        {
            sqlConnect = new SqlConnection(@"Data Source=DESKTOP-8T9EGI7\SQLEXPRESS;Initial Catalog=" + "Teleproga" + ";Integrated Security=true");
            await sqlConnect.OpenAsync();



        }
        public async Task<DataTable> CommnadWithQuery(string query)
        {
            try
            {

                await Open();
                sqlCommand = new SqlCommand(query, sqlConnect);

                var data = await sqlCommand.ExecuteReaderAsync();
                DataTable table = new DataTable();
                table.Load(data);
                return table;

            }
            catch (Exception ex)
            {
               
                return null;
            }
        }
    }
}
