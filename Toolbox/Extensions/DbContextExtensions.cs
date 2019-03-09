using System;
using System.Collections.Generic;
using System.Text;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
//using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;

namespace Toolbox.Extensions
{
    
    public static class DbContextExtensions
    {
        // Commented out until I can figure out how to call sprocs from EFCore
        /*
            #region [+] SPROC Execution Utilities

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="context"></param>
            /// <param name="procedureName"></param>
            /// <param name="parameters"></param>
            /// <param name="timeOut">Sets CommandTimeout for the sproc call. DO NOT SET IF YOU DO NOT NEED TO!!! NEIN!!! EINFACH NEIN!</param>
            /// <returns></returns>
            public static List<T> ExecuteStoredProcedure<T>(this DbContext context, string procedureName, IDictionary<string, object> parameters, int? timeOut = null)
            {
                List<SqlParameter> sqlParamList;
                // Build the sql string to execute and build the lst of sql parameters to pass in.
                var queryString = QueryHelper.BuildProcedureQueryString(procedureName, parameters, out sqlParamList);

                return context.ExecuteStoredProcedureInternal<T>(queryString, sqlParamList, timeOut);
            }

            ///// <summary>
            ///// 
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="context"></param>
            ///// <param name="sprocContext"></param>
            ///// <returns></returns>
            //public static List<T> ExecuteStoredProcedure<T>(this DbContext context, IStoredProcedureContext sprocContext)
            //{
            //    List<SqlParameter> sqlParamList;
            //    var queryString = QueryHelper.BuildProcedureQueryString(sprocContext, out sqlParamList);

            //    return ExecuteStoredProcedureInternal<T>(context, queryString, sqlParamList, sprocContext.TimeOut);
            //}

            private static List<T> ExecuteStoredProcedureInternal<T>(this DbContext context, string queryString, IEnumerable<SqlParameter> sqlParamList, int? timeOut = null)
            {
                //var originalTimeOut = context.Database.CommandTimeout;
                try
                {
                    //// set command timeout, if it was sent.
                    //if (timeOut.HasValue)
                    //    context.Database.CommandTimeout = timeOut;

                    return context.Database.SqlQuery<T>(queryString, sqlParamList.ToArray<object>()).ToList();
                }
                finally
                {
                    //// reset commandTimeOut
                    //if (context.Database.CommandTimeout != originalTimeOut)
                    //    context.Database.CommandTimeout = originalTimeOut;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="context"></param>
            /// <param name="procedureName"></param>
            /// <param name="parameters"></param>
            /// <param name="timeOut">Sets CommandTimeout for the sproc call. DO NOT SET IF YOU DO NOT NEED TO!!! NEIN!!! EINFACH NEIN!</param>
            /// <returns></returns>
            public static T ExecuteStoredProcedureSingleValue<T>(this DbContext context, string procedureName, IDictionary<string, object> parameters, int? timeOut = null)
            {
                List<SqlParameter> sqlParamList;
                // Build the sql string to execute and build the lst of sql parameters to pass in.
                var queryString = QueryHelper.BuildProcedureQueryString(procedureName, parameters, out sqlParamList);

                return context.ExecuteStoredProcedureSingleValueInternal<T>(queryString, sqlParamList, timeOut);
            }

            private static T ExecuteStoredProcedureSingleValueInternal<T>(this DbContext context, string queryString, IEnumerable<SqlParameter> sqlParamList, int? timeOut = null)
            {
                //var originalTimeOut = context.Database.CommandTimeout;
                try
                {
                    //// set command timeout, if it was sent.
                    //if (timeOut.HasValue)
                    //    context.Database.CommandTimeout = timeOut;

                    return context.Database.SqlQuery<T>(queryString, sqlParamList.ToArray<object>()).FirstOrDefault();
                }
                finally
                {
                    //// reset commandTimeOut
                    //if (context.Database.CommandTimeout != originalTimeOut)
                    //    context.Database.CommandTimeout = originalTimeOut;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <param name="procedureName"></param>
            /// <param name="parameters"></param>
            /// <param name="timeOut">Sets CommandTimeout for the sproc call. DO NOT SET IF YOU DO NOT NEED TO!!! NEIN!!! EINFACH NEIN!</param>
            /// <returns></returns>
            public static int ExecuteStoredProcedure(this DbContext context, string procedureName, IDictionary<string, object> parameters = null, int? timeOut = null)
            {
                List<SqlParameter> sqlParamList;
                var queryString = QueryHelper.BuildProcedureQueryString(procedureName, parameters, out sqlParamList);

                return ExecuteStoredProcedureInternal(context, queryString, sqlParamList, timeOut);
            }

            private static int ExecuteStoredProcedureInternal(this DbContext context, string queryString, IEnumerable<SqlParameter> sqlParamList, int? timeOut = null)
            {
                //var originalTimeOut = context.Database.CommandTimeout;
                try
                {
                    //// set command timeout, if it was sent.
                    //if (timeOut.HasValue)
                    //    context.Database.CommandTimeout = timeOut;

                    return context.Database.ExecuteSqlCommand(queryString, sqlParamList.ToArray<object>());
                }
                finally
                {
                    //// reset commandTimeOut
                    //if (context.Database.CommandTimeout != originalTimeOut)
                    //    context.Database.CommandTimeout = originalTimeOut;
                }
            }

            #endregion

            #region [+] Async SPROC Execution Utilities

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="context"></param>
            /// <param name="procedureName"></param>
            /// <param name="parameters"></param>
            /// <param name="timeOut">Sets CommandTimeout for the sproc call. DO NOT SET IF YOU DO NOT NEED TO!!! NEIN!!! EINFACH NEIN!</param>
            /// <returns></returns>
            public static Task<List<T>> ExecuteStoredProcedureAsync<T>(this DbContext context, string procedureName, IDictionary<string, object> parameters, int? timeOut = null)
            {
                List<SqlParameter> sqlParamList;
                // Build the sql string to execute and build the lst of sql parameters to pass in.
                var queryString = QueryHelper.BuildProcedureQueryString(procedureName, parameters, out sqlParamList);

                return context.ExecuteStoredProcedureAsyncInternal<T>(queryString, sqlParamList, timeOut);
            }

            private static Task<List<T>> ExecuteStoredProcedureAsyncInternal<T>(this DbContext context, string queryString, IEnumerable<SqlParameter> sqlParamList, int? timeOut = null)
            {
                //var originalTimeOut = context.Database.CommandTimeout;
                try
                {
                    //// set command timeout, if it was sent.
                    //if (timeOut.HasValue)
                    //    context.Database.CommandTimeout = timeOut;
                    return context.Database.FromSql(new RawSqlString(queryString), sqlParamList.ToArray<object>()).ToListAsync();
                }
                finally
                {
                    //// reset commandTimeOut
                    //if (context.Database.CommandTimeout != originalTimeOut)
                    //    context.Database.CommandTimeout = originalTimeOut;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <param name="procedureName"></param>
            /// <param name="parameters"></param>
            /// <param name="timeOut">Sets CommandTimeout for the sproc call. DO NOT SET IF YOU DO NOT NEED TO!!! NEIN!!! EINFACH NEIN!</param>
            /// <returns></returns>
            public static Task<T> ExecuteStoredProcedureSingleValueAsync<T>(this DbContext context, string procedureName, IDictionary<string, object> parameters, int? timeOut = null)
            {
                List<SqlParameter> sqlParamList;
                // Build the sql string to execute and build the lst of sql parameters to pass in.
                var queryString = QueryHelper.BuildProcedureQueryString(procedureName, parameters, out sqlParamList);

                return context.ExecuteStoredProcedureSingleValueAsyncInternal<T>(queryString, sqlParamList, timeOut);
            }

            private static Task<T> ExecuteStoredProcedureSingleValueAsyncInternal<T>(this DbContext context, string queryString, IEnumerable<SqlParameter> sqlParamList, int? timeOut = null)
            {
                //var originalTimeOut = context.Database.CommandTimeout;
                try
                {
                    //// set command timeout, if it was sent.
                    //if (timeOut.HasValue)
                    //    context.Database.CommandTimeout = timeOut;

                    return context.Database.FromS<T>(queryString, sqlParamList.ToArray<object>()).FirstOrDefaultAsync();
                }
                finally
                {
                    //// reset commandTimeOut
                    //if (context.Database.CommandTimeout != originalTimeOut)
                    //    context.Database.CommandTimeout = originalTimeOut;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="context"></param>
            /// <param name="procedureName"></param>
            /// <param name="parameters"></param>
            /// <param name="timeOut">Sets CommandTimeout for the sproc call. DO NOT SET IF YOU DO NOT NEED TO!!! NEIN!!! EINFACH NEIN!</param>
            /// <returns></returns>
            public static Task<int> ExecuteStoredProcedureAsync(this DbContext context, string procedureName, IDictionary<string, object> parameters = null, int? timeOut = null)
            {
                List<SqlParameter> sqlParamList;
                var queryString = QueryHelper.BuildProcedureQueryString(procedureName, parameters, out sqlParamList);

                return ExecuteStoredProcedureAsyncInternal(context, queryString, sqlParamList, timeOut);
            }

            private static Task<int> ExecuteStoredProcedureAsyncInternal(this DbContext context, string queryString, IEnumerable<SqlParameter> sqlParamList, int? timeOut = null)
            {
                //var originalTimeOut = context.Database.CommandTimeout;
                try
                {
                    //// set command timeout, if it was sent.
                    //if (timeOut.HasValue)
                    //    context.Database.CommandTimeout = timeOut;

                    return context.Database.ExecuteSqlCommandAsync(queryString, sqlParamList.ToArray<object>());
                }
                finally
                {
                    //// reset commandTimeOut
                    //if (context.Database.CommandTimeout != originalTimeOut)
                    //    context.Database.CommandTimeout = originalTimeOut;
                }
            }

            #endregion

            /// <summary>
            /// Utility class used in aiding of the database queries by performing operations such as:
            /// 
            /// <list type="bullet">
            ///     <item>Query manipulation, </item>
            ///     <item>Parameter construction, </item>
            ///     <item>Stored procedure execution,</item>
            /// </list>
            /// </summary>
            public static class QueryHelper
            {
                /// <summary>
                /// The prefix for the EXEC statement.
                /// </summary>
                public const string ExecStatementPrefixFormat = "EXEC {0} ";

                /// <summary>
                /// The parameter delimiter in the EXEC statement.
                /// </summary>
                public const string ExecStatementParameterDelimiter = ", ";

                /// <summary>
                /// The format of parameters in the EXEC statement.
                /// </summary>
                public const string ExecStatementParameterFormat = "@{0} = @{1}";

                #region [+] Utilities

                #region [+] Query Builder Utilities

                /// <summary>
                ///     Builds the stored procedure sql string to execute based on the <paramref name="procedureName" /> and
                ///     <paramref name="parameters" /> passed in.
                /// </summary>
                /// <param name="procedureName">The name of the procedure we are looking to execute.</param>
                /// <param name="parameters">A dictionary of parameter name/value pairs to pass into the procedure.</param>
                /// <param name="sqlParamList">Out: A list of <see cref="SqlParameter" /> to pass the values into the procedure.</param>
                /// <returns>The formatted sql string to execute and the list of paramter values to pass in.</returns>
                public static string BuildProcedureQueryString(string procedureName, IDictionary<string, object> parameters,
                    out List<SqlParameter> sqlParamList)
                {
                    return BuildProcedureQueryStringInternal(
                        procedureName,
                        parameters,
                        (parameter, i) => new ParameterIndexer(i, parameter.Key, parameter.Value),
                        out sqlParamList);
                }

                /// <summary>
                ///     Builds the stored procedure sql string to execute based on the <paramref name="procedureName" /> and
                ///     <paramref name="parameters" /> passed in.
                /// </summary>
                /// <param name="procedureName">The name of the procedure we are looking to execute.</param>
                /// <param name="parameters">A object of parameters to pass into the procedure.</param>
                /// <param name="sqlParamList">Out: A list of <see cref="SqlParameter" /> to pass the values into the procedure.</param>
                /// <returns>The formatted sql string to execute and the list of paramter values to pass in.</returns>
                public static string BuildProcedureQueryString(string procedureName, object parameters, out List<SqlParameter> sqlParamList)
                {
                    return BuildProcedureQueryString(procedureName, ToPropertyDictionary(parameters), out sqlParamList);
                }

                //public static string BuildProcedureQueryString(IStoredProcedureContext sprocContext, out List<SqlParameter> sqlParamList)
                //{
                //    return BuildProcedureQueryStringInternal(
                //        sprocContext.StoredProcedureName,
                //        sprocContext.ParameterList,
                //        (parameter, i) => new ParameterIndexer(i, parameter),
                //        out sqlParamList);
                //}

                /// <summary>
                /// 
                /// </summary>
                /// <typeparam name="T">Type of parameters being sent</typeparam>
                /// <param name="procedureName">The name of the procedure we are looking to execute.</param>
                /// <param name="parameters">An enumarable set of parameters</param>
                /// <param name="indexerFunc">A function that maps a value and index to a <see cref="ParameterIndexer" /></param>
                /// <param name="sqlParamList">Out: A list of <see cref="SqlParameter" /> to pass the values into the procedure.</param>
                /// <returns></returns>
                private static string BuildProcedureQueryStringInternal<T>(string procedureName, IEnumerable<T> parameters, Func<T, int, ParameterIndexer> indexerFunc, out List<SqlParameter> sqlParamList)
                {
                    parameters = parameters ?? new List<T>();
                    var queryStringBuilder = RetrieveExecStatementQueryBuilder(procedureName);
                    var indexedParameters = parameters.Select(indexerFunc).ToList();
                    var indexedFormattedParameters = indexedParameters.Select(x => string.Format(ExecStatementParameterFormat, x.ParameterName, x.Index));
                    sqlParamList = indexedParameters.Select(indexer => indexer.Parameter).ToList();

                    queryStringBuilder.Append(string.Join(ExecStatementParameterDelimiter, indexedFormattedParameters));

                    return queryStringBuilder.ToString();
                }

                #endregion


                #region [+] Miscellaneous Utilities

                /// <summary>
                /// Retreives the query builder used for executing EXEC statemnts.
                /// </summary>
                /// <param name="storedProcedureName"></param>
                /// <returns></returns>
                private static StringBuilder RetrieveExecStatementQueryBuilder(string storedProcedureName)
                {
                    var execStatementPrefix = string.Format(ExecStatementPrefixFormat, storedProcedureName);
                    var queryStringBuilder = new StringBuilder(execStatementPrefix);
                    return queryStringBuilder;
                }


                /// <summary>
                /// This utility function will generate a formatted debug message for the query string that will
                /// be used when executing a stored procedure.
                /// </summary>
                /// <param name="storedProcedureName"></param>
                /// <param name="parameterList"></param>
                /// <returns></returns>
                public static string BuildDebugSprocQueryString(string storedProcedureName, IDictionary<string, object> parameterList)
                {
                    List<SqlParameter> outParams;
                    var queryString = BuildProcedureQueryString(storedProcedureName, parameterList, out outParams);
                    const string paramStrFormat = "@{0}: {1} \n";
                    var parameterBuilder = new StringBuilder(queryString);
                    parameterBuilder.Append("\n");
                    parameterBuilder.Append("Parameters\n");
                    parameterBuilder.Append("==========\n");
                    foreach (var param in outParams)
                    {
                        parameterBuilder.Append(string.Format(paramStrFormat, param.ParameterName, param.Value));
                    }
                    return parameterBuilder.ToString();
                }

                public static IDictionary<string, object> ToPropertyDictionary(object obj)
                {
                    if (obj == null) return null;
                    if (obj is IDictionary<string, object>) return (IDictionary<string, object>)obj;

                    var dictionary = new Dictionary<string, object>();
                    var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    foreach (var propertyInfo in properties)
                        dictionary[propertyInfo.Name] = propertyInfo.GetValue(obj, null);
                    return dictionary;
                }

                #endregion

                #endregion
            }

            public class ParameterIndexer
            {
                #region [+] Constructors

                /// <summary>
                /// 
                /// </summary>
                /// <param name="index"></param>
                /// <param name="parameterName"></param>
                /// <param name="parameterValue"></param>
                /// <param name="parameterType"></param>
                /// <param name="typeName"></param>
                public ParameterIndexer(int index, string parameterName, object parameterValue)
                {
                    Index = index.ToString();
                    ParameterName = parameterName;
                    Parameter = new SqlParameter
                    {
                        ParameterName = Index,
                        Value = parameterValue ?? DBNull.Value
                    };
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="parameter"></param>
                /// <param name="index"></param>
                public ParameterIndexer(int index, SqlParameter parameter)
                {
                    Index = index.ToString();
                    ParameterName = parameter.ParameterName;
                    Parameter = new SqlParameter
                    {
                        ParameterName = Index,
                        Value = parameter.Value ?? DBNull.Value,
                        SqlDbType = parameter.SqlDbType,
                        TypeName = parameter.TypeName
                    };
                }

                #endregion

                #region [+] Properties
                /// <summary>
                /// The index of the parameter.
                /// </summary>
                public string Index { get; set; }

                /// <summary>
                /// The name of the paramter.
                /// </summary>
                public string ParameterName { get; set; }

                public SqlParameter Parameter { get; private set; }
                #endregion

                /// <summary>
                /// This retrieve the SQL parameter with the correct indexing.
                /// </summary>
                /// <returns></returns>
                public SqlParameter RetrieveIndexedSqlParameter()
                {
                    return Parameter;
                }
            }
        */
    }
}
