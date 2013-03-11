//general configurations
var ROOT = "/";

//general functions
function htmlEscape(str) {
    return String(str)
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&lsquo;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/&/g, '&amp;');
}

function htmlUnEscape(str) {
    return String(str)
            .replace(/&quot;/g, '"')
            .replace(/&lsquo;/g, "'")
            .replace(/&lt;/g, '<')
            .replace(/&gt;/g, '>')
            .replace(/&amp;/g, '&');
}

function xmlEscape(str) {
    return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&lsquo;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
}

function xmlUnEscape(str) {
    return String(str)
            .replace(/&amp;/g, '&')
            .replace(/&quot;/g, '"')
            .replace(/&lsquo;/g, "'")
            .replace(/&lt;/g, '<')
            .replace(/&gt;/g, '>');
}

function encodeUri(uri) {
    return uri.replace(/#/g, '%23').replace(/&amp;/g, '&').replace(/&/g, '%26').replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
}

function decodeUri(uri) {
    return uri.replace(/&amp;/g, '&').replace(/#/g, '%23');
}

$.print = function (article) {
    if (article == null) {
        return false;
    }

    if ($("body .print_container").length) {
        $("body .print_container").empty();
    }
    else {
        $("body").append("<div class='print_container'></div>")
    }

    $("body .print_container").html($(article).html());
    window.print();
}

$.centerPopup = function (popup) {
    $("#popup_cover").show();

    $(".popup:visible").hide("normal");
    $("#corner_popup_icon").hide("normal", function () {
        $(popup).show("normal");
    })
}

$.fadePopup = function (popup, fn) {
    $("#popup_cover").hide();

    $(popup).hide("normal", function () {
        $("#corner_popup_icon").show("normal", fn);
    });
}

$.centerFlashPopup = function (popup) {
    $(popup).fadeIn('normal', function () {
        $(this).animate({ "opacity": 1 }, "slow").fadeOut(3000);
    });
}

$.disableButtons = function (buttons) {
    $(buttons).find("a").addClass("disabled");
    $(buttons).find("input").addClass("disabled");
    $(buttons).append("<div class='cover'>&nbsp;</div>");
}

$.enableButtons = function (buttons) {
    $(buttons).find("a").removeClass("disabled");
    $(buttons).find("input").removeClass("disabled");
    $(buttons).find(".cover").remove();
}

$(function () {
    //list table 
    $(".list_table tr:even").addClass("even");

    $(".list_table tr").mouseover(function () {
        $(this).children("td").addClass("over");
    }).mouseout(function () {
        $(this).children("td").removeClass("over");
    });

    //module
    $('.module .tabs .tab').live('click', function () {
        var show = $(this).attr('show');

        if (show && ($(this).parents('.tabs').siblings().children('.page:visible').attr('page') != show)) {
            $(this).addClass('selected').siblings().removeClass('selected');
            $(this).parents('.tabs').siblings().children('.page:visible').hide();
            $(this).parents('.tabs').siblings().children('.page[page=' + show + ']').show();
        }
    });

    var moduletabGroups = $(".module .tabs");
    if (moduletabGroups.length > 0) {
        for (var i = 0; i < moduletabGroups.length; i++) {
            var tabs = moduletabGroups.eq(i).children(".tab");
            if (tabs.length > 0) {
                tabs.eq(0).click();

            }
        }
    }

    $('.popup .close').live('click', function () {
        $.fadePopup($(this).parents(".popup"));
    });

    $(".popup .cancel").live("click", function () {
        $(this).parents(".popup").children(".close").click();
    });

    //one click buttons
    $(".one_click_buttons a").live("click", function () {
        $.disableButtons($(this).parent());
    });

    $(".one_click_buttons input:button").live("click", function () {
        $.disableButtons($(this).parent());
    });

    $(".one_click_buttons input:submit").live("click", function () {
        $.disableButtons($(this).parent());
    });

    //mini_search
    $("input.mini_search.tip_search").live("click", function () {
        $(this).val("").removeClass("tip_search");
    })

    $("input.mini_search").live("keyup", function (e) {
        if (e.keyCode == 13) {
            window.location.href = encodeURI(ROOT + $(this).attr("action")) + encodeURIComponent($(this).val());
        }
    })

    //tip_input
    $("input.tip_input").live("focus", function () {
        $(this).removeClass("tip_input").val("");
    })

    //toggle read
    $(".toggle_read .toggle").live("click", function () {
        var toggleContents = $(this).parents(".toggle_read").find(".toggle_content").toggleClass("hidden");

        if ($(this).hasClass("one_click_toggle")) {
            $(this).hide();
        }
    })

    //ajax search suggestions
    var selectedItem = -1;
    $('.ajax_search .ajax_input_title').live('keyup', function (event) {
        switch (event.keyCode) {
            case 13: //选中某个选项
                if (selectedItem >= 0) {
                    $(this).siblings('.ajax_suggestions').find('.option:eq(' + selectedItem + ')').mousedown();
                }
                $(this).siblings('.ajax_suggestions').empty();
                selectedItem = -1;
                break;
            case 38: //
                var ajaxSelectors = $(this).siblings('.ajax_suggestions').find(".options");
                var matches = $(ajaxSelectors).children('.option').length;
                selectedItem--;
                if (selectedItem < 0) {
                    selectedItem = -1;
                }

                $(ajaxSelectors).children('.option').removeClass('selected');
                if (selectedItem >= 0) {
                    $(ajaxSelectors).children('.option:eq(' + selectedItem + ')').addClass('selected');
                }

                break;
            case 40:
                var ajaxSelectors = $(this).siblings('.ajax_suggestions').find(".options");
                var matches = $(ajaxSelectors).children('.option').length;

                selectedItem++;
                if (selectedItem >= matches) {
                    selectedItem = matches - 1;
                }

                $(ajaxSelectors).children('.option').removeClass('selected');
                $(ajaxSelectors).children('.option:eq(' + selectedItem + ')').addClass('selected');

                break;
            case 37:
                break;
            case 39:
                break;
            default:
                var thisInput = this;
                $.ajax(
                        {
                            url: encodeURI($(this).parents('.ajax_search').attr('url') + $(this).val()),
                            dataType: "json",
                            success: function (data) {
                                var opts = "";
                                for (var i = 0; i < data.Entities.length; i++) {
                                    opts += "<div class='option' sid='" + data.Entities[i].Id + "'>" + data.Entities[i].Title + "</div>";
                                }

                                if (opts.length) {
                                    opts = "<div class='options'>" + opts + "</div>"
                                }

                                $(thisInput).siblings('.ajax_suggestions').html(opts);

                            },
                            error: function () {
                                $(thisInput).siblings('.ajax_suggestions').empty();
                            }
                        }
                    );

                selectedItem = -1;
                $(this).siblings('.ajax_input_sid').val("");
        }
    }).live('focus', function () {
        var thisInput = this;
        $.ajax(
            {
                url: encodeURI($(this).parents('.ajax_search').attr('url') + $(this).val()),
                dataType: "json",
                success: function (data) {
                    var opts = "";
                    for (var i = 0; i < data.Entities.length; i++) {
                        opts += "<div class='option' sid='" + data.Entities[i].Id + "'>" + data.Entities[i].Title + "</div>";
                    }

                    if (opts.length) {
                        opts = "<div class='options'>" + opts + "</div>"
                    }
                    $(thisInput).siblings('.ajax_suggestions').html(opts);
                },
                error: function () {
                    $(thisInput).siblings('.ajax_suggestions').empty();
                }
            }
        );

        selectedItem = -1;
        $(this).siblings('.ajax_input_sid').val("");
    }).live('blur', function () {
        $(this).siblings('.ajax_suggestions').empty();
    });

    $('.ajax_suggestions .option').live('mousedown', function () {
        $(this).parents('.ajax_suggestions').siblings('.ajax_input_title').val($(this).text());
        $(this).parents('.ajax_suggestions').siblings('.ajax_input_sid').val($(this).attr('sid'));
    }).live("mouseover", function () {
        $(this).siblings().removeClass('selected');
        $(this).addClass('selected');
    });

    $(".printable .print").live("click", function () {
        $.print($(this).parents(".printable"));
    })
})