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
	});
}
function checkIfCourseExist(courseName)
{
	var courseExist = $.grep(courseList, function (e) { return e.label == courseName.toUpperCase(); }).length == 1;
	var courseAlreadyAdded = $.inArray(courseName, addedCourseList) == -1
	if (courseExist && courseAlreadyAdded)
		return true;
	else if (courseExist)
		alert("Course already added");
	else
		alert("Course doesn't exist");
	return false;

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
		addedCourseList.splice($.inArray($(this).text(), addedCourseList), 1);
		$(this).closest("div").remove();
	});
});