﻿
/* Alternating color for each course in schedule
 * @param courseCode class for each diff block
 */
function courseAltColor(courseCode) {

    var randomColor = '#' + (0x1000000 + (Math.random()) * 0xffffff).toString(16).substr(1, 6);//Math.floor(Math.random() * 16777215).toString(16);

    function shadeColor1(color, percent) {
        var num = parseInt(color.slice(1), 16), amt = Math.round(2.55 * percent), R = (num >> 16) + amt, G = (num >> 8 & 0x00FF) + amt, B = (num & 0x0000FF) + amt;
        return "#" + (0x1000000 + (R < 255 ? R < 1 ? 0 : R : 255) * 0x10000 + (G < 255 ? G < 1 ? 0 : G : 255) * 0x100 + (B < 255 ? B < 1 ? 0 : B : 255)).toString(16).slice(1);
    }

    $("." + courseCode).css({
        backgroundColor: shadeColor1(randomColor, 45)
    });



}

<<<<<<< HEAD
var count = 0;
var courseList;
var url = '/ScheduleGenerator/CoursesViewJson';

$(function () {
    $("#addcourse").click(function () {
   //     debugger;
        var name = $("input[name='courseName']").val();
        if (name.length > 0) {
            $(".selected-courses").children().append("<li>" + name + "<input name='courseCode[" + count + "]' value='" + name + "' hidden><input type='button' value='Remove' class='removeClass li"+count+"'/></li>");
        }
        $("input[name='courseName']").val("");
        count++;
    });

    $(".removeClass").click(function () {
        //this.addClass("clicked");
        this.parent().hide();
=======
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
    $('#generateSch').click(function () {
        $('#result').html('');
        $("#divProcessing").show();
>>>>>>> 95b599510b92d715a28754ff7c4d97988d7e816b
    });

    $('#result').bind("DOMSubtreeModified", function () {
        $("#divProcessing").hide();
    });

});


