using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.glhp.Nodes
{
    public class SqlGenerator
    {
        public static string CreateVisitInfoTableMSSQL()
        {
            var sql = new StringBuilder();
            sql.AppendLine("CREATE TABLE VisitInfo (");
            sql.AppendLine("    ID INT IDENTITY(1,1) PRIMARY KEY,");
            sql.AppendLine("    Account NVARCHAR(100),");
            sql.AppendLine("    LatitudeE7 FLOAT,");
            sql.AppendLine("    LongitudeE7 FLOAT,");
            sql.AppendLine("    Address NVARCHAR(255),");
            sql.AppendLine("    LocationConfidence FLOAT,");
            sql.AppendLine("    DeviceTag NVARCHAR(100),");
            sql.AppendLine("    StartTimestamp DATETIME,");
            sql.AppendLine("    EndTimestamp DATETIME,");
            sql.AppendLine("    PlaceConfidence NVARCHAR(50),");
            sql.AppendLine("    PlaceVisitType NVARCHAR(50)");
            sql.AppendLine(");");
            return sql.ToString();
        }

        public static string CreateVisitInfoTableMySQL()
        {
            var sql = new StringBuilder();
            sql.AppendLine("CREATE TABLE VisitInfo (");
            sql.AppendLine("    ID INT AUTO_INCREMENT PRIMARY KEY,");
            sql.AppendLine("    Account VARCHAR(100),");
            sql.AppendLine("    LatitudeE7 DOUBLE,");
            sql.AppendLine("    LongitudeE7 DOUBLE,");
            sql.AppendLine("    Address VARCHAR(255),");
            sql.AppendLine("    LocationConfidence DOUBLE,");
            sql.AppendLine("    DeviceTag VARCHAR(100),");
            sql.AppendLine("    StartTimestamp DATETIME,");
            sql.AppendLine("    EndTimestamp DATETIME,");
            sql.AppendLine("    PlaceConfidence VARCHAR(50),");
            sql.AppendLine("    PlaceVisitType VARCHAR(50)");
            sql.AppendLine(");");
            return sql.ToString();
        }

        public static string CreateVisitInfoTableSQLite()
        {
            var sql = new StringBuilder();
            sql.AppendLine("CREATE TABLE VisitInfo (");
            sql.AppendLine("    ID INTEGER PRIMARY KEY AUTOINCREMENT,");
            sql.AppendLine("    Account TEXT,");
            sql.AppendLine("    LatitudeE7 REAL,");
            sql.AppendLine("    LongitudeE7 REAL,");
            sql.AppendLine("    Address TEXT,");
            sql.AppendLine("    LocationConfidence REAL,");
            sql.AppendLine("    DeviceTag TEXT,");
            sql.AppendLine("    StartTimestamp TEXT,"); // SQLite does not have a dedicated DateTime type
            sql.AppendLine("    EndTimestamp TEXT,");   // Use TEXT to store ISO8601 strings ("YYYY-MM-DD HH:MM:SS.SSS")
            sql.AppendLine("    PlaceConfidence TEXT,");
            sql.AppendLine("    PlaceVisitType TEXT");
            sql.AppendLine(");");
            return sql.ToString();
        }

        public static string InsertVisitInfo(VisitInfo visitInfo)
        {
            var sql = new StringBuilder();
            sql.AppendLine("INSERT INTO VisitInfo (Account, LatitudeE7, LongitudeE7, Address, LocationConfidence, DeviceTag, StartTimestamp, EndTimestamp, PlaceConfidence, PlaceVisitType) VALUES (");
            sql.AppendFormat("    '{0}', {1}, {2}, '{3}', {4}, '{5}', {6}, {7}, '{8}', '{9}'",
                visitInfo.Account,
                visitInfo.LatitudeE7.HasValue ? visitInfo.LatitudeE7.ToString() : "NULL",
                visitInfo.LongitudeE7.HasValue ? visitInfo.LongitudeE7.ToString() : "NULL",
                visitInfo.Address,
                visitInfo.LocationConfidence.HasValue ? visitInfo.LocationConfidence.ToString() : "NULL",
                visitInfo.DeviceTag,
                visitInfo.StartTimestamp.HasValue ? $"'{visitInfo.StartTimestamp.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL",
                visitInfo.EndTimestamp.HasValue ? $"'{visitInfo.EndTimestamp.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL",
                visitInfo.PlaceConfidence,
                visitInfo.PlaceVisitType
            );
            sql.AppendLine(");");
            return sql.ToString();
        }
    }
}
