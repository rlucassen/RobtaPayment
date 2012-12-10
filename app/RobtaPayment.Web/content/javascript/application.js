$(document).ready(function () {
    $("table").tablesorter();

    $(".delete-button").click(function () {
        return confirm("Weet je het zeker?");
    });

    $('#enrolment_Exam_Id').change(function () {
        var toets = $(this).find('option:selected').val();
        $('#education').hide();
        $('#education.' + toets).show();
    }).trigger('change');
});
