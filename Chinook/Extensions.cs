using System.Data.SqlClient;

namespace Chinook
{
    public static class Extensions
    {
        public static string GetSafeString(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
    }
}
