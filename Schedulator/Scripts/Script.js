var colorCount = 0;

/* Alternating color for each course in schedule
 * @param courseCode class for each diff block
 */
function courseAltColor(courseCode) {
    var colors = ["#FAB586", "#d3d3d3", "#67aded", "#f7fe2e", "#d3ffce", "#daa520", "#66cdaa", "#cbbeb5"];
    //var randomColor = '#' + (0x1000000 + (Math.random()) * 0xf57722).toString(16).substr(1, 6);
    
    if (colors[colorCount] == undefined || typeof colors[colorCount] == "undefined") {
        colorCount = 0;
    }

    var shade = colors[colorCount];
    colorCount += 1;

    $("." + courseCode).css({
        backgroundColor: shade
    });



}

$(document).ready(function () {
    $("#courseSequenceRecommend").load('/ScheduleGenerator/StudentsCourseSequence');

    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

    $('#showCourseSequence').click(function () {
        $('#courseSequenceRecommend').show("slow");
    });

    $('#generateSch').click(function (e) {
        colorCount = 0;

        if ($('#selectedCourses').is(':empty')) {
            e.preventDefault();
            $('#addCourseErr').delay(50).fadeOut().fadeIn('fast');
            return false;
        } else {
            $('#addCourseErr').hide();
            $('#result').html('');
            $("#divProcessing").show();
        }
    });

    $('#result, #courseSequenceRecommend').bind("DOMSubtreeModified", function () {
        $("#divProcessing").hide();
    });

});


