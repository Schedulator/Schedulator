function showHint(str) {
	$("#suggestion").autocomplete({
		source: courseList,
		autoFocus: true,
		select: function (event, ui) {
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
var count = 0;
var courseList;
var url = '/ScheduleGenerator/CoursesViewJson';

$(document).ready(function () {
	$("#addCourse").click(function () {
		var name = $("input[name='courseName']").val();
		$("input[name='courseName']").val("");
		$("#selectedCourses").append("<div class='col-sm-1 courseBlock'>" + name + "<input name='courseCode[" + count + "]' value='" + name + "' hidden></div>");
		count++;
	});

	$(".removeClass").click(function () {
		this.parent().hide();
	});

	$.getJSON(url, function (data) {
		courseList = data;
	});


});