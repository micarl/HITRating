<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
        <div id="user_control_panels">
            <script type="text/javascript">
                //configurations
                var PanelApiRoot = "/";
                var iam = "<%: Page.User.Identity.Name %>";
            </script>
            <script type="text/javascript">
                $(".taction[object='Session'][taction_type='1']").live("click", function () {
                    $("#account_log_on").attr("redirect_to", $(this).attr("redirect_to"));

                    $.centerPopup($("#account_log_on"));
                });

                $(".taction[object='Session'][taction_type='4']").live("click", function () {
                    var redirectTo = $(this).attr("redirect_to");

                    $.ajax({
                        type: "DELETE",
                        url: PanelApiRoot + "Api/Account/" + "<%: Page.User.Identity.Name %>" + "/Session",
                        dataType: "json",
                        success: function () {
                            window.location.href = redirectTo;
                        },
                        error: function (jqXHR) {
                            $.miniErrorAjaxResult("钥匙拿错了!? 请重试");
                        }
                    });

                    return false;
                });

                $(".taction[object='Account'][taction_type='1']").live("click", function () {
                    $("#account_register").attr("redirect_to", $(this).attr("redirect_to")).attr("no_redirect", $(this).attr("no_redirect"));

                    $.centerPopup($("#account_register"));
                });
            </script>
            <div id="account_log_on" class="module popup" style="width:350px;">
                <script type="text/javascript">
                    $(function () {
                        $("#account_log_on input[name='Password']").keyup(function (e) {
                            if (e.keyCode == 13) {
                                $("#account_log_on input:submit").click();
                            }
                        });

                        $("#account_log_on input:submit").click(function () {
                            var redirectTo = $(this).parents("#account_log_on").attr("redirect_to");
                            var noRedirect = $(this).parents("#account_log_on").attr("no_redirect") == "true" ? true : false;

                            $.ajax({
                                type: "POST",
                                url: PanelApiRoot + "Api/Account/" + $("#account_log_on input[name='UserName']").val() + "/Session",
                                data: { Password: $("#account_log_on input[name='Password']").val(), RememberMe: $("#account_log_on input[name='RememberMe']").is(":checked") },
                                dataType: "json",
                                success: function () {
                                    if (!noRedirect) {
                                        if (redirectTo == null) {
                                            location.reload();
                                        }
                                        else {
                                            window.location.href = redirectTo;
                                        }
                                    }
                                    else {
                                        $.miniSuccessAjaxResult("登录成功");
                                        $.fadePopup($("#account_log_on"));
                                    }
                                },
                                error: function (jqXHR) {
                                    if ($("#account_log_on .validation_error").length < 1) {
                                        $("#account_log_on").prepend("<div class='validation_error line'>用户名或密码错误</div>");
                                    }

                                    $.enableButtons($("#account_log_on .buttons"));
                                }
                            });
                        });
                    })
                </script>
                <h3 class="caption">登录</h3>
                <a class="close">x</a>
                <div class="pages">
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
                </div>
                <div class="bottom left">
                    <div class="buttons one_click_buttons">
                        <input type="submit" value="登录" />
                    </div>
                </div>
            </div>

            <div id="account_register" class="module popup" style="width:350px;">
                <script type="text/javascript">
                    $(function () {
                        $("#account_register input:submit").click(function () {
                            var redirectTo = $(this).parents("#account_register").attr("redirect_to");

                            $("#account_register .validation_error").remove();

                            var flag = true;
                            if ($("#account_register input[name='UserName']").val().length < 1) {
                                if ($("#account_register .validation_error").length < 1) {
                                    $("#account_register").prepend("<div class='validation_error line'>请填写用户名</div>");
                                }
                                else {
                                    $("#account_register .validation_error").text("请填写用户名");
                                }

                                flag = false;
                            }

                            if ($("#account_register input[name='Password']").val().length < 6) {
                                if ($("#account_register .validation_error").length < 1) {
                                    $("#account_register").prepend("<div class='validation_error line'>密码不能少于6个字符</div>");
                                }
                            }

                            if ($("#account_register input[name='Password']").val() != $("#account_register input[name='PasswordConfirm']").val()) {
                                if ($("#account_register .validation_error").length < 1) {
                                    $("#account_register").prepend("<div class='validation_error line'>两次密码输入不一致</div>");
                                }

                                flag = false;
                            }

                            if (flag) {
                                $.disableButtons($("#account_register .buttons"));

                                $.ajax({
                                    type: "POST",
                                    url: PanelApiRoot + "Api/Accounts",
                                    data: { UserName: $("#account_register input[name='UserName']").val(), Email: $("#account_register input[name='Email']").val(), Password: $("#account_register input[name='Password']").val() },
                                    dateType: "json",
                                    success: function (data) {
                                        if (redirectTo == null) {
                                            location.reload();
                                        }
                                        else {
                                            window.location.href = redirectTo;
                                        }
                                    },
                                    error: function () {
                                        if ($("#account_register .validation_error").length < 1) {
                                            $("#account_register").prepend("<div class='validation_error line'>注册失败，请重试</div>");
                                        }

                                        $.enableButtons($("#account_register .buttons"));
                                    }
                                })
                            }
                        })
                    })
                </script>
                <h3 class="caption">注册</h3>
                <a class="close">x</a>
                <div class="pages">
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
                </div>
                <div class="bottom buttons left line">
                    <input type="submit" value="注册" />
                </div>
            </div>
        </div>

        <script type="text/javascript">
            $.ajaxResult = function (caption, content, buttons) {
                $("#ajax_result .caption").html(caption);
                $("#ajax_result .pages").html(content);
                $("#ajax_result .bottom").html(buttons);
                $.centerPopup($("#ajax_result"));
            }
        </script>
        <div id="ajax_result" class="module popup">
            <h3 class="caption">ajax结果</h3>
            <a class="close">x</a>
            <div class="pages">
                <p class="gray center">加载中...</p>
            </div>
            <div class="bottom buttons right">
            </div>
        </div>

        <script type="text/javascript">
            $.miniSuccessAjaxResult = function (content) {
                $("#mini_success_ajax_result .pages").html(content);
                $.centerFlashPopup($("#mini_success_ajax_result"));
            }
        </script>
        <div id="mini_success_ajax_result" class="module popup flash">
            <div class="pages center" style="background: #3c6;color:#fff">
                <p class="gray center">加载中...</p>
            </div>
        </div>

        <script type="text/javascript">
            $.miniErrorAjaxResult = function (content) {
                $("#mini_error_ajax_result .pages").html(content);
                $.centerFlashPopup($("#mini_error_ajax_result"));
            }
        </script>
        <div id="mini_error_ajax_result" class="module popup flash">
            <div class="pages center" style="background: #f66;color:#fff">
                <p class="gray center">加载中...</p>
            </div>
        </div>

        <div id="popup_cover"></div>

        <div id="corner_control">
             <script type="text/javascript">
                 $.pushPopupPanel = function (panel, prepend) {
                     try {
                         if (prepend) {
                             $("#popup_menus .pages").prepend(panel);
                         }
                         else {
                             $("#popup_menus .pages").append(panel);
                         }
                     }
                     catch (e) {
                         return "im failed :<";
                     }
                 };

                 $(function () {
                     $("#corner_popup_icon").click(function () {
                         $.centerPopup($("#popup_menus"));
                     })
                 })
            </script>
            <a id="corner_popup_icon"></a>

            <div id="popup_menus" class="module popup">
                <h3 class="caption">@ur_serv</h3>
                <a class="close">x</a>
                <div class="pages">
                    
                </div>
                <div class="bottom buttons right">
                </div>
            </div>
        </div>

        <div id="tools">
            <script type="text/javascript">
                jQuery.fn.extend({
                    everyTime: function (interval, label, fn, times, belay) {
                        return this.each(function () {
                            jQuery.timer.add(this, interval, label, fn, times, belay);
                        });
                    },
                    oneTime: function (interval, label, fn) {
                        return this.each(function () {
                            jQuery.timer.add(this, interval, label, fn, 1);
                        });
                    },
                    stopTime: function (label, fn) {
                        return this.each(function () {
                            jQuery.timer.remove(this, label, fn);
                        });
                    }
                });

                jQuery.extend({
                    timer: {
                        guid: 1,
                        global: {},
                        regex: /^([0-9]+)\s*(.*s)?$/,
                        powers: {
                            // Yeah this is major overkill...
                            'ms': 1,
                            'cs': 10,
                            'ds': 100,
                            's': 1000,
                            'das': 10000,
                            'hs': 100000,
                            'ks': 1000000
                        },
                        timeParse: function (value) {
                            if (value == undefined || value == null)
                                return null;
                            var result = this.regex.exec(jQuery.trim(value.toString()));
                            if (result[2]) {
                                var num = parseInt(result[1], 10);
                                var mult = this.powers[result[2]] || 1;
                                return num * mult;
                            } else {
                                return value;
                            }
                        },
                        add: function (element, interval, label, fn, times, belay) {
                            var counter = 0;

                            if (jQuery.isFunction(label)) {
                                if (!times)
                                    times = fn;
                                fn = label;
                                label = interval;
                            }

                            interval = jQuery.timer.timeParse(interval);

                            if (typeof interval != 'number' || isNaN(interval) || interval <= 0)
                                return;

                            if (times && times.constructor != Number) {
                                belay = !!times;
                                times = 0;
                            }

                            times = times || 0;
                            belay = belay || false;

                            if (!element.$timers)
                                element.$timers = {};

                            if (!element.$timers[label])
                                element.$timers[label] = {};

                            fn.$timerID = fn.$timerID || this.guid++;

                            var handler = function () {
                                if (belay && this.inProgress)
                                    return;
                                this.inProgress = true;
                                if ((++counter > times && times !== 0) || fn.call(element, counter) === false)
                                    jQuery.timer.remove(element, label, fn);
                                this.inProgress = false;
                            };

                            handler.$timerID = fn.$timerID;

                            if (!element.$timers[label][fn.$timerID])
                                element.$timers[label][fn.$timerID] = window.setInterval(handler, interval);

                            if (!this.global[label])
                                this.global[label] = [];
                            this.global[label].push(element);

                        },
                        remove: function (element, label, fn) {
                            var timers = element.$timers, ret;

                            if (timers) {

                                if (!label) {
                                    for (label in timers)
                                        this.remove(element, label, fn);
                                } else if (timers[label]) {
                                    if (fn) {
                                        if (fn.$timerID) {
                                            window.clearInterval(timers[label][fn.$timerID]);
                                            delete timers[label][fn.$timerID];
                                        }
                                    } else {
                                        for (var fn in timers[label]) {
                                            window.clearInterval(timers[label][fn]);
                                            delete timers[label][fn];
                                        }
                                    }

                                    for (ret in timers[label]) break;
                                    if (!ret) {
                                        ret = null;
                                        delete timers[label];
                                    }
                                }

                                for (ret in timers) break;
                                if (!ret)
                                    element.$timers = null;
                            }
                        }
                    }
                });

                var NewComingCount = 0;

                $.enableTwordsAutoUpdate = function (api, interval) {
                    $.timer.add(this, interval, '', function () {
                        $.ajax({
                            type: "get",
                            url: api + "&IdUpper=" + $("#twords .container .tword_instant:first").attr("tword_id"),
                            dataType: "json",
                            success: function (data) {
                                var ts = data.Entities;

                                NewComingCount += ts.length;

                                for (var i = 0; i < ts.length; i++) {
                                    $("#twords .container").prepend($.renderTword(ts[ts.length - 1 - i]).hide());
                                }

                                $("#new_twords_alert .count").text(NewComingCount);
                                $("#new_twords_alert").fadeIn("normal");
                            }
                        });
                    }, 500, interval);
                }

                $(function () {
                    $("#new_twords_alert").live("click", function () {
                        $(this).fadeOut("normal");
                        $("#go_top").click();

                        $("#twords .container .tword_instant:hidden").fadeIn("normal");
                    })
                })
            </script>
            <a id="new_twords_alert" class="hidden">有 <span class="count"></span> 条新的Tword，请点击查收</a>
            <script type="text/javascript">
                $(function () {
                    $("#go_top").click(function () {
                        $('body').animate({ scrollTop: 0 }, "normal");
                        return false;
                    });
                })
            </script>
            <a id="go_top">返回<br />顶部</a>
        </div>