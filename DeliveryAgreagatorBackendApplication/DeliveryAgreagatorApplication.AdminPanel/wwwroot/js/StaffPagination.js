

$(document).ready(function () {
    $('#table1').DataTable({
        paging: true,
        pageLength: 5,
        searching: true,
        columnDefs: [
            { targets: 0 },
            { targets: 1, type: 'name-search' },
            { targets: 2 }
        ],
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                if (column.index() === 1) {


                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                }
            });
        }
    });
});

$(document).ready(function () {
    $('#table2').DataTable({
        paging: true,
        pageLength: 5,
        searching: true,
        columnDefs: [
            { targets: 0 },
            { targets: 1, type: 'name-search' },
            { targets: 2 }
        ],
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                if (column.index() === 1) { 


                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                }
            });
        }
    });
});

$(document).ready(function () {
    $('div.dataTables_filter input[type="search"]').css({ 'width': '120px' });
});