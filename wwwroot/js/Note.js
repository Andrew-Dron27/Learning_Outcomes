/*
 * Caleb Edwards & Andrew Dron
 * 
 * This page is for managing the js in the Notes
 */

$(function () { console.log("loaded"); });

/**
 * This function is to submit a note to the controller and based on return value
 * return back to the view if we succeeded or failed
 * @param {any} e
 * @param {any} noteID
 * @param {any} CourseID
 */
function submit_note(e, noteID, CourseID) {
    tinyMCE.triggerSave();
    $.ajax({
        url: "/CourseInstances/ChangeNote",
        data: {
            note: $('#note').val(),
            noteID: noteID,
            CourseInstancesID: CourseID
        },
        method: e.srcElement.method,
    }).done(function (result) {
        console.log("action taken: " + result)
        $('#note').val(result.note)
        $('#Approved').text("Note Pending Approval")
        Swal.fire(
            'Good jorb!',
            'Note Submitted',
            'success'
        )
        // Note sure if we need to show this here or have another 
        $('#approveButton').show();
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("failed: "); console.log(jqXHR); console.log(textStatus); console.log(errorThrown);
        Swal.fire(
            'ERR',
            'Note Failed to Submit',
            'error'
        )
    }).always(function () {
        console.log("but I will always do this")
    });

     e.preventDefault(); 
}

function submit_LO_note(e, noteID, LOID) {
    tinyMCE.triggerSave();
    $.ajax({
        url: "/LearningOutcomeInstances/ChangeNote",
        data: {
            note: $('#note').val(),
            noteID: noteID,
            LOID: LOID
        },
        method: e.srcElement.method,
    }).done(function (result) {
        console.log("action taken: " + result)
        $('#note').val(result.note)
        Swal.fire(
            'Good jorb!',
            'Note Submitted',
            'success'
        )
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("failed: "); console.log(jqXHR); console.log(textStatus); console.log(errorThrown);
        Swal.fire(
            'ERR',
            'Note Failed to Submit',
            'error'
        )
    }).always(function () {
        console.log("but I will always do this")
    });

    e.preventDefault(); 
}

function approve_note(e, noteID) {
    $.ajax({
        url: "/CourseInstances/ApproveNote",
        data: {
            noteID: noteID
        },
        method: "POST",
    }).done(function (result) {
        $('#Approved').text("Note Approved")
        Swal.fire(
            'Good jorb!',
            'Note Approved',
            'success'
        )
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("failed: ");
        console.log(jqXHR);
        console.log(textStatus); console.log(errorThrown);
        Swal.fire(
            'ERR',
            'Note Failed to be Approved',
            'error'
        )
    });
    e.preventDefault();
}