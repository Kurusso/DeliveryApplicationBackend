const selectField = document.getElementById('selected-role');
const selectField2 = document.getElementById('selected-action')

selectField.addEventListener('change', function () {

    const selectedValue = selectField.value;
    const selectedValue2 = selectField2.value;

    if ((selectedValue == 0 || selectedValue == 1) && selectedValue2 == 0) {
        document.getElementById('id-field').style.display = "";
        document.getElementById('id-field-2').style.display = "";
    }
    else if (selectedValue2 == 2 || selectedValue2 == 3) {
        document.getElementById('id-field').style.display = "none";
        document.getElementById('id-field-2').style.display = "none";
    }
    else {
        document.getElementById('id-field').style.display = "none";
    }
});

selectField2.addEventListener('change', function () {

    const selectedValue = selectField.value;
    const selectedValue2 = selectField2.value;

    if ((selectedValue == 0 || selectedValue == 1) && selectedValue2 == 0) {
        document.getElementById('id-field').style.display = "";
        document.getElementById('id-field-2').style.display = "";
    }
    else if (selectedValue2 == 2 || selectedValue2 == 3) {
        document.getElementById('id-field').style.display = "none";
        document.getElementById('id-field-2').style.display = "none";
    }
    else {
        document.getElementById('id-field').style.display = "none";
        document.getElementById('id-field-2').style.display = "";
    }
});