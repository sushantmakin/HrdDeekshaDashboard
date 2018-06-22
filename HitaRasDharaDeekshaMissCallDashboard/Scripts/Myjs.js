var showPopup = function (code) {
    debugger;
    switch (code) {
        case 0:
            swal('Success !', 'Applicant data added successfully.', 'success');
            break;
        case 1:
            swal('Applicant already exists !', 'You can update the status if required from Update Status page.', 'error');
            break;
        case 2:
            swal('Technical Error !', 'An error has occured, please try again.', 'error');
            break;
        case 3:
            swal('Error !', 'Applicants current status is same as new status. Updation not required.', 'error');
            break;
        case 4:
            swal('Error !', 'Applicant doesnot exist. Kindly add applicant entry via Add data page.', 'error');
            break;
        case 5:
            swal('Success !', 'Applicants data updated successfully', 'success');
            break;
    }
};

