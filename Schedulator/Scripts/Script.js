﻿var colorCount = 0;
/* Alternating color for each course in schedule
 * @param courseCode class for each diff block
 */

function courseAltColor(courseCode) {


//    var colors = ["#f57722", "#d3d3d3", "#67aded","#f7fe2e", "#d3ffce", "#daa520", "#66cdaa","#cbbeb5"];
    //var randomColor = '#' + (0x1000000 + (Math.random()) * 0xf57722).toString(16).substr(1, 6);

    //var colors = ["#FAB586", "#DFDFDF", "#90C3F2", "#F9FE67", "#DFFFDB", "#E4BE5D", "#90DBC1", "#D9D0C9"];
    
    // red, orange, lightblue, lightgreen, purple,yellow,turquoise,grey
    //var colors = ["#f06b5d", "#f0ad4e", "#5bc0de", "#5cb85c", "#B284C5", "#f1c40f", "#1abc9c", "#bdc3c7"];
    //  grey, purple, lightblue, turquoise, lightgreen, yellow, orange,red
    var colors = ["#bdc3c7", "#B284C5", "#5bc0de", "#1abc9c", "#5cb85c", "#f1c40f", "#f0ad4e", "#f06b5d"];
    
    if (colors[colorCount] == undefined || typeof colors[colorCount] == "undefined") {
        colorCount = 0;
    }

    var shade = colors[colorCount];
    var white = "#FFFFFF";
    colorCount += 1;

    $("." + courseCode).css({
        backgroundColor: shade,
        color: white
    });

}


var count = 0;
var courseList;
var url = '/ScheduleGenerator/CoursesViewJson';

