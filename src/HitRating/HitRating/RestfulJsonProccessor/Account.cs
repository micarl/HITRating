using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HitRating.RestfulJsonProccessor
{
    public static class Account
    {
        public static object List(IEnumerable<Models.aspnet_Users> data, string userName)
        {
            try
            {
                IList<object> ts = new List<object>();
                foreach (Models.aspnet_Users t in data)
                { 
                    ts.Add(Single(t, userName));
                }

                return ts;
            }
            catch
            {
                throw;
            }
        }

        public static object Single(Models.aspnet_Users data, string userName) 
        {
            return DataProcess(data, userName);
        }

        public static object MiniSingle(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName)) {
                    throw new ArgumentNullException();
                }

                return new
                {
                    UserName = userName,
                    PhotoUrl = (new Models.AccountMembershipService()).GetPhotoUrl(userName)
                };
            }
            catch
            {
                return null;
            }
        }

        private static object DataProcess(Models.aspnet_Users data, string userName)
        {
            try
            {
                return new
                {
                    UserName = data.UserName,
                    PhotoUrl = (new Models.AccountMembershipService()).GetPhotoUrl(data.UserName),
                    Options = AccountControlProcess(data.UserName, userName),
                    Created = data.aspnet_Membership.CreateDate.ToString(),
                    LastActivityDate = data.LastActivityDate.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public static IList<RestfulJsonProccessor.RestfulOption> AccountControlProcess(string followedUserName, string userName)
        {
            return null;
        }
    }
}