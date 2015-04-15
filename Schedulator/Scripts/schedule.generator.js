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
        $('#courseSequenceRecommend').slideToggle('slow');
    });

    $('#generateSch').click(function (e) {
        colorCount = 0;
        $("#prefOptions").slideUp('slow');
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

    $("#pageTitle").click(function () {
        $("#prefOptions").slideDown('slow');
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