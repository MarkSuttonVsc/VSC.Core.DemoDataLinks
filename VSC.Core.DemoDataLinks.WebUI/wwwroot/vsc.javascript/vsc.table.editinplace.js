//vsc.ajax.editinplace jQuery plug-in
//update: 11/03/2025 new name and deprecated keyCode and trim changed

//this plug-in is activated by HTML vsc-table-edit-in-place-* attributes (declarative)
//can use AppPath hidden control for relative app paths



; (function ($) //ensure exclusive use of $ for jQuery
{

	console.log("VSC JavaScript Library vsc.ajax.editinplace.js");

	//this plugin need a trim function replacement
	function vscTrim(s)
	{
		if (typeof s.valueOf(s) === 'string')
		{
			return s.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, '');
		}
		else
		{
			return s.valueOf(s);
		}
	}

	//VSC Javascript Namespace Setup
	if (typeof VSC === "undefined")
	{
		console.log("creating VSC JavaScript namespace");
		VSC = {};
	}
	if (typeof VSC.Ajax === "undefined")
	{
		console.log(">> Creating VSC.Ajax namespace");
		VSC.Ajax = {};
	}

	//replace any existing EditInPlace
	console.log(">> Creating VSC.Ajax.EditInPlace namespace");
	VSC.Ajax.EditInPlace = {};

	//dependency - VSC.Path
	if (typeof VSC.Path === "undefined")
	{
		throw "Developer Warning: VSC.Ajax.EditInPlace requires VSC.Path";
	}
	else
	{
		console.log("  Dependency - VSC.Path is loaded OK");
		console.log("  VSC.AppPath = " + VSC.AppPath);
	}

	(function ()
	{
		this.StartEditInPlace = function ()
		{
			console.log("VSC Javascript ajax edit-in-place is loading...");
			if ($("td[vsc-edit-in-place]").length > 0)
			{
				console.log("--> " + $("td[vsc-edit-in-place]").length + " edit-in-place cells found. ");

				$("td[vsc-edit-in-place]")
					.prepend("<i class='float-end text-faded-reveal bi bi-pencil-square' title='click anywhere in cell to edit'></span>")
					.on('click', function (e)
					{
						console.log("--> vsc-edit-in-place click");

						//do not edit when parent cell is disabled
						if ($(e.currentTarget).hasClass("disabled"))
						{
							console.log(" x no click - this cell is disabled.");
							return false;
						}

						if ($(e.currentTarget).data("status") == undefined)
						{
							//going into edit mode
							$(e.currentTarget).data("status", "edit");
							var oldvalue = vscTrim($(e.currentTarget).text());
							var olddisplay = vscTrim($(e.currentTarget).html());
							var isencoded = false;
							if (oldvalue === "-") value = "";
							if ($(e.currentTarget).attr("vsc-edit-in-place-encoded") === "true")
							{
								oldvalue = $(e.currentTarget).attr("vsc-edit-in-place-oldvalue"); //unencoded version
								isencoded = true;
							}
							var value = oldvalue;

							$(e.currentTarget).empty();

							//attach the old value(s) as a property 
							$(e.currentTarget).data("oldvalue", oldvalue);
							$(e.currentTarget).data("olddisplay", olddisplay);
							$(e.currentTarget).data("isencoded", isencoded);
							console.log("  old value saved is " + oldvalue);
							if (isencoded)
							{
								console.log("  the value is vsc-encoded.");
							}

							var editinput = null; //this will be assigned to the editing element

							//---------input or date---------------------------------------------------------
							if ($(e.currentTarget).attr("vsc-edit-in-place-type") === "input"
								|| $(e.currentTarget).attr("vsc-edit-in-place-type") === "date")
							{
								editinput = $("<input>").val(value) //.addClass("vsc-edit-in-place")
									.on('keyup', function (kev)
									{
										console.log("KEY " + kev.key);
										if (kev.key == "ArrowUp") //Code == 38) //up-arrow
										{
											$(kev.currentTarget).parents("tr").prev("tr")
												.find("td[vsc-edit-in-place]")
												.trigger("click");
											return false;
										}
										if (kev.key == "ArrowDown" //Code == 40 //down-arrow
											|| kev.key == "Enter") //13)
										{
											$(kev.currentTarget).parents("tr").next("tr")
												.find("td[vsc-edit-in-place]")
												.trigger("click");
											return false;
										}

										if (kev.key == "Escape") //27) //escape to cancel the edit
										{
											$(e.currentTarget).empty().text(oldvalue)
												.removeData("status");
											$("<i class='float-end text-faded-reveal bi bi-pencil-square' title='click anywhere in cell to edit'></i>")
												.prependTo($(e.currentTarget));
											return false;
										}
									});

								if ($(e.currentTarget).attr("vsc-edit-in-place-type") === "input")
								{
									//text select all
									editinput[0].selectionStart = 0;
									editinput[0].selectionEnd = value.length;
								}
								else
								{
									//add a date picker (must be in relative container)
									$(e.currentTarget).css("position", "relative");
									//$(editinput).datepicker({ format: "dd M yyyy" });
								}
							}

							//---------text area----------------------------------------------------------
							if ($(e.currentTarget).attr("vsc-edit-in-place-type") === "text")
							{
								editinput = $("<textarea rows='5'>").val(value)
									.on('keyup', function (kev)
									{
										//up and down acts inside the text area
										if (kev.code == "Escape") //27) 
										{
											$(e.currentTarget).empty().text(oldvalue)
												.removeData("status");
											$("<i class='float-end text-faded-reveal bi bi-pencil-square' title='click anywhere in cell to edit'></i>")
												.prependTo($(e.currentTarget));
											return false;
										}
									});

								//text cursor at end
								editinput[0].selectionStart = value.length;
								editinput[0].selectionEnd = value.length;
							}

							//---------checkbox as drop-down---------------------------------------------------------
							if ($(e.currentTarget).attr("vsc-edit-in-place-type") === "bool")
							{
								var options = "<option value='true'>Yes</option><option value='false' selected>No</option>"
								if (oldvalue.startsWith("Y"))
								{
									options = "<option value='true' selected>Yes</option><option value='false'>No</option>"
								}
								editinput = $("<select>" + options + "</select>")
									.on('keyup', function (kev)
									{
										if (kev.key == "ArrowUp")// 38) //up-arrow
										{
											$(kev.currentTarget).parents("tr").prev("tr")
												.find("td[vsc-edit-in-place]")
												.trigger("click");
											return false;
										}
										if (kev.key == "ArrowDown" //Code == 40 //down-arrow
											|| kev.key == "Enter") // Code == 13)
										{
											$(kev.currentTarget).parents("tr").next("tr")
												.find("td[vsc-edit-in-place]")
												.trigger("click");
											return false;
										}

										if (kev.key == "Escape") //Code == 27) //escape
										{
											$(e.currentTarget).empty().text(oldvalue)
												.removeData("status");
											return false;
										}
									});
								//no text selection in checkbox select
							}

							//---------select----------------------------------------------------------
							if ($(e.currentTarget).attr("vsc-edit-in-place-type") === "select")
							{
								editinput = $("<select>")
									.on('keyup', function (kev)
									{
										if (kev.key == "ArrowUp") //Code == 38) //up-arrow
										{
											$(kev.currentTarget).parents("tr").prev("tr")
												.find("td[vsc-edit-in-place]")
												.trigger("click");
											return false;
										}
										if (kev.key == "ArrowDown" //Code == 40 //down-arrow
											|| kev.key == "Enter") //Code == 13)
										{
											$(kev.currentTarget).parents("tr").next("tr")
												.find("td[vsc-edit-in-place]")
												.trigger("click");
											return false;
										}

										if (kev.key == "Escape") //Code == 27) //escape
										{
											$(e.currentTarget).empty().text(oldvalue)
												.removeData("status");
											return false;
										}
									});
								//no text selection in select
								//get options
								var url = $(e.currentTarget).attr("vsc-edit-in-place-options");
								//this must combine with the app path
								//uses AppPath for ajax path in sub-folders
								VSC.AppPath = $("#hiddenAppPath").val();
								if (VSC.AppPath === undefined) VSC.AppPath = "";
								console.log("  VSC.AppPath = " + VSC.AppPath);
								var optionsData = { value: oldvalue }
								var jsonOptionsData = JSON.stringify(optionsData);
								$.ajax({
									contentType: "application/json charset=utf-8",
									data: jsonOptionsData,
									dataType: "json",
									type: "post",
									url: VSC.Path.Combine(VSC.AppPath, url),
									success: function (data)
									{
										$(editinput).html(data.options);
									}
								});
							}


							//--------------All input types-------------------------------------//
							var editInputId = "vsc-edit-in-place-" + $(e.currentTarget).attr("vsc-edit-in-place-name")
								+ "-" + Math.random().toString(16).slice(2, 10);
							console.log("  Editing input id assigned " + editInputId);
							$(editinput).appendTo($(e.currentTarget))
								.addClass("form-control")
								.attr("id", editInputId)
								.trigger('focus')
								.on('blur', function (evt)
								{
									var newvalue = vscTrim($(evt.currentTarget).val());
									if (newvalue === "") newvalue = "-";
									if (newvalue === oldvalue)
									{
										//no change in value
										$(e.currentTarget) //the cell
											.empty()
											.removeData("status");
										if (isencoded)
										{
											$(e.currentTarget).html(olddisplay);
										}
										else
										{
											$(e.currentTarget).text(oldvalue);
											$("<i class='float-end text-faded-reveal bi bi-pencil-square' title='click anywhere in cell to edit'></i>")
												.prependTo($(e.currentTarget));
										}

										console.log("   x no change.");
										return false;
									}

									//------------value changed----------------
									$(evt.currentTarget).addClass("data-editor-faded").attr("readonly", "true");
									window.setTimeout(function ()
									{
										var formData = {
											name: $(e.currentTarget).attr("vsc-edit-in-place-name"),
											key: $(e.currentTarget).attr("vsc-edit-in-place-key"),
											value: vscTrim($(evt.currentTarget).val()),
											elementid: $(evt.currentTarget).attr("id")
										};
										var url = $(e.currentTarget).attr("vsc-edit-in-place");
										var jsonData = JSON.stringify(formData);
										console.log("Sending data to " + url);
										console.log(jsonData);
										$.ajax({
											contentType: "application/json charset=utf-8",
											data: jsonData,
											dataType: "json",
											type: "post",
											url: VSC.Path.Combine(VSC.AppPath, url),
											success: function (data)
											{
												console.log(" - data saved, response returned with success.");
												$("#" + data.elementId).fadeOut(
													function ()
													{
														var oldvalue = $("#" + data.elementId).val(); //this is not encoded
														var newvalue = data.newValue;
														var isencoded = data.isencoded;

														if (vscTrim(newvalue) === "") newvalue = "";
														if (!data.status) newvalue = "";

														var targetCell = $("#" + data.elementId).parent("td");

														targetCell.removeData("status").empty();
														console.log("  old contents cleared.");

														if (isencoded)
														{
															console.log("  setting cell html for new value.");
															targetCell.attr("vsc-edit-in-place-oldvalue", oldvalue);
															targetCell.html(newvalue);
														}
														else
														{
															console.log("  setting text to new value.");
															targetCell.text(newvalue);
														}
														targetCell.prepend($("<i class='float-end text-faded-reveal bi bi-pencil-square' title='click anywhere in cell to edit'></i>"));

														VSC.Ajax.EditInPlace.FinishEditInPlace($(e.currentTarget), data.key, data.name, data.value, data.newValue);
													});
											}
										});
									}, 200);
								});
						}
						return false;
					});
			}
			else
			{
				console.log("  - no edit-in-place cells present.");
			}

		}
	}).apply(VSC.Ajax.EditInPlace);

	(function ()
	{
		this.FinishEditInPlace = function (cell, key, name, value, newvalue, command)
		{
			if (cell.attr("vsc-edit-in-place-success"))
			{
				var functionName = cell.attr("data-edit-in-place-success");
				console.log("finish it with function " + functionName);
				console.log("  key = " + key);
				console.log("  name = " + name);
				console.log("  value = " + value);
				console.log("  newvalue = " + newvalue);
				console.log("  command = " + command);
				window[functionName](cell, key, name, value, newvalue, command);
			}
			else
			{
				console.log("  - no finish function set");
			}
		}
	}).apply(VSC.Ajax.EditInPlace);


	//run this now
	VSC.Ajax.EditInPlace.StartEditInPlace();
	console.log("Done.");
	console.log("");

})(jQuery)