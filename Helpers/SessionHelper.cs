using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace LibraryManagementSystem.Helpers
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static int? GetUserId(this ISession session)
        {
            return session.GetInt32("UserId");
        }

        public static void SetUserId(this ISession session, int userId)
        {
            session.SetInt32("UserId", userId);
        }

        public static string? GetUserRole(this ISession session)
        {
            return session.GetString("UserRole");
        }

        public static void SetUserRole(this ISession session, string role)
        {
            session.SetString("UserRole", role);
        }

        public static string? GetUserName(this ISession session)
        {
            return session.GetString("UserName");
        }

        public static void SetUserName(this ISession session, string userName)
        {
            session.SetString("UserName", userName);
        }

        public static bool IsAuthenticated(this ISession session)
        {
            return session.GetInt32("UserId").HasValue;
        }

        public static void ClearSession(this ISession session)
        {
            session.Clear();
        }
    }
}
