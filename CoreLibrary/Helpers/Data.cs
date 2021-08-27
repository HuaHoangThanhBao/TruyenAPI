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
        public static readonly int PassRequiredMinLength = 8;
        public static readonly int PassRequiredMaxLength = 30;
        public static readonly int MinLength = 0;
        public static readonly int NameRequiredMaxLength = 50;
        public static readonly int EmailRequiredMaxLength = 30;

        //Quyền của user
        public static readonly int UserRole = 0;
        public static readonly int EditorRole = 1;
        public static readonly int AdminRole = 2;
    }
}
