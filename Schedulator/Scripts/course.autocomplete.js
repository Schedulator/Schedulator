function showHint(str) {
    $("#suggestion").autocomplete({
        source: courseList,
        autoFocus: true,
        select: function (event, ui) {
            $("input[name='courseName']").val(ui.item.label);
            $('#addCourse').click();
            this.value = "";
            return false;
        },
        messages: {
            noResults: '',
            results: function () { }
        }
    })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<span style='font-weight:bold'>"+item.label + "</span><br><span style='text-transform:capitalize;font-size:12px'>" + item.desc + "</span>")
            .appendTo(ul);
    };
}
function checkIfCourseExist(courseName)
{
	var courseExist = $.grep(courseList, function (e) { return e.label == courseName.toUpperCase(); }).length == 1;
	var courseAlreadyAdded = $.inArray(courseName, addedCourseList) == -1
	if (courseExist && courseAlreadyAdded)
		return true;
	else if (courseExist)
	    showAddCourseMsg("Course already added");
	else
	    showAddCourseMsg("Course doesn't exist");
	return false;

}


/** 
 * Add course autocomplete message 
 * @param {String} msg Message to show
 **/
function showAddCourseMsg(msg) {
    msg = "" || msg;
    $('#addCourseErr').html(msg);
    $('#addCourseErr').delay(50).fadeOut().fadeIn('fast');  
}

/*
 * Hide add course autocomplete message
 */
function hideAddCourseMsg() {
    $('#addCourseErr').hide();
}

var courseList;
var addedCourseList = [];
$(document).ready(function () {
	$("#addCourse").click(function () {
		var name = $("input[name='courseName']").val();
		$("input[name='courseName']").val("");
		if (checkIfCourseExist(name)) {
		    $("#selectedCourses").append("<div class='courseBlock col-xs-2'><span>" + name.toUpperCase()
                                                               + "</span><input name='courseCode' value='"
                                                               + name + "' hidden> "
                                                               + "<span id='delCourse'> <img src='/Content/img/delete.ico.png' /><span></div>");
		    addedCourseList.push(name);
		    $('#addCourseErr').hide();
		}		
	});

	$(".removeClass").click(function () {
		this.parent().hide();
	});

	$.getJSON('/ScheduleGenerator/CoursesViewJson', function (data) {
		courseList = data;
	});

	$(document).on("click", ".courseBlock", function () {
	    hideAddCourseMsg();
		addedCourseList.splice($.inArray($(this).text(), addedCourseList), 1);
		$(this).closest("div").remove();
	});
});