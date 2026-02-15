// VSC Path JavaScript source code


//VSC JavaScript Namespace Setup

; (function ($)
{
    console.log("VSC JavaScript Library vsc.path.js");

    if (typeof VSC === "undefined")
    {
        console.log(">> Creating VSC JavaScript namespace");
        VSC = {};
    }
    if (typeof VSC.Path === "undefined")
    {
        console.log(">> Creating VSC.Path namespace");
        VSC.Path = {};
    }

    (function ()
    {
        /**
         * 
         * @param {any} path
         * @param {any} param
         * @param {any} value
         */
        this.AddParameter = function (path, param, value)
        {
            if (path.indexOf("?") > -1)
            {
                return path + "&" + param + "=" + value;
            }
            return path + "?" + param + "=" + value;
        };
        /**
         * 
         * @param {any} p1
         * @param {any} p2
         */
        this.Combine = function (p1, p2)
        {
            if (p1 === undefined) p1 = "";
            if (p2 === undefined) p2 = "";
            var scheme = "";

            //if p1 starts with a scheme, strip it off and store
            if (p1.length > 5 && p1.substr(0, 8) == "https://")
            {
                scheme = "https://";
                p1 = p1.substring(8);
            }
            if (p1.length > 4 && p1.substr(0, 7) == "https://")
            {
                scheme = "http://";
                p1 = p1.substring(7);
            }

            //if p2 starts with a scheme, strip it off and discard

            //strip trailing / from p1
            if (typeof p1 == "string" && p1.length > 0 && p1.substr(p1.length - 1, 1) === "/")
            {
                p1 = p1.substr(0, p1.length - 1);
            }

            //strip p1 off p2 if p2 leads with p1
            if (p1 != "" && p2.length > p1.length && p2.substr(0, p1.length) === p1)
            {
                p2 = p2.substr(p1.length, p2.length - p1.length);
            }

            //strip leading / from p2
            if (p2.length > 0 && p2.substr(0, 1) === "/")
            {
                p2 = p2.substr(1, p2.length);
            }

            var returnpath = scheme + p1 + "/" + p2;

            //final check for trailing /
            if (typeof returnpath == "string" && returnpath.length > 0 && returnpath.substr(returnpath.length - 1, 1) === "/")
            {
                returnpath = returnpath.substr(0, returnpath.length - 1);
            }
            return returnpath;
        };

        this.GetAppPath = function ()
        {
            VSC.AppPath = $("#hiddenAppPath").val();
            if (VSC.AppPath === undefined) VSC.AppPath = "";
            console.log(" --> VSC.Path.GetAppPath");
            console.log("  VSC.AppPath = " + VSC.AppPath);
        };

    }).apply(VSC.Path);

    //call this now
    VSC.Path.GetAppPath();
    console.log("Done.");
    console.log("");
})(jQuery);
