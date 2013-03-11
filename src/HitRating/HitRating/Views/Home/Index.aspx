<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    主页
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        #main 
        {
            background: none;
            box-shadow: none;
        }
        #logon_and_register
        {
            width: 420px;
            margin: 0 auto;
            padding: 1em;
            border-radius: 5px;
            box-shadow: 0px 0px 20px #666; 
        }
        #logon_and_register .module 
        {
            border: 0; 
            border-top: 1px solid #ccc;
        }
        #slogan 
        {
            padding: 1em 0;
            background: #fff;
            height: 97px;
            text-align: center;
        }
        #main_content
        {
            background: none;    
        }
    </style>
    <script type="text/javascript">
        $(function () {
        
        })
    </script>
    <div id="logon_and_register">
        <div id="slogan">
            <img src="<%: Html.ParseImageUrl("Images/logo.png") %>" alt="" />
        </div>
        <div class="module">
            <div class="tabs hidden">
                <script type="text/javascript">
                    $(function () {
                        $("#go_register").click(function () {
                            $(this).parents(".module").find(".tab[show='2']").click();
                        })

                        $("#go_logon").click(function () {
                            $(this).parents(".module").find(".tab[show='1']").click();
                        })
                    })
                </script>
                <span class="tab" show="1">登录</span>
                <span class="tab" show="2">注册</span>
            </div>
            <div class="pages">
                <div id="log_on" class="page" page="1">
                    <script type="text/javascript">
                        $(function () {
                            $("#log_on input[name='Password']").keyup(function (e) {
                                if (e.keyCode == 13) {
                                    $("#log_on input:submit").click();
                                }
                            });

                            $("#log_on input:submit").click(function () {
                                $.ajax({
                                    type: "POST",
                                    url: ROOT + "Api/Account/" + $("#log_on input[name='UserName']").val() + "/Session",
                                    data: { Password: $("#log_on input[name='Password']").val(), RememberMe: $("#log_on input[name='RememberMe']").is(":checked") },
                                    dataType: "json",
                                    success: function () {
                                        window.location.href = ROOT + "Account/Home";
                                    },
                                    error: function (jqXHR) {
                                        if ($("#log_on .validation_error").length < 1) {
                                            $("#log_on").prepend("<div class='validation_error line'>用户名或密码错误</div>");
                                        }

                                        $.enableButtons($("#log_on .one_click_buttons"));
                                    }
                                });
                            });
                        })
                    </script>
                    <div class="line">
                        <label>用户</label>
                        <input name="UserName" />
                    </div>
                    <div class="line">
                        <label>密码</label>
                        <input type="password" name="Password" />
                    </div>
                    <div class="line">
                        <input type="checkbox" name="RememberMe" checked="checked" />
                        <label>记住我</label>
                    </div>
                    <div class="line" style="position: absolute;">
                    </div>
                    <div class="line big_buttons one_click_buttons">
                        <input type="submit" value="登录" />
                    </div>
                    <div class="line">
                        <a href="#" id="go_register">免费注册!</a>
                    </div>
                </div>

                <div id="register" class="page" page="2">
                    <script type="text/javascript">
                        $(function () {
                            $("#register input:submit").click(function () {
                                $("#register .validation_error").remove();

                                var flag = true;
                                if ($("#register input[name='UserName']").val().length < 1) {
                                    if ($("#register .validation_error").length < 1) {
                                        $("#register").prepend("<div class='validation_error line'>请填写用户名</div>");
                                    }
                                    else {
                                        $("#register .validation_error").text("请填写用户名");
                                    }

                                    flag = false;
                                }

                                if ($("#register input[name='Password']").val().length < 6) {
                                    if ($("#register .validation_error").length < 1) {
                                        $("#register").prepend("<div class='validation_error line'>密码不能少于6个字符</div>");
                                    }
                                }

                                if ($("#register input[name='Password']").val() != $("#register input[name='PasswordConfirm']").val()) {
                                    if ($("#register .validation_error").length < 1) {
                                        $("#register").prepend("<div class='validation_error line'>两次密码输入不一致</div>");
                                    }

                                    flag = false;
                                }

                                if (flag) {
                                    $.disableButtons($("#register .big_buttons"));

                                    $.ajax({
                                        type: "POST",
                                        url: ROOT + "Api/Accounts",
                                        data: { UserName: $("#register input[name='UserName']").val(), Email: $("#register input[name='Email']").val(), Password: $("#register input[name='Password']").val() },
                                        dateType: "json",
                                        success: function (data) {
                                            window.location.href = ROOT + "Account/Home";
                                        },
                                        error: function () {
                                            if ($("#register .validation_error").length < 1) {
                                                $("#register").prepend("<div class='validation_error line'>注册失败，请重试</div>");
                                            }

                                            $.enableButtons($("#register .big_buttons"));
                                        }
                                    })
                                }
                            })
                        })
                    </script>
                    <div class="line">
                        <label>用户</label>
                        <span class="green">*</span>
                        <input name="UserName" />
                    </div>
                    <div class="line">
                        <label>邮箱</label>
                        <input name="Email" />
                    </div>
                    <div class="line">
                        <label>密码</label>
                        <span class="green">*</span>
                        <input type="password" name="Password" />
                    </div>
                    <div class="line">
                        <label>密码确认</label>
                        <span class="green">*</span>
                        <input type="password" name="PasswordConfirm" />
                    </div>
                    <div class="line big_buttons">
                        <input type="submit" value="注册" />
                    </div>
                    <div class="line">
                        <a href="#" id="go_logon">返回登录</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
