//VSC.Forms.DateTime jQuery Plug-in

//created: 14/03/2025 this binds a date, time and hidden inputs together

//plug-in activated by the vsc-date-time attributes (declaritive)

//for a date/time input called myDateTime, these can be linked:
// <input type="hidden" id="myDateTime" vsc-date-time/>
// <input type="date" id="myDateTimeDateOnly"/><input type="time" id="myDateTimeTimeOnly"/>

; (function ($) //ensure exclusive use of $ for jQuery
{
    console.log("VSC JavaScript Library vsc.forms.datetime.js");

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

    console.log(">> Creating VSC.Forms.DateTime namespace");
    //replace any existing DateTime
    VSC.Forms.DateTime = {};

    (function ()
    {
        this.StartDateTime = function ()
        {
            console.log("VSC.Forms.DateTime is starting...");
            if ($("[vsc-date-time]").length == 0)
            {
                console.log(" - no active DateTime elements found.");
            }
            else
            {
                console.log(" " + $("[vsc-date-time]").length + " active DateTime elements found.");
                $("[vsc-date-time]").each(function (i, o)
                {
                    var id = $(o).attr("id");
                    console.log("DateTime element #" + i + " id = " + id);
                    console.log(" - checking valid related inputs:");
                    var checkOk = true;

                    console.log("Date only input = " + "#" + id + "DateOnly");
                    if ($("#" + id + "DateOnly").length == 0)
                    {
                        console.log(" *** missing DateOnly element!");
                        checkOk = false;
                    }

                    console.log("Time only input = " + "#" + id + "TimeOnly");
                    if ($("#" + id + "TimeOnly").length == 0)
                    {
                        console.log(" *** missing TimeOnly element!");
                        checkOk = false;
                    }

                    if (!checkOk)
                    {
                        console.log(" *** check failed, time input will be inactive!");
                    }
                    else 
                    {
                        console.log(" - ok - adding change events...");
                        $("#" + id + "DateOnly").on('change', function (e)
                        {
                            var datenumber = e.currentTarget.valueAsNumber;
                            var timetext = $("#" + id + "TimeOnly").val();

                            if (isNaN(datenumber))
                            {
                                $("#myDateTime").empty();
                            }
                            else
                            {
                                var date = new Date(datenumber);
                                var isoDateString = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + " " + timetext;
                                $("#" + id).val(isoDateString);
                                console.log("Hidden " + id + " value is now " + isoDateString);
                                var callbackFunctionName = $(o).attr("vsc-date-time");
                                if (callbackFunctionName != null && typeof (window[callbackFunctionName]) == "function")
                                {
                                    window[callbackFunctionName](isoDateString);
                                }
                            }
                        });
                        $("#" + id + "TimeOnly").on('change', function (e) { $("#" + id + "DateOnly").trigger('change'); });

                        console.log("Done.");
                    }
                });

            }

        };
    }).apply(VSC.Forms.DateTime);
    //run this now!
    VSC.Forms.DateTime.StartDateTime();

})(jQuery);