var colorCount = 0;

/* Alternating color for each course in schedule
 * @param courseCode class for each diff block
 */
function courseAltColor(courseCode) {
    // var colors = ["#FAB586", "#DFDFDF", "#90C3F2", "#F9FE67", "#DFFFDB", "#E4BE5D", "#90DBC1", "#D9D0C9"];
    var colors = ["#bdc3c7", "#B284C5", "#5bc0de", "#1abc9c", "#5cb85c", "#f1c40f", "#f0ad4e", "#f06b5d"];

    if (colors[colorCount] == undefined || typeof colors[colorCount] == "undefined") {
        colorCount = 0;
    }

    var shade = colors[colorCount];
    colorCount += 1;

    $("." + courseCode).css({
        backgroundColor: shade,
        color: "#FFFFFF"
    });



}

function reloadCourseSequence()
{
    $("#courseSequenceRecommend").load('/ScheduleGenerator/StudentsCourseSequence');
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
        $('#courseSequenceRecommend').collapse("slow");
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

    /** Flashing **/
    window.setTimeout(function () {
        $(".flash").fadeTo(500, 0).slideUp(500, function () {
            $(this).remove();
        });
    }, 5000);

});