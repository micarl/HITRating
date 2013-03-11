using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HitRating.Models
{

    #region 模型
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "新密码和确认密码不匹配。")]
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("当前密码")]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("新密码")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("确认新密码")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessage="必填")]
        [DisplayName("用户名/Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "必填")]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("记住我")]
        public bool RememberMe { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "密码和确认密码不匹配。")]
    public class RegisterModel
    {
        [Required(ErrorMessage = "必填")]
        [DisplayName("用户名")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "必填")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "必填")]
        [DataType(DataType.Password)]
        [DisplayName("确认密码")]
        public string ConfirmPassword { get; set; }
    }
    #endregion

    #region Services
    // FormsAuthentication 类型是密封的且包含静态成员，因此很难对
    // 调用其成员的代码进行单元测试。下面的接口和 Helper 类演示
    // 如何围绕这种类型创建一个抽象包装，以便可以对 AccountController
    // 代码进行单元测试。

    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        string GetUserNameByEmail(string email); 
        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        MembershipUser GetAccountByUserName(string userName);
        string GetEmail(string userName);
        void SetEmail(string userName, string newEmail);
        bool Exist(string userName);
        string SetValidationCode(string userName);
        void ResetPassword(string validationCode, string newPassword);
        void SetPhotoUrl(string userName, string photoUrl);
        string GetPhotoUrl(string userName);
        bool DeleteUser(string userName);
    }

    public class AccountMembershipService : IMembershipService
    {
        public MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public void SetPhotoUrl(string userName, string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                throw new ArgumentNullException("头像图片地址为空");
            }

            try
            {
                var en = new Models.Entities();
                aspnet_Users u = en.aspnet_Users.First(m => m.UserName == userName);

                u.PhotoUrl = photoUrl;
                en.ApplyCurrentValues("aspnet_Users", u);
                en.SaveChanges();
            }
            catch
            {
                throw new ArgumentException("用户名不存在");
            }
        }

        public string GetPhotoUrl(string userName)
        {
            try
            {
                return (new Models.Entities()).aspnet_Users.First(m => m.UserName == userName).PhotoUrl;
            }
            catch
            {
                throw new ArgumentException("用户名不存在");
            }
        }

        public string GetUserNameByEmail(string email) 
        {
            return _provider.GetUserNameByEmail(email);
        }

        public string GetEmail(string userName)
        {
            try
            {
                var en = new Models.Entities();
                aspnet_Users u = null;
                aspnet_Membership member = null;
                try
                {
                    u = en.aspnet_Users.First(m => m.UserName == userName);
                    member = en.aspnet_Membership.First(m => m.UserId == u.UserId);

                    return member.Email;
                }
                catch
                {
                    throw new ArgumentException("用户名不存在");
                }
            }
            catch
            {
                throw;
            }
        }

        public void SetEmail(string userName, string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
            {
                throw new ArgumentException("新的Email为空");
            }

            if (GetUserNameByEmail(newEmail) != null)
            {
                throw new ArgumentException("Email已经被注册");
            }

            try
            {
                var en = new Models.Entities();
                aspnet_Users u = null;
                aspnet_Membership member = null;
                try
                {
                    u = en.aspnet_Users.First(m => m.UserName == userName);
                    member = en.aspnet_Membership.First(m => m.UserId == u.UserId);
                }
                catch
                {
                    throw new ArgumentException("用户名不存在");
                }

                u.Email = newEmail;
                member.Email = newEmail;
                member.LoweredEmail = newEmail.ToLower();

                en.ApplyCurrentValues("aspnet_Users", u);
                en.ApplyCurrentValues("aspnet_Membership", member);
                en.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("必填", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("必填", "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("必填", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("必填", "password");

            //check email 不重复
            if (GetAccountByUserName(userName) != null)
            {
                throw new ArgumentException("用户名: "+ userName +" 已被注册");
            }

            //check email 不重复
            if (!String.IsNullOrEmpty(email) && GetUserNameByEmail(email) != null) {
                throw new ArgumentException("Email已被注册");
            }

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);

            SetPhotoUrl(userName, Models.ApplicationConfig.DefaultAccountPhotoUrl);

            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("必填", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("必填", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("必填", "newPassword");

            // 在某些出错情况下，基础 ChangePassword() 将引发异常，
            // 而不是返回 false。
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, false);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public MembershipUser GetAccountByUserName(string userName)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException();
            }

            return _provider.GetUser(userName, false);
        }

        public bool Exist(string userName) 
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("用户名为空");
            }

            if (null == GetAccountByUserName(userName))
            {
                return false;
            }
            else
            { 
                return true;
            }
        }

        public string SetValidationCode(string userName) 
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("用户名为空");
            }

            try
            {
                var en = new Models.Entities();
                aspnet_Users u = null;
                try
                {
                    u = en.aspnet_Users.First(m => m.UserName == userName);
                }
                catch
                {
                    throw new ArgumentException("用户名不存在");
                }

                // set a 20-lengthed validation string (code), it's only validate in 24 hours
                u.ValidationCode = (new Utilities.RandString()).Generate();
                u.validUntil = DateTime.Now.AddDays(1);

                en.ApplyCurrentValues("aspnet_Users", u);
                en.SaveChanges();

                return u.ValidationCode;
            }
            catch
            {
                throw;
            }
        }

        public void ResetPassword(string validationCode, string newPassword) 
        {
            if (string.IsNullOrEmpty(validationCode))
            {
                throw new ArgumentNullException("验证码为空");
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException("新密码不能为空");
            }

            if (newPassword.Length < 6)
            {
                throw new ArgumentNullException("新密码不能少于6个字符");
            }

            try
            {
                var en = new Models.Entities();
                aspnet_Users u = null;
                try
                {
                    u = en.aspnet_Users.First(m => m.ValidationCode == validationCode);
                }
                catch
                {
                    throw new ArgumentException("用户名不存在");
                }

                if (((DateTime)u.validUntil).CompareTo(DateTime.Now) < 0) 
                {
                    throw new ArgumentException("验证码过期");
                }

                aspnet_Membership member = en.aspnet_Membership.First(m => m.UserId == u.UserId);

                var userName = u.UserName;
                var email = member.Email;

                var cd = member.CreateDate;
                var lld = member.LastLoginDate;
                var unc = u.UserNameChanged;

                // delete old user
                en.DeleteObject(member);
                en.DeleteObject(u);

                en.SaveChanges();

                // recreate the user
                CreateUser(userName, newPassword, email);

                // copy the old user's attrs to the new one
                try
                {
                    var newU = en.aspnet_Users.First(m => m.UserName == userName);
                    var newMember = en.aspnet_Membership.First(m => m.UserId == newU.UserId);

                    newU.UserNameChanged = unc;
                    newMember.CreateDate = cd;
                    newMember.LastLoginDate = lld;

                    en.ApplyCurrentValues("aspnet_Users", newU);
                    en.ApplyCurrentValues("aspnet_Membership", newMember);

                    en.SaveChanges();
                }
                catch
                {
                    throw new ArgumentException("用户名不存在");
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        public string[] Search(string userName = null, int start = 0, int count = 0)
        {
            
            try
            {
                var en = new Models.Entities();
                var us = string.IsNullOrEmpty(userName) ? en.aspnet_Users : en.aspnet_Users.Where(m => m.UserName.Contains(userName));

                if (start > 0)
                {
                    us = us.Skip(start);
                }

                if (count > 0)
                {
                    us = us.Take(count);
                }

                var us2 = new string[us.Count()];
                int i = 0;
                foreach (var u in us)
                {
                    us2[i] = u.UserName;
                    i++;
                }

                return us2;
            }
            catch
            {
                throw new ArgumentException("用户名不存在");
            }
        }

        public bool DeleteUser(string userName)
        {
            try
            {
                var en = new Models.Entities();
                aspnet_Users u = null;
                aspnet_Membership member = null;
                try
                {
                    u = en.aspnet_Users.First(m => m.UserName == userName);
                    member = en.aspnet_Membership.First(m => m.UserId == u.UserId);

                    en.DeleteObject(member);
                    en.DeleteObject(u);

                    en.SaveChanges();

                    return true;
                }
                catch
                {
                    throw new ArgumentException("注销失败");
                }
            }
            catch
            {
                throw;
            }
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("必填", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请另输入一个用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "已存在与该Email对应的用户名。请另输入一个Email。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' 和 '{1}' 不相同。";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' 必须至少包含 {1} 个字符。";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion

}
