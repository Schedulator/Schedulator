var colorCount = 0;

/* Alternating color for each course in schedule
 * @param courseCode class for each diff block
 */
function courseAltColor(courseCode) {
    var colors = ["#FAB586", "#DFDFDF", "#90C3F2", "#F9FE67", "#DFFFDB", "#E4BE5D", "#90DBC1", "#D9D0C9"];
    
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


