//VSC.Forms.AutoSubmit jQuery Plug-in

//update: 01/03/2025 attribute is now 'vsc-auto-submit*'
//update: 06/03/2025 updated deprecated functions click() and submit() to on() and trigger()

//this plug-in is activated by HTML 'vsc-auto-submit*' attributes (declarative)

//Activate AutoSubmit using attribute vsc-auto-submit="True" in a form control
//then the following events trigger a form submit (parent of the control).

// <select vsc-auto-submit="True">  -- on change
// optional vsc-auto-submit-remove-first="True" will also remove the first option in the list after the first selection
// <input type="checkbox" vsc-auto-submit="True">  -- on click
// <input type="radio" vsc-auto-submit="True">  -- on click
// <input type="button" vsc-auto-submit="True">  -- on click
// <a vsc-auto-submit="True"> -- on click (href is ignored)


; (function ($) //ensure exclusive use of $ for jQuery
{
    console.log("VSC JavaScript Library vsc.forms.autosubmit.js");

    //VSC JavaScript Namespace Setup
    if (typeof VSC === "undefined")
    {
        console.log(">> Creating VSC JavaScript namespace");
        VSC = {};
    }

    if (typeof VSC.Forms === "undefined")
    {
        console.log(">> Creating VSC.Forms namespace");
        VSC.Forms = {};
    }

    console.log(">> Creating VSC.Forms.AutoSubmit namespace");
    //replace any existing AutoSubmit
    VSC.Forms.AutoSubmit = {};

    (function ()
    {
        this.StartAutoSubmit = function ()
        {
            console.log("VSC.Forms.AutoSubmit is starting...");
            if ($("[vsc-auto-submit]").length == 0)
            {
                console.log(" - no active AutoSubmit elements found.");
            }
            else
            {
                console.log(" " + $("[vsc-auto-submit]").length + " active AutoSubmit elements found.");
                $("select[vsc-auto-submit]").on('change', function (e)
                {
                   
                    if ($(e.currentTarget).attr("vsc-auto-submit-remove-first") === "True")
                    {
                        console.log(" - with remove-first option.");
                        $(e.currentTarget).find("option")[0].remove();
                        $(e.currentTarget).removeAttr("vsc-auto-submit-remove-first")
                    }
                    console.log(" -> AutoSubmit list change - attempting to submit form.");  
                    $(e.currentTarget).parents("form").trigger('submit');

                });
                $("input[type=checkbox][vsc-auto-submit]").on('click', function (e)
                {
                    console.log(" -> AutoSubmit checkbox click - attempting to submit form.");
                    $(e.currentTarget).parents("form").trigger('submit');
                });

                $("input[type=radio][vsc-auto-submit]").on('click', function (e)
                {
                    console.log(" -> AutoSubmit radio button click - attempting to submit form.");
                    $(e.currentTarget).parents("form").trigger('submit');
                });

                $("input[type=button][vsc-auto-submit]").on('click', function (e)
                {
                    console.log(" -> AutoSubmit input (button) click - attempting to submit form.");
                    $(e.currentTarget).parents("form").trigger('submit');
                });

                $("a[vsc-auto-submit]").on('click', function (e)
                {
                    console.log(" -> AutoSubmit hyperlink click - attempting to submit form.");
                    $(e.currentTarget).parents("form").trigger('submit');
                    console.log("  - note: href was not used.");
                    return false; //handled
                });
            }
            console.log("Done.");
            console.log("");
        };
    }).apply(VSC.Forms.AutoSubmit);
    //run this now!
    VSC.Forms.AutoSubmit.StartAutoSubmit();
})(jQuery);