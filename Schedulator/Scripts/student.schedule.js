var currentSemester;
$(document).ready(function () {
    
    $(".schedule-semester").click(function () {
        var url = '/StudentSchedule/GetSchedule?semester=' + $(this).attr('semester');
        currentSemester = $(this).attr('semester');
        $("#schedule-div").load(url);
    });

});