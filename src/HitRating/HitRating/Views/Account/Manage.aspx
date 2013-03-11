<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	账户管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <style type="text/css">
            #account 
            {
                width: 100%;
                max-width: 400px;
                margin: 0 auto; 
                   
            }
        </style>
        <div id="account">
            <script type="text/javascript">
                var userName = "<%: Page.User.Identity.Name %>";
            </script>

            <p class="green_background">账户管理</p>

            <script type="text/javascript">
                $(function () {
                    $("#edit_account_email input:submit").click(function () {
                        $.ajax({
                            type: "PUT",
                            url: ROOT + "Api/Account/" + userName + "/Email",
                            data: { Email: $("#edit_account_email input[name='email']").val() },
                            dataType: "json",
                            success: function (data) {
                                $.miniSuccessAjaxResult("修改Email成功");
                                $("#edit_account_email input[name='email']").val("");
                            },
                            error: function () {
                                $.miniErrorAjaxResult("修改Email失败");
                            }
                        });

                        return false;
                    })
                })
            </script>
            <div id="edit_account_email" class="line">
                <p class="caption">新邮箱</p>
                <input name="email" />
                <div class="buttons one_click_buttons line right">
                    <input type="submit" value="保存" />
                </div>
            </div>

            <script type="text/javascript">
                $(function () {
                    $("#edit_account_photo input:submit").click(function () {
                        $.ajax({
                            type: "PUT",
                            url: ROOT + "Api/Account/" + userName + "/Photo",
                            data: { Photo: $("#edit_account_photo input[name='photo']").val() },
                            dataType: "json",
                            success: function (data) {
                                $.miniSuccessAjaxResult("修改头像成功");
                                $("#edit_account_photo input[name='photo']").val("");
                            },
                            error: function () {
                                $.miniErrorAjaxResult("修改头像失败");
                            }
                        });

                        return false;
                    })
                })
            </script>
            <div id="edit_account_photo"  class="line">
                <p class="caption">新头像</p>
                <input name="photo" />
                <div class="buttons one_click_buttons line right">
                    <input type="submit" value="保存" />
                </div>
           </div>

           <script type="text/javascript">
               $(function () {
                   $("#change_account_password input:submit").click(function () {
                       $.ajax({
                           type: "PUT",
                           url: ROOT + "Api/Account/" + userName + "/Password",
                           data: { OldPassword: $("#change_account_password input[name='OldPassword']").val(), NewPassword: $("#change_account_password input[name='NewPassword']").val(), PasswordConfirm: $("#change_account_password input[name='PasswordConfirm']").val() },
                           dataType: "json",
                           success: function (data) {
                               $.miniSuccessAjaxResult("修改密码成功");
                               $("#change_account_password input:password").val("");
                           },
                           error: function () {
                               $.miniErrorAjaxResult("修改密码失败");
                               $("#change_account_password input:password").val("");
                           }
                       });

                       return false;
                   })
               })
            </script>
            <div id="change_account_password"  class="line">
                <p class="caption">修改密码</p>
                <div class="line">
                    <label>旧密码</label>
                    <input type="password" name="OldPassword" />
                </div>
                <div class="line">
                    <label>新密码</label>
                    <input type="password" name="NewPassword" />
                </div>
                <div class="line">
                    <label>新密码确认</label>
                    <input type="password" name="PasswordConfirm" />
                </div>
                <div class="buttons one_click_buttons line right">
                    <input type="submit" value="修改" />
                </div>
           </div>

           <div class="big_buttons line">
                <a class="taction" object="Session" taction_type="4" title="退出" redirect_to="">退出</a>
           </div>
       </div>
</asp:Content>
