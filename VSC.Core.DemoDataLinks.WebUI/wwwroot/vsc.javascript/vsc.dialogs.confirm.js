//vsc.dialogs.confirm jQuery Plugin

//this plugin is activated by HTML vsc-* attribute (declarative)

//update: 02/02/2020 plugin converted to modal operation
//update: 09/03/2025 activated by vsc-* attributes,
//                   updated deprecated function click() to on(),
//                   dialog div is now called 'vsc-confirm-dialog'



//use:
// vsc-confirm-action - activates the dialog on an anchor
// vsc-confirm-title - replaces the default dialog title "Please Confirm"
// vsc-confirm-message - replaces the default dialog message "Do you confirm this delete?"

(function ($) //ensure exclusive use of $ for jQuery
{
    console.log("VSC JavaScript Library vsc.dialogs.confirm.js");

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

    //replace any existing ConfirmDialog
    console.log(">> Creating VSC.Dialogs.ConfirmDialog namespace");
    VSC.Dialogs.ConfirmDialog = {};

    (function ()
    {
        this.StartConfirmDialog = function ()
        {
            console.log("VSC.Dialogs.ConfirmDialog loading...");
            if ($("a[vsc-confirm-action]").length > 0)
            {
                //adds the dialog div if they do not already exist
                if ($('#vsc-confirm-dialog').length === 0)
                {
                    console.log("  - adding #vsc-confirm-dialog div");

                    var outerDiv = $("<div>").attr("id", "vsc-confirm-dialog")
                        .attr("tabindex", "-1")
                        .attr("aria-labelledby", "confirmmodallabel")
                        .addClass("modal").addClass("fade").prependTo($(document.body));
                    var documentDiv = $("<div>").addClass("modal-dialog").attr("role", "document").appendTo($(outerDiv));
                    var contentDiv = $("<div>").attr("id", "confirmmodalcontent").addClass("modal-content").appendTo($(documentDiv));

                    var dheader = $("<div>").addClass("modal-header").css("border-radius", "5px 5px 0 0").appendTo(contentDiv);
                    $("<h5>").addClass("modal-title").text("Please Confirm").appendTo(dheader);
                    var linkButton = $("<a>").addClass("btn").attr("data-bs-dismiss", "modal").attr("aria-label", "Close").appendTo(dheader);
                    $("<i>").addClass("bi-x-lg").appendTo(linkButton);
                    var dbody = $("<div>").addClass("modal-body").appendTo(contentDiv);
                    $("<p>").text("Do you confirm this delete?").appendTo(dbody);
                    var dfooter = $("<div>").addClass("modal-footer").appendTo(contentDiv);
                    $("<a>").addClass("btn").addClass("btn-primary").addClass("btn-confirm-action").text("Ok")
                        .on('click', function (e)
                        {
                            //$("div.dialog").slideUp(); - removed - this is for old-style slide-down table dialogs
                            //-->call to the action gets bound here (see ---CONFIRM BUTTON--- section below
                        })
                        .appendTo(dfooter);
                    $("<button>").addClass("btn").addClass("btn-outline-secondary").addClass("btn-cancel").attr("data-bs-dismiss", "modal").text("Cancel")
                        .appendTo(dfooter);
                }

                //---------------------CONFIRM BUTTON CLICK-----------------------------//
                $("a[vsc-confirm-action]").on('click', function (e)
                {
                    $(".context-menu").hide();
                    console.log(" --> Confirm button click");
                    var title = $(e.currentTarget).attr("vsc-confirm-title");
                    if (title !== undefined) $("#vsc-confirm-dialog div.modal-header h5").text(title);
                    console.log("  - title = " + title);
                    var msg = $(e.currentTarget).attr("vsc-confirm-message");
                    if (msg !== undefined) $("#vsc-confirm-dialog div.modal-body p").text(msg);
                    console.log("  - msg = " + msg);
                    var url = $(e.currentTarget).attr("href");
                    //this can use the id from a table row
                    if ($(e.currentTarget).attr("vsc-dialog-action-id") === "True")
                    {
                        var getKeyFrom = $(e.currentTarget).data("GetKeyFromTable");
                        if (getKeyFrom !== undefined)
                        {
                            var id = $("#" + getKeyFrom).data("key");
                            if (id !== null && id !== undefined)
                            {
                                url += "/" + id;
                            }
                        }
                    }

                    //bind the confirm action
                    $("#vsc-confirm-dialog a.btn-confirm-action").attr("href", url);
                    console.log("  - confirm dialog Ok button is bound to " + url);
                    $("#vsc-confirm-dialog").modal("show");

                    return false; //handled
                });

                console.log("  - added confirm button");
            }
            else
            {
                console.log("  - no confirm buttons found.");
            }
        };
    }).apply(VSC.Dialogs.ConfirmDialog);



    //run this now
    VSC.Dialogs.ConfirmDialog.StartConfirmDialog();
    console.log("Done.");
    console.log("");

})(jQuery);