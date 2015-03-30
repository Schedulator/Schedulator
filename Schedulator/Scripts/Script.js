/* Loading indicator */
//$(document).ready(function () {
//    $.ajaxSetup({
//        beforeSend: function () {
//            // show gif here, eg:
//            $("#loading").show();
//        },
//        complete: function () {
//            // hide gif here, eg:
//            $("#loading").hide();
//        }
//    });

//    $.ajax({
//        global: false,

//    });
//});

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
        backgroundColor: shadeColor1(randomColor, 25)
    });



}

$(function () {
    $("#addcourse").click(function () {
        var name = $("input[name='courseName']").val();
        $("input[name='courseName']").val("");
        $(".selected-courses").children().append("<li>"+name+"</li>");
    });
});

function showHint(str) {
    var courses = ["COMP 232", "COMP 352", "COMP 348", "SOEN 341", "SOEN 331"];
    console.log(str);
    if (str.length == 0) {
        $(".suggestion").html("");
        return;
    } else {
        console.log("inside else");
        str = str.toUpperCase();
        var len = str.length;
        console.log(str +"TEST");
        var chkctr = 0;
        $(courses).each(function (idx, course) {
            
            console.log(course + " in 1");
            var partial = course.substring(0, len);
            console.log(partial);
            if (str == partial) {
                
                $(".suggestion").append(course + " ");
                console.log("here");
            } /*else {
                console.log("2 else");
                $(".suggestion").append(" Nothing found");
            }*/
        });
    }
}
