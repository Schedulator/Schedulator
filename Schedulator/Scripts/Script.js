var colorCount = 0;
/* Alternating color for each course in schedule
 * @param courseCode class for each diff block
 */

function courseAltColor(courseCode) {


//    var colors = ["#f57722", "#d3d3d3", "#67aded","#f7fe2e", "#d3ffce", "#daa520", "#66cdaa","#cbbeb5"];
    //var randomColor = '#' + (0x1000000 + (Math.random()) * 0xf57722).toString(16).substr(1, 6);

    //var colors = ["#FAB586", "#DFDFDF", "#90C3F2", "#F9FE67", "#DFFFDB", "#E4BE5D", "#90DBC1", "#D9D0C9"];
    
    // red, orange, lightblue, lightgreen, purple,yellow,turquoise,grey
    var colors = ["#e74c3c", "#f0ad4e", "#5bc0de", "#5cb85c", "#B284C5", "#f1c40f", "#1abc9c", "#bdc3c7"];
    
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
    });

    //$(document).ready(function () {
    $("#courseSequenceRecommend").load('/ScheduleGenerator/StudentsCourseSequence');



    $('#showCourseSequence').click(function () {
        $('#courseSequenceRecommend').show("slow");
    });

    $('#generateSch').click(function () {
        colorCount = 0;
    });
    $('#generateSch, #showCourseSequence').click(function () {
        $('#result').html('');
        $("#divProcessing").show();

    });

    $('#result, #courseSequenceRecommend').bind("DOMSubtreeModified", function () {
        $("#divProcessing").hide();
    });



});