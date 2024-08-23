namespace Domain.Utils
{
    public static class Useful
    {
        public const int TOKEN_JWT_EXPIRES_IN_30_MIN = 30;
        public const int REDIS_DEFAULT_EXPIRES_IN_ONE_HOUR = 3600;
        public const int USERS_PER_PAGE = 5;
        public const string JWT_COOKIE_INDEX = "jwtToken";

        public class CustomClaimTypes
        {
            public const string Name = "name";
            public const string Permission = "permission";
            public const string NameIdentifier = "nameidentifier";

        }
        public class Claims
        {
            public const string SYS_ADMIN = "sys_admin";
            public const string SYS_USER = "sys_user";
        }

        public class Policies
        {
            public const string ADMIN_ONLY = "AdminOnly";
            public const string USER_OR_ADMIN = "UserOrAdmin";
        }

        public enum Resources
        {
            Users,
            Addresses
        }
    }
}
