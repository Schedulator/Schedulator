﻿var colorCount = 0;

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
            showAddCourseMsg('Please add one or more course.');
            e.preventDefault();
            return false;
        } else {
            hideAddCourseMsg();
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





