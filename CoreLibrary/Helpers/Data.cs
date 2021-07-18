using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Helpers
{
    public static class Data
    {
        //Identity
        public static readonly TimeSpan LockOutAccountTime = TimeSpan.FromDays(3);
        public static readonly int MaxFailedAccessAttempts = 3;
        public static readonly int PasswordRequiredLength = 8;
        public static readonly int PasswordRequiredMaxLength = 30;

        //Cookie
        public static readonly TimeSpan CookieExpireTime = TimeSpan.FromDays(7);
        public static readonly DateTime CookieOptionExpireTime = DateTime.Now.AddDays(7);
    }
}
