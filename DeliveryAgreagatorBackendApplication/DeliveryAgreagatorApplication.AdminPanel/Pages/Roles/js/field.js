const selectField = document.getElementById('selected-role');


selectField.addEventListener('change', function () {

    const selectedValue = selectField.value;


    if (selectedValue === 'Manager' || selectedValue === 'Cook') {
        document.getElementById('id-field').style.visibility = "hidden";
    }
    else {
        document.getElementsById('id-field').style.visibility = "visible";
    }
});