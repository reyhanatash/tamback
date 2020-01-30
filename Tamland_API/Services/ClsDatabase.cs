using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace Tamland_API
{
    public class ClsDatabase
    {
        static public string ServerName = "";
        static public string DatabaseName = "";
        static public string JabizDatabaseName = "";
        static public string DBUserName = "";
        static public string DBPass = "";

        static public int RunFileScript(string FileName, int CnnType, string DBName, String JabizDBName)
        {
            try
            {
                string script = File.ReadAllText(FileName);
                script.Replace("N'Chart'", "N'" + DBName + "'");
                // split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                         RegexOptions.Multiline | RegexOptions.IgnoreCase);
                using (SqlConnection SCon = new SqlConnection(ConnectionString(CnnType)))
                {
                    SCon.Open();
                    foreach (string commandString in commandStrings)
                    {
                        if (commandString.Trim() != "")
                        {
                            using (var command = new SqlCommand(commandString, SCon))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    SCon.Close();
                }
                return 0;
            }
            catch (Exception)
            {
                return -1;
                throw;
            }
        }

        static private string ConnectionString()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnServer"].ConnectionString);
            return con.ConnectionString;
        }

        static private string ConnectionString(int CnnType)
        {
            string cn = "";
            if (CnnType == 1)
            {
                cn = WebConfigurationManager.ConnectionStrings["ConnServer"].ConnectionString;
            }
            else if (CnnType == 2)
            {
                cn = WebConfigurationManager.ConnectionStrings["ConnServer"].ConnectionString;
            }

            SqlConnection con = new SqlConnection(cn);
            return con.ConnectionString;
        }

        static public object ExecuteScalerSP(string SpName, DbParameter[] SpParams)
        {

            using (SqlConnection SCon = new SqlConnection(ConnectionString()))
            {
                try
                {
                    SCon.Open();

                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandTimeout = 600;
                        SC.CommandType = CommandType.StoredProcedure;
                        SC.Parameters.Clear();
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }


                        return SC.ExecuteScalar();
                    }
                }
                catch
                {
                    return null;

                }


            }

        }

        static public object ExecuteScalerSP(string SpName, DbParameter[] SpParams, int CnnType)
        {
            using (SqlConnection SCon = new SqlConnection(ConnectionString(CnnType)))
            {
                try
                {
                    SCon.Open();
                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandTimeout = 600;
                        SC.CommandType = CommandType.StoredProcedure;
                        SC.Parameters.Clear();
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }

                        return SC.ExecuteScalar();
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        internal static DbParameter[] GenParameters(string v1, int id, string v2, int peopleType, string v3, string firstName, string v4, string surName, string v5, string v6, string phoneNum, string v7, string email, string v8, string notes)
        {
            throw new NotImplementedException();
        }

        static public object ExecuteScalerSP(string SpName, DbParameter[] SpParams, string ConnectionStr)
        {

            using (SqlConnection SCon = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SCon.Open();

                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandTimeout = 600;
                        SC.CommandType = CommandType.StoredProcedure;
                        SC.Parameters.Clear();
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }

                        return SC.ExecuteScalar();
                    }
                }
                catch
                {
                    return null;

                }


            }

        }

        static public int ExecuteNonQuerySP(string SpName, DbParameter[] SpParams)
        {

            using (SqlConnection SCon = new SqlConnection(ConnectionString()))
            {
                try
                {
                    SCon.Open();

                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandType = CommandType.StoredProcedure;
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        return SC.ExecuteNonQuery();
                    }
                }
                catch
                {
                    return -1;

                }


            }

        }

        static public int ExecuteNonQuerySP(string SpName, DbParameter[] SpParams, int CnnType)
        {

            using (SqlConnection SCon = new SqlConnection(ConnectionString(CnnType)))
            {
                try
                {
                    SCon.Open();

                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandType = CommandType.StoredProcedure;
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        return SC.ExecuteNonQuery();
                    }
                }
                catch
                {
                    return -1;

                }


            }

        }

        static public int ExecuteNonQuerySP(string SpName, DbParameter[] SpParams, string ConnectionStr)
        {

            using (SqlConnection SCon = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SCon.Open();

                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandType = CommandType.StoredProcedure;
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        return SC.ExecuteNonQuery();
                    }
                }
                catch
                {
                    return -1;

                }


            }

        }

        static public DataTable ExecuteDatatable(string Text, DbParameter[] SpParams)
        {

            using (SqlConnection SCon = new SqlConnection(ConnectionString()))
            {
                try
                {
                    SCon.Open();
                    using (SqlCommand SC = new SqlCommand(Text, SCon))
                    {
                        SC.CommandTimeout = 600;

                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        DataTable DT = new DataTable();

                        using (SqlDataReader dr = SC.ExecuteReader())
                        {
                            DT.Load(dr);
                            return DT;
                        }
                    }
                }
                catch
                {

                    return null;
                }

            }

        }

        static public DataTable ExecuteDatatable(string Text, DbParameter[] SpParams, int CnnTyp)
        {

            using (SqlConnection SCon = new SqlConnection(ConnectionString(CnnTyp)))
            {
                try
                {
                    SCon.Open();
                    using (SqlCommand SC = new SqlCommand(Text, SCon))
                    {
                        SC.CommandTimeout = 600;
                        SC.CommandType = CommandType.StoredProcedure;
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        DataTable DT = new DataTable();

                        using (SqlDataReader dr = SC.ExecuteReader())
                        {
                            DT.Load(dr);
                            return DT;
                        }
                    }
                }
                catch (Exception e)
                {

                    return null;
                }

            }

        }

        static public DataTable ExecuteDatatable(string Text, DbParameter[] SpParams, string ConnectionStr)
        {

            using (SqlConnection SCon = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SCon.Open();
                    using (SqlCommand SC = new SqlCommand(Text, SCon))
                    {
                        SC.CommandTimeout = 6000;

                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        DataTable DT = new DataTable();

                        using (SqlDataReader dr = SC.ExecuteReader())
                        {
                            DT.Load(dr);
                            return DT;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("An error occurred", e);

                }

            }

        }

        static public DataTable ExecuteDatatableSP(string SpName, DbParameter[] SpParams)
        {


            using (SqlConnection SCon = new SqlConnection(ConnectionString()))
            {
                try
                {
                    SCon.Open();

                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandTimeout = 6000;
                        SC.CommandType = CommandType.StoredProcedure;

                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        DataTable DT = new DataTable();


                        using (SqlDataReader dr = SC.ExecuteReader())
                        {
                            DT.Load(dr);
                            return DT;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("An error occurred", e);

                }

            }


        }

        static public DataTable ExecuteDatatableSP(string SpName, DbParameter[] SpParams, int CnnTyp)
        {


            using (SqlConnection SCon = new SqlConnection(ConnectionString(CnnTyp)))
            {
                try
                {
                    SCon.Open();

                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandTimeout = 6000;
                        SC.CommandType = CommandType.StoredProcedure;
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        DataTable DT = new DataTable();


                        using (SqlDataReader dr = SC.ExecuteReader())
                        {
                            DT.Load(dr);
                            return DT;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred", ex);

                }

            }


        }

        static public DataTable ExecuteDatatableSP(string SpName, DbParameter[] SpParams, string ConnectionStr)
        {


            using (SqlConnection SCon = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SCon.Open();

                    using (SqlCommand SC = new SqlCommand(SpName, SCon))
                    {
                        SC.CommandTimeout = 600;
                        SC.CommandType = CommandType.StoredProcedure;
                        foreach (DbParameter P in SpParams)
                        {
                            SC.Parameters.Add(P);
                        }
                        DataTable DT = new DataTable();


                        using (SqlDataReader dr = SC.ExecuteReader())
                        {
                            DT.Load(dr);
                            return DT;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred", ex);

                }

            }


        }


        #region [Generate Parameters]


        static public DbParameter GenParam(string ParamName, object ParamValue)
        {
            return new SqlParameter(ParamName, ParamValue);
        }

        static public DbParameter[] GenParameters(Int32 Count)
        {

            return new SqlParameter[Count];
        }

        static public DbParameter[] GenParameters(DbParameter Par)
        {
            SqlParameter[] res = new SqlParameter[1];
            res[0] = (SqlParameter)Par;
            return res;
        }

        static public DbParameter[] GenParameters(string PName, object PValue)
        {
            SqlParameter[] res = new SqlParameter[1];
            res[0] = new SqlParameter(PName, PValue);
            return res;
        }

        public static DbParameter[] GenParameters(params object[] pNames)
        {
            SqlParameter[] res = new SqlParameter[pNames.Length / 2];

            for (int i = 0; i < pNames.Length; i = i + 2)
            {
                res[i / 2] = new SqlParameter(pNames[i].ToString(), pNames[i + 1]);

            }
            return res;
        }



        #endregion


    }
}
