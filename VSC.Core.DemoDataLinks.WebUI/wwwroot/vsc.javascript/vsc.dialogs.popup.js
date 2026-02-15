// vsc.dialogs.popup jQuery Plugin
// 11/03/2025 Bootstrap 5.3 compatible

//this plugin is activated by HTML vsc-* attribute (declarative)
//depends on VSC.Path lib and requires Bootstrap 5.3 

; (function ($) //ensure exclusive use of $ for jQuery
{

    console.log("VSC JavaScript Library vsc.dialogs.popup.js");

    //VSC JavaScript Namespace Setup
    if (typeof VSC === "undefined")
    {
        console.log(">> Creating VSC JavaScript namespace");
        VSC = {};
    }

    if (typeof VSC.Dialogs === "undefined")
    {
        console.log(">> Creating VSC.Dialogs namespace");
        VSC.Dialogs = {};
    }

    //replace any existing AjaxPopup
    console.log(">> Creating VSC.Dialogs.AjaxPopup namespace");
    VSC.Dialogs.AjaxPopup = {};

    //prerequisite - VSC.Path must be loaded (this means VSC.AppPath will have been read)
    if (typeof VSC.Path === "undefined")
    {
        throw "Developer Warning: VSC.Dialogs.AjaxPopup requires VSC.Path";
    }
    else
    {
        console.log("  Dependency - VSC.Path is loaded OK");
        console.log("  VSC.AppPath = " + VSC.AppPath);
    }

    (function ()
    {
        this.StartAjaxPopups = function ()
        {
            console.log("VSC.Dialogs.AjaxPopup is starting...");
            if ($("a[vsc-dialog-popup-action]").length > 0)
            {
                console.log($("a[vsc-dialog-popup-action]").length + " - ajax popup(s) found ");
                //adds the dialog divs if they do not already exist
                if ($('#vsc-popup-dialog').length == 0)
                {
                    console.log("  - adding #vsc-popup-dialog ");
                    var outerDiv = $("<div>").attr("id", "vsc-popup-dialog")
                        .attr("tabindex", "-1")
                        .attr("aria-labelledby", "confirmmodallabel")
                        .addClass("modal").addClass("fade").prependTo($(document.body));
                    var documentDiv = $("<div>").addClass("modal-dialog modal-dialog-centered").attr("role", "document").appendTo($(outerDiv));
                    var contentDiv = $("<div>").attr("id", "vsc-popup-dialog-content").addClass("modal-content").appendTo($(documentDiv));


                }
            }
            else
            {
                console.log("  - no ajax popups present.");
            }

            //----------------------SHOW POPUP DIALOG----------------------------//
            $("a[vsc-dialog-popup-action]").on('click', function (e)
            {
                console.log("--> VSC.Dialogs.AjaxPopup click");
                $(".context-menu").hide();

                var contextTable = $(e.currentTarget).data("GetKeyFromTable"); //no #
                var id = 0;
                if (contextTable == undefined)
                {
                    //get id from attribute of link
                    id = $(e.currentTarget).attr("data-key");
                }
                else
                {
                    id = $("#" + contextTable).data("key");
                }

                var path = $(e.currentTarget).attr("vsc-dialog-popup-action");


                console.log("  vsc-dialog-popup-action = " + path);
                console.log("  VSC.AppPath = " + VSC.AppPath);

                //check if path already has VSC.AppPath at start
                var url = path;
                if (path.length > VSC.AppPath.length)
                {
                    if (path.toLowerCase().substring(0, VSC.AppPath.length) !== VSC.AppPath.toLowerCase())
                    {
                        var url = VSC.AppPath + path;
                    }
                    else
                    {
                        console.log("  - action path is Ok for MVC");
                    }
                }

                //check for trailing "/"
                if (url != undefined && url.length > 0)
                {
                    if (url.substring(url.length - 1, url.length) == "/")
                    {
                        url = url.substring(0, url.length - 1);
                    }
                }
                console.log("  url = " + url);

                $.get(url, function (data)
                {
                    $("#vsc-popup-dialog-content").html(data);
                    //re-validate any forms
                    if ($.validator != undefined) $.validator.unobtrusive.parse('form');
                    //if the page contains any create start up functions - run them
                    if (typeof window["RunOnPopupDialogActivate"] == "function")
                    {
                        window["RunOnPopupDialogActivate"]();
                    }
                    //VSC.Dialogs.AjaxPopup.RegisterDialogClose();
                    $("#vsc-popup-dialog").modal("show");
                });


                return false;
            });
        };
    }).apply(VSC.Dialogs.AjaxPopup);

    //all this removed 12/03/2025 the dialog uses the Bootstrap Modal dialog now and
    //                   the dialog contents (view) must have a button with the data-bs-dismiss attribute
    //(function ()
    //{
    //    this.RegisterDialogClose = function ()
    //    {
    //        $(".dialog button.close, .dialog a.btn-cancel").on('click',function (e)
    //        {
    //            $("div.dialog").slideUp(function () { $('#editdialog').empty(); });
    //        });
    //    };
    //}).apply(VSC.Dialogs.AjaxPopup);

    //run this now
    VSC.Dialogs.AjaxPopup.StartAjaxPopups();
    console.log("Done.");
    console.log("");

})(jQuery);