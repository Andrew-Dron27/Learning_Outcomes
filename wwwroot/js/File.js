/*
 * Andrew Dron
 *Script to upload and delete files from the backend db
 * Example files have a score associated with them and there can be multiple saved on the DB
 * per learning outcome
 * There can only be on definition file
 */
async function UploadFile(e, ID, Def, cellID) {

    var score = $(e[1]).val();
    
    if ($(e[0]).val() == "") {
        Swal.fire(
            'No File Selected',
            'Please select a file and try again',
            'error'
        )
        return;
    }  
    if (Def) {
        upLoadDefFile(e, ID)

    } else {
        uploadExampleFile(e, ID)  
    }
}

/*
 * Sends Upload request to the controller for a definiton file
 * I cannot for the life of me figure out how to get proper
 * error messages to display
 */
function upLoadDefFile(e, ID) {

    Swal.fire({
        title: 'Upload Definition File',
        showCancelButton: true,
        confirmButtonText: 'upload',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            const formData = new FormData(e);
            return fetch(e.action, {
                method: 'POST',
                body: formData,
                headers: {
                    'File-type': "Definition",
                    'OutcomeID': ID,
                    'RequestVerificationToken': getCookie('RequestVerificationToken'),
                }
            }
            ).then(response => {
                if (!response.ok) {
                    throw new Error(response.statusText)
                }
                return response.json()
            })
                .catch(error => {
                    
                    Swal.showValidationMessage(
                        `Request failed: ${error}`
                    )
                })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then(result => {
        if (result.value) {
            Swal.fire(
                "File Uploaded",
                'success'
            ).then(() => {
                location.reload();
            })
        }
    })
}
/*
 * Sends upload request to the controller with the assocated file stream
 * and score
 * Appropriate error messages are not displayed due to technical difficulties
 */
function uploadExampleFile(e, ID) {

    Swal.fire({
        title: 'Upload Example File, Please Input the Example\'s Score',
        input: 'text',
        inputAttributes: {
            autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: 'upload',
        showLoaderOnConfirm: true,
        preConfirm: (score) => {
            if (!score) {
                return { "Error": "Please Input A Score" };
            }
            scoreNum = parseInt(score);
            if (isNaN(scoreNum)) {
                return { "Error": "Score Must Be An Integer" };
            }
            if (scoreNum < 0 || scoreNum > 100) {
                return { "Error": "Please Input A Correct Score Between 0 and 100" };
            }

            const formData = new FormData(e);
            return fetch(e.action, {
                method: 'POST',
                body: formData,
                headers: {
                    'File-type': "Example",
                    'OutcomeID': ID,
                    'RequestVerificationToken': getCookie('RequestVerificationToken'),
                    'OutcomeScore': score
                }
            }
            ).then(response => {
                if (!response.ok) {
                    throw new Error(response.statusText)
                }
                return response.json()
            })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error}`
                    )
                })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then(result => {
        if (result.value.Error) {
            Swal.fire(
                "An Error Occured",
                result.value.Error,
                "error"
            )
        }
        else if (result.value) {
            Swal.fire(
                "File Uploaded",
                'success'
            ).then(() => {
                location.reload();
            })
        }
    })
}

/*
 * Retrives the cookie from a name
 * Taken from the .net core sample application
 * https://github.com/aspnet/AspNetCore.Docs/blob/master/aspnetcore/mvc/models/file-uploads/samples/2.x/SampleApp/Pages/StreamedSingleFileUploadDb.cshtml
 */
function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}
/*
 * Sends a delete request to the controller
 */
function deleteFile(e, ID) {
    e.preventDefault();
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/LearningOutcomeInstances/DeleteFile",
                data: {
                    ID: ID
                },
                method: "POST",
            }).done(function (result) {
                if (result.success == false) {
                    swal.fire(
                        "An error occured whilst deleting the file",
                        "error"
                    )
                }
                else {
                    swal.fire(
                        "File Deleted",
                        "success"
                    ).then(() => {
                        location.reload();
                    });

                }
            })
        }
    })
}
/*
 * Uploads a given API key to the database
 * Verification of the key is not present in this version
 */
function uploadApiKey(e) {
    
    Swal.fire({
        title: 'Upload ',
        input: 'text',
        inputAttributes: {
            autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: 'upload',
        showLoaderOnConfirm: true,
        preConfirm: (key) => {
            $.ajax({
                url: "/CourseInstances/uploadApiKey",
                data: {
                    apiKey: key
                },
                method: "GET",
            }).then(response => {
                if (response.success) {
                    Swal.fire(
                        "Successfully uploaded Api key",
                        "success"
                    ).then(() => {
                        location.reload();
                    })
                }
            })
        }
    })
    e.preventDefault();
}
