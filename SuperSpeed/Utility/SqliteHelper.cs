using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace SuperSpeed
{
    public class SQLiteHelper : IDisposable
    {
        #region Private Variables

        private SQLiteConnection _conn = null;
        private SQLiteConnectionStringBuilder _connectionString = null;
        private SQLiteCommand _cmd = null;
        private SQLiteDataAdapter _dataAdapter = null;

        #endregion

        #region Public Variables

        public string SQLiteConnectionString
        {
            get { return _conn.ConnectionString; }
        }      

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteHelper"/> class.
        /// </summary>
        /// <param name="filePathAndName">Name of the file path and.</param>
        public SQLiteHelper(string filePathAndName)
        {
            if (!File.Exists(filePathAndName))
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePathAndName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePathAndName));
                }
                System.Data.SQLite.SQLiteConnection.CreateFile(filePathAndName);
            }

            this._connectionString = new SQLiteConnectionStringBuilder();
            this._connectionString.DataSource = filePathAndName;
            this._connectionString.JournalMode = SQLiteJournalModeEnum.Persist;
            this._conn = new SQLiteConnection();
            this._conn.DefaultTimeout = 100;
            this._conn.ConnectionString = this._connectionString.ToString();
            this._cmd = this._conn.CreateCommand();
            this._dataAdapter = new SQLiteDataAdapter(this._cmd);
        }

        ~SQLiteHelper()
        {
            if (this._conn != null)
            {
                this._conn = null;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create Data File
        /// </summary>
        /// <param name="filePathAndName">Name of the file path and.</param>
        public static void CreateDataFile(string filePathAndName)
        {
            try
            {
                if (!File.Exists(filePathAndName))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(filePathAndName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePathAndName));
                    }
                    System.Data.SQLite.SQLiteConnection.CreateFile(filePathAndName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.CloseConnection();
        }

        /// <summary>
        /// Execute sql command
        /// </summary>
        /// <param name="commandText">sql command</param>
        public int ExecuteNonQuery(string commandText)
        {
            try
            {
                this._cmd.CommandText = commandText;
                this.OpenConnection();
                return this._cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public int ExecuteNonQuery(SQLiteTransaction transaction, string commandText)
        {
            try
            {
                IDbCommand cmd = transaction.Connection.CreateCommand();
                cmd.CommandText = commandText;
                if (transaction.Connection.State == ConnectionState.Closed)
                    transaction.Connection.Open();
                int result = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>The table of result</returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            DataTable dt = new DataTable();
            try
            {
                this._cmd.CommandText = commandText;
                this.OpenConnection();
                DataSet ds = new DataSet();
                this._dataAdapter.SelectCommand.CommandText = commandText;
                this._dataAdapter.Fill(ds);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>The table of result</returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            try
            {
                this._cmd.CommandText = commandText;
                this.OpenConnection();
                DataSet ds = new DataSet();
                this._dataAdapter.SelectCommand.CommandText = commandText;
                this._dataAdapter.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Executes the data reader.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>SQLiteDataReader object</returns>
        public SQLiteDataReader ExecuteDataReader(string commandText)
        {
            SQLiteDataReader dr = null;

            try
            {
                this._cmd.CommandText = commandText;
                this.OpenConnection();
                dr = this._cmd.ExecuteReader();

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Open connection
        /// </summary>
        /// <returns>
        /// true or false
        /// </returns>
        private bool OpenConnection()
        {
            bool result = false;
            try
            {
                if (this._conn != null && this._conn.State != ConnectionState.Open)
                {
                    this._conn.Open();
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <returns>Whether close the connection successful</returns>
        private bool CloseConnection()
        {
            bool result = false;

            try
            {
                if (this._conn != null && this._conn.State != ConnectionState.Closed)
                {
                    this._conn.Close();
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion
    }


    public class SQLiteTransactionScope : IDisposable, IDbTransaction
    {
        private SQLiteTransaction transaction;
        private SQLiteConnection connection;

        public SQLiteTransactionScope(string SQLiteConnectionString)
        {
            SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder();
            connectionString.DataSource = SQLiteConnectionString;
            connectionString.JournalMode = SQLiteJournalModeEnum.Persist;

            this.connection = new SQLiteConnection();
            this.connection.DefaultTimeout = 100;
            this.connection.ConnectionString = connectionString.ToString();
            this.connection.Open();
            this.transaction = connection.BeginTransaction();
        }

        public SQLiteTransaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }

        public void CloseDatabase()
        {
            if (this.transaction != null)
            {
                this.transaction = null;
            }

            if (this.connection != null)
            {
                if (this.connection.State == ConnectionState.Open)
                {
                    this.connection.Close();
                }
                this.connection = null;
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            this.CloseDatabase();
        }
        #endregion

        #region IDbTransaction Members

        public void Commit()
        {
            this.transaction.Commit();
        }

        public IDbConnection Connection
        {
            get
            {
                return this.transaction.Connection;
            }
        }

        public IsolationLevel IsolationLevel
        {
            get
            {
                return this.transaction.IsolationLevel;
            }
        }

        public void Rollback()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
                this.transaction = null;
            }
        }

        #endregion
    }
}
