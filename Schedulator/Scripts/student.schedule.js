var counter = 0;
var count = 0;
function ShowSchedules(data) {
    $('#generatedSchedulesContent').empty();
    $('#generatedSchedulesContent').html(data);
    $('.registerScheduleForm').attr('data-ajax-success', 'ReloadSchedule');
    $('#generatedSchedules').modal('show');
}
function ReloadSchedule() {
    location.reload();
    $('#generatedSchedules').modal('hide');
    counter = 0;

}
function ManageScheduleSuccess(data) {
    $('#schedule').empty();
    $('#schedule').html(data);
}
$(document).ready(function () {

    $(".schedule-semester").click(function () {
        var url = '/StudentSchedule/GetSchedule?semester=' + $(this).attr('semester');
        $("#schedule-div").load(url);
    });

    $('#addCoursesButton').on('click', function () {
        $('#collapse-schedule').collapse('toggle');
    });

    $("#generateSchedule").on("click", function (e) {
        e.preventDefault();
        $('#manageScheduleForm').attr('data-ajax-success', 'ShowSchedules(data)');
        $('#manageScheduleForm').attr('action', "/StudentSchedule/GenerateSchedules").submit();
    });

    $('#removeButton').on('click', function () {
        if ($('input[type=checkbox]:checked').length) {
            $('#submitType').val('remove');
            $('#manageScheduleForm').submit();
        }
        else
            alert("You must select courses to remove!");
    });
    $('#manageScheduleButton').on('click', function () {
        if (counter == 0) {
            var sectionCounter = 0;
            counter = 1;
            $('[id^=radioButtonSectionGroup]').each(function () {
                $(this).after('<input type="checkbox" class="section-checkbox" name="sectionIds" value="' + $(this).val() + '" >');
            });
        }
        else {
            counter = 0;
            $(".section-checkbox").hide();
            $('#collapseSchedule').collapse('hide');
        }
    });
});