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

var count = 0;
var courseList;
var url = '/ScheduleGenerator/CoursesViewJson';

$(function () {
    $("#addcourse").click(function () {
        var name = $("input[name='courseName']").val();
        $("input[name='courseName']").val("");
        $(".selected-courses").children().append("<li>"+name+"<input name='courseCode["+count+"]' value='" + name + "' hidden><input type='button' value='Remove' class='removeClass'/></li>");
        count++;
    });

    $(".removeClass").click(function () {
        this.parent().hide();
    });

    $.getJSON(url, function (data) {
        console.log(data);
        courseList = data;
        console.log(courseList);
        
    });

});


function showHint(str) {
      
    console.log(str);
    $("#suggestion").autocomplete({
        source: courseList     


    });
  
}
