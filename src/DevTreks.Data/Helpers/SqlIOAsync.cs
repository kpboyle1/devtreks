using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		Sql stored procedure utilities  
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///Notes:	    160 async sps meant open and closing connection for each run sp
    ///             2.0.0 uses an informal EF7-style constructor w/ 1 std connection method
    /// </summary>
    public class SqlIOAsync : IDisposable
    {
        //intended to ultimately mimic an EF7-style DataContext constructor
        //this constructor uses uri.URIDataManager.less formally
        //than using DbContextOptions options in the constructor
        //supports a potential upgrade to an IServiceProvider constructor
        public SqlIOAsync(ContentURI uri) 
        {
            _uri = uri;
        }
        private ContentURI _uri { get; set; }

        //async requires opening new for each sp, using datareader, then disposing in calling procedure
        //example
        //SqlIOAsync sqlIO = new SqlIOAsync();
        //SqlDataReader dataReader = await RunProc...
        //using(datareader)
        //{
        // }
        //sqlIO.Dispose()
     
        private SqlConnection con;
        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Stored procedure return value.</returns>
        public async Task<int> RunProcIntAsync(string procName)
        {
            // make sure connection is open
            // 160 requires new open connection
            SqlCommand cmd = await CreateCommandAsync(procName, null);
            await cmd.ExecuteNonQueryAsync();
            return (int)cmd.Parameters["ReturnValue"].Value;
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <returns>Stored procedure return value.</returns>
        public async Task<int> RunProcIntAsync(string procName, SqlParameter[] prams)
        {
            // make sure connection is open
            SqlCommand cmd = await CreateCommandAsync(procName, prams);
            await cmd.ExecuteNonQueryAsync();
            //several sps don't use this return value (they just receive output params)
            return (int)cmd.Parameters["ReturnValue"].Value;
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public async Task<SqlDataReader> RunProcAsync(string procName)
        {
            // make sure connection is open
            SqlCommand cmd = await CreateCommandAsync(procName, null);
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
            return dataReader;
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public async Task<SqlDataReader> RunProcAsync(string procName, 
            SqlParameter[] prams)
        {
            // make sure connection is open
            SqlCommand cmd = await CreateCommandAsync(procName, prams);
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
            return dataReader;
        }
        //stylesheets get resourceurls but can't be async
        public SqlDataReader RunProc(string procName,
            SqlParameter[] prams)
        {
            // make sure connection is open
            SqlCommand cmd = CreateCommand(procName, prams);
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dataReader;
        }
        public async Task<SqlDataReader> RunSequentialProcAsync(string procName, 
            SqlParameter[] prams)
        {
            // make sure connection is open
            SqlCommand cmd = await CreateCommandAsync(procName, prams);
            //blob data (varbinary) is more efficient when reading sequentially
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SequentialAccess);
            return dataReader;
        }

        /// <summary>
        /// Run 2 stored procedures using same command (so that a row count and sqldatareader can be used).
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure parameters.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public async Task<SqlDataReader> Run2ProcAsync(string procName, 
            SqlParameter[] prams, int outputPramPosition)
        {
            //find a way to return row count too
            int rowCount = 0;
            // make sure connection is open
            SqlCommand cmd = await CreateCommandAsync(procName, prams);
            System.Object oReturnFirstRowColumn = cmd.ExecuteScalar();
            //used to get the row count
            if (prams[outputPramPosition].Value != System.DBNull.Value) rowCount = (int)prams[outputPramPosition].Value;
            //used to get the sqldatareader
            SqlDataReader dataReader 
                = await cmd.ExecuteReaderAsync();
            return dataReader;
        }
        public async Task<int> RunRowCountAsync(string procName, SqlParameter[] prams, 
            int outputPramPosition)
        {
            int rowCount = 0;
            // make sure connection is open
            SqlCommand cmd = await CreateCommandAsync(procName, prams);
            System.Object oReturnFirstRowColumn = await cmd.ExecuteScalarAsync();
            //used to get the row count
            if (prams[outputPramPosition].Value != System.DBNull.Value) 
                rowCount = (int)prams[outputPramPosition].Value;
            return rowCount;
        }
        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public async Task<int> RunProcIdAsync(string procName, SqlParameter[] prams)
        {
            int id = 0;
            // make sure connection is open
            SqlCommand cmd = await CreateCommandAsync(procName, prams);
            System.Object oReturnObject = await cmd.ExecuteScalarAsync();
            if (oReturnObject == null)
            {
                id = 0;
            }
            else
            {
                id = (int)Convert.ToInt32(oReturnObject.ToString());
            }
            return id;
        }
        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public async Task<string> RunProcStringAsync(string procName, 
            SqlParameter[] prams )
        {
            string sScalarOutParam = string.Empty;
            // make sure connection is open
            SqlCommand cmd = await CreateCommandAsync(procName, prams);
            System.Object oReturnObject = await cmd.ExecuteScalarAsync();
            if (oReturnObject == null)
            {
                sScalarOutParam = string.Empty;
            }
            else
            {
                sScalarOutParam = oReturnObject.ToString();
            }
            return sScalarOutParam;
        }
        
        /// <summary>
        /// Create command object used to call stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Params to stored procedure.</param>
        /// <returns>Command object.</returns>
        private async Task<SqlCommand> CreateCommandAsync(string procName, SqlParameter[] prams)
        {
            // make sure connection is open
            await OpenAsync(Helpers.AppSettings.GetConnection(_uri));
            SqlCommand cmd = new SqlCommand(procName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procName;
            // add proc parameters
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }

            // return param
            cmd.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));
            return cmd;
        }
        private SqlCommand CreateCommand(string procName, SqlParameter[] prams)
        {
            Open(Helpers.AppSettings.GetConnection(_uri));
            SqlCommand cmd = new SqlCommand(procName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procName;
            // add proc parameters
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }

            // return param
            cmd.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));
            return cmd;
        }
        private async Task OpenAsync(string connect)
        {
            // open connection
            if (con == null)
            {
                if (!string.IsNullOrEmpty(connect))
                {
                    con = new SqlConnection(connect);
                    await con.OpenAsync();
                }
            }
            else
            {
                //if this is hit the connection has a bug and needs to be newed and disposed like everything else
                //each command must be opened and closed separately
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                await con.OpenAsync();
            }
        }
        private void Open(string connect)
        {
            // open connection
            if (con == null)
            {
                if (!string.IsNullOrEmpty(connect))
                {
                    con = new SqlConnection(connect);
                    con.Open();
                }
            }
            else
            {
                //if this is hit the connection has a bug and needs to be newed and disposed like everything else
                //each command must be opened and closed separately
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
            }
        }
        
        public void Dispose()
        {
            // make sure connection is closed
            if (con != null)
            {
                con.Close();
                con.Dispose();
                con = null;
            }
        }

        /// <summary>
        /// Make input param.
        /// </summary>
        /// <param name="paramName">Name of parameter</param>
        /// <param name="dbType">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParam(string paramName, SqlDbType dbType, int size, object value)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
        }
        /// <summary>
        /// Make stored procedure parameter
        /// </summary>
        /// <param name="paramName">Name of parameter.</param>
        /// <param name="dbType">Parameter type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="Direction">Parameter direction.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeParam(string paramName, SqlDbType dbType, Int32 size, ParameterDirection Direction, object value)
        {
            SqlParameter param;
            if (size > 0)
                param = new SqlParameter(paramName, dbType, size);
            else
                param = new SqlParameter(paramName, dbType);
            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && value == null))
                param.Value = value;
            return param;
        }
        /// <summary>
        /// Make input param.
        /// </summary>
        /// <param name="paramName">Name of param.</param>
        /// <param name="dbType">Param type.</param>
        /// <param name="size">Param size.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeOutParam(string paramName, SqlDbType dbType, int size)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Output, null);
        }

    }
}
