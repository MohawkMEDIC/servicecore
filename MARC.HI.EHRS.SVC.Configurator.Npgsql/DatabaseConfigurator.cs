/*
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 5-12-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Data;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Configuration.Data;
using MARC.HI.EHRS.SVC.Configuration;
using MARC.HI.EHRS.SVC.Configuration.UI;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Configurator.PostgreSql9
{
    /// <summary>
    /// Database configurator for PostgreSQL 9.0
    /// </summary>
    public class DatabaseConfigurator : IDatabaseProvider
    {
        #region IDatabaseConfigurator Members


        /// <summary>
        /// Get the name of the configurator
        /// </summary>
        public string Name
        {
            get { return "PostgreSQL Data Provider"; }
        }

        /// <summary>
        /// Gets the invariant name of the configurator
        /// </summary>
        public string InvariantName
        {
            get { return "Npgsql"; }
        }

        /// <summary>
        /// Get the db provider type
        /// </summary>
        public Type DbProviderType
        {
            get
            {
                return typeof(NpgsqlFactory);
            }
        }

        /// <summary>
        /// Deploy a feature
        /// </summary>
        public void DeployFeature(IDataFeature feature, string connectionStringName, System.Xml.XmlDocument configurationDom)
        {
            // Get the embedded resource
            try
            {

                String installSql = feature.GetDeploySql(this.InvariantName),
                    checkSql = feature.GetCheckSql(this.InvariantName);

                // Deploy the feature
                string connectionString = configurationDom.SelectSingleNode(String.Format("//connectionStrings/add[@name='{0}']/@connectionString", connectionStringName)).Value;
                NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(connectionString);
                builder.MaxPoolSize = 1;
                builder.MinPoolSize = 1;
                builder.Pooling = false;
                NpgsqlConnection conn = new NpgsqlConnection(builder.ConnectionString);
                try
                {
                    conn.Open();

                    // Check for existing deployment
                    if (!String.IsNullOrEmpty(checkSql))
                        using (NpgsqlCommand cmd = new NpgsqlCommand(checkSql, conn))
                            if ((bool)cmd.ExecuteScalar())
                                return;
                    
                    if (!String.IsNullOrEmpty(installSql))
                        using (NpgsqlCommand cmd = new NpgsqlCommand(installSql, conn))
                            cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Un-deploy feature
        /// </summary>
        public void UnDeployFeature(IDataFeature feature, string connectionStringName, System.Xml.XmlDocument configurationDom)
        {
            // Get the embedded resource
            try
            {

                String uninstallSql = feature.GetUnDeploySql(this.InvariantName),
                    checkSql = feature.GetCheckSql(this.InvariantName);

                // Deploy the feature
                string connectionString = configurationDom.SelectSingleNode(String.Format("//connectionStrings/add[@name='{0}']/@connectionString", connectionStringName)).Value;
                NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(connectionString);
                builder.MaxPoolSize = 1;
                builder.MinPoolSize = 1;
                builder.Pooling = false;
                NpgsqlConnection conn = new NpgsqlConnection(builder.ConnectionString);
                try
                {
                    conn.Open();

                    // Check for existing deployment
                    if (!String.IsNullOrEmpty(checkSql))
                        using (NpgsqlCommand cmd = new NpgsqlCommand(checkSql, conn))
                            if (!(bool)cmd.ExecuteScalar())
                                return;

                    // Uninstall
                    if (!String.IsNullOrEmpty(uninstallSql))
                        using (NpgsqlCommand cmd = new NpgsqlCommand(uninstallSql, conn))
                            cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Tostring method for GUI
        /// </summary>
        public override string ToString()
        {
            return this.Name;
        }
        #endregion

        #region IDatabaseConfigurator Members

        /// <summary>
        /// Get all databases
        /// </summary>
        public string[] GetDatabases(DbConnectionString connection)
        {
            connection.Database = "postgres";
            NpgsqlConnection conn = new NpgsqlConnection(this.CreateConnectionString(connection));
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT datname FROM pg_database;", conn);
                List<String> retVal = new List<string>(10);
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        retVal.Add(Convert.ToString(reader[0]));
                return retVal.ToArray();
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Utility Functions
        /// <summary>
        /// Create connection string
        /// </summary>
        public string CreateConnectionString(DbConnectionString connectionString)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Username = connectionString.UserName;
            builder.Database = connectionString.Database;
            builder.Host = connectionString.Host;
            builder.Pooling = true;
            builder.MinPoolSize = 1;
            builder.MaxPoolSize = 10;
            builder.CommandTimeout = 240;
            builder.Add("password", connectionString.Password);
            return builder.ConnectionString;
        }

        #region IDatabaseConfigurator Members

        /// <summary>
        /// Get connection string element
        /// </summary>
        public DbConnectionString ParseConnectionString(string connectionString)
        {
            NpgsqlConnectionStringBuilder bldr = new NpgsqlConnectionStringBuilder(connectionString);
            return new DbConnectionString()
            {
                Database = bldr.Database,
                Host = bldr.Host,
                Password = bldr.Password,
                Provider = this,
                UserName = bldr.Username
            };
        }

        #endregion
        #endregion

        #region IDatabaseConfigurator Members

        /// <summary>
        /// Create a database
        /// </summary>
        public void CreateDatabase(DbConnectionString connection, string databaseName, string owner)
        {
            connection.Database = "postgres"; // connect to postgres database
            string connectionString = this.CreateConnectionString(connection);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string[] createcmds = {
                                       String.Format("CREATE DATABASE {0} WITH OWNER {1}", databaseName, owner)
                                   };

                    // create the DB
                    foreach (var cmd in createcmds)
                        new NpgsqlCommand(cmd, conn).ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }

            connection.Database = databaseName;
            connectionString = this.CreateConnectionString(connection);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Now switch db
                    try
                    {
                        string[] setupcmds = {
                                        String.Format("REVOKE ALL ON DATABASE {0} FROM public", databaseName),
                                        String.Format("GRANT ALL ON DATABASE {0} TO {1}", databaseName, owner),
                                            "CREATE OR REPLACE LANGUAGE plpgsql",
                                            "CREATE EXTENSION IF NOT EXISTS PGCRYPTO",
                                            "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"",
                                            "SET bytea_output = ESCAPE"
                            };
                        foreach (var cmd in setupcmds)
                        {
                            new NpgsqlCommand(cmd, conn).ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion

        /// <summary>
        /// Get the updates 
        /// </summary>
        public List<IDataUpdate> GetUpdates(string connectionStringName, XmlDocument configurationDom)
        {
            string connectionString = configurationDom.SelectSingleNode(String.Format("//connectionStrings/add[@name='{0}']/@connectionString", connectionStringName)).Value;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Now switch db
                    try
                    {
                        return DatabaseConfiguratorRegistrar.Updates.FindAll(o =>
                        {
                            var checkSql = o.GetCheckSql(this.InvariantName);
                            if (String.IsNullOrEmpty(checkSql)) return false;

                            using (NpgsqlCommand cmd = new NpgsqlCommand(checkSql, conn))
                                return !(bool)cmd.ExecuteScalar();
                        });

                    }
                    catch
                    {
                        throw;
                    }
                }
                catch
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Deploy the update
        /// </summary>
        public void DeployUpdate(IDataUpdate update, string connectionStringName, XmlDocument configurationDom)
        {
            string connectionString = configurationDom.SelectSingleNode(String.Format("//connectionStrings/add[@name='{0}']/@connectionString", connectionStringName)).Value;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var checkSql = update.GetCheckSql(this.InvariantName);
                if (!String.IsNullOrEmpty(checkSql))
                    using (NpgsqlCommand cmd = new NpgsqlCommand(checkSql, conn))
                        if (!(bool)cmd.ExecuteScalar()) return;

                var deplSql = update.GetDeploySql(this.InvariantName);
                if (!String.IsNullOrEmpty(deplSql))
                    using (NpgsqlCommand cmd = new NpgsqlCommand(deplSql, conn))
                        cmd.ExecuteNonQuery();
            }
        }


    }
}
