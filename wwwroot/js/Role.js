/*
 * Caleb Edwards & Andrew Dron
 *
 * This page manages the js for the roles
 */

$(function () { console.log("loaded"); });

/**
 * 
 *Ajax call for denying request
 * @param {any} e
 * @param {any} role
 * @param {any} username
 */
function deny_rolerequest(e, role, username) {
    $.ajax({
        url: "/Home/Deny_RoleRequest",
        data: {
            role: role,
            username: username
        },
        method: "POST",
    }).done(function (result) {
        console.log("action taken: " + result)
        if (result.success) {
            //reload page if successfull in order for switches to update
            Swal.fire(
                'Good job!',
                'You requested to not change a role',
                'success'
            )
        }
        else {
            Swal.fire(
                'Bad Job',
                'The Role change was not requested',
                'error'
            )
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("failed: "); console.log(jqXHR); console.log(textStatus); console.log(errorThrown);
        Swal.fire(
            'Failure!',
            'The transaction failed',
            'error'
        )
    }).always(function () {
        console.log("attempting to change role")
    });
}


/**
 * approve role change request
 * @param {any} e
 * @param {any} role
 * @param {any} username
 */
function submit_rolerequest(e, role, username) {
    $.ajax({
        url: "/Home/Submit_RoleRequest",
        data: {
            role: role,
            username: username
        },
        method: "POST",
    }).done(function (result) {
        console.log("action taken: " + result)
        if (result.success) {
            //reload page if successfull in order for switches to update
            Swal.fire(
                'Good job!',
                'You changed a role',
                'success'
            )
        }
        else {
            Swal.fire(
                'Bad Job',
                'The Role change was not submitted',
                'error'
            )
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("failed: "); console.log(jqXHR); console.log(textStatus); console.log(errorThrown);
        Swal.fire(
            'Failure!',
            'The transaction failed',
            'error'
        )
    }).always(function () {
        console.log("attempting to change role")
    });
}

/**
 * request role change ajax call
 * @param {any} e
 * @param {any} role
 * @param {any} username
 */
function Request_role(e, role, username) {
    e.preventDefault();
    $.ajax({
        url: "/Home/Request_role",
        data: {
            role: role,
            username: username
        },
        method: "POST",
    }).done(function (result) {
        console.log("action taken: " + result)
        if (result.success) {
            //reload page if successfull in order for switches to update
            Swal.fire(
                'Good job!',
                'You requested a role change',
                'success'
            )
        }
        else {
            Swal.fire(
                'Bad Job',
                'The Role change was not requested',
                'error'
            )
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("failed: "); console.log(jqXHR); console.log(textStatus); console.log(errorThrown);
        Swal.fire(
            'Failure!',
            'The transaction failed',
            'error'
        )
    }).always(function () {
        console.log("attempting to change role")
    });
}


/**
 * This function submits to the server a post with correct data for role and waits for return values 
 * @param {any} e
 * @param {any} id
 * @param {any} role
 * @param {any} username
 */
function change_role(e, id, role, username) {
    
    var checked = $('#' + id).prop("checked");
    $.ajax({
        url: "/Home/ChangeRole",
        data: {
            check: $('#'+id).prop("checked"),
            username: username,
            role: role
        },
        method: "POST",
    }).done(function (result) {
        console.log("action taken: " + result)
        if (result.success) {
            //reload page if successfull in order for switches to update
            Swal.fire(
                'Good job!',
                'You Changed a role',
                'success'
            )
        }
        else {
            Swal.fire(
                'Bad Job',
                'The Role was not changed',
                'error'
            )
            if (!checked) {
                $('#' + id).prop("checked", "checked");
            }
            else {
                $('#' + id).prop("checked", false);
            }
            
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("failed: "); console.log(jqXHR); console.log(textStatus); console.log(errorThrown);
        Swal.fire(
            'Failure!',
            'The transaction failed',
            'error'
        )
    }).always(function () {
        console.log("attempting to change role")
    });
     //e.preventDefault();

}