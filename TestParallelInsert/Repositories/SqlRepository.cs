using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;

namespace TestParallelInsert.Repositories
{
    public class SqlRepository: IRepository
    {
        private readonly ConnectionSettings _connectionSettings;
        private string WeChatConnection => _connectionSettings.DataCenter;

        public SqlRepository(IOptions<ConnectionSettings> connectionSettings)
        {
            _connectionSettings = connectionSettings?.Value ??
                                        throw new ArgumentException("Value is not defined.", nameof(connectionSettings));
        }

        public string InsertData(string untranslated, string translated)
        {
            using (var connection = new SqlConnection(WeChatConnection))
            {
                string sql = @"
                            INSERT INTO [wxpay].[Translations]([Untranslated],[Translated])
                                VALUES(@Untranslated,@Translated);
                            SELECT id from  [wxpay].[Translations]
	                         where [Untranslated] = @Untranslated and [Translated] = @Translated";

                var result = connection.Query(sql, new { Untranslated = untranslated, Translated = translated }).Single();
                return Convert.ToString(result.id);
            }
        }
    }
}
