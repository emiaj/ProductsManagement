$(document).ready(function() {
    $("#grid").jqGrid(
        {
            url: "/products/grid/data",
            autowidth: true,
            autoencode : true,
            datatype: "json",
            pager: "#pager",
            prmNames: { page: "Page", search: null, nd: null, rows: "Rows", sort: null, order: null },
            jsonReader:
                {
                    repeatitems: false,
                    records: "Records",
                    root: "Rows",
                    total: "Total",
                    id: "Id",
                    page: "Page"
                },
            colNames: [
                "Id", "Name", "Description", "Quantity"
            ],
            colModel:
                [
                    { name: "Id", index: "Id", sortable: false },
                    { name: "Name", index: "Name", sortable: false },
                    { name: "Description", index: "Description", sortable: false },
                    { name: "Quantity", index: "Quantity", sortable: false }
                ]
        })
        .navGrid("#pager", { search: false, add: false, edit: false, del: false, refresh: false })
        .navButtonAdd("#pager", { caption: "Add", title: $.jgrid.nav.addtitle, buttonicon: 'ui-icon ui-icon-plus',
            onClickButton: function () {
                    window.location.href = "/products/add";
            }
        })
        .navButtonAdd("#pager", { caption: "Edit", title: $.jgrid.nav.edittitle, buttonicon: 'ui-icon ui-icon-pencil',
            onClickButton: function () {
                var id = $("#grid").getGridParam("selrow");
                if (id) {
                    window.location.href = "/products/edit/" + id;
                }
                else {
                    alert($.jgrid.nav.alerttext);
                }
            }
        })
        .navButtonAdd("#pager", { caption: "Delete", title: $.jgrid.nav.deltitle, buttonicon: 'ui-icon ui-icon-trash',
            onClickButton: function () {
                var id = $("#grid").getGridParam("selrow");
                if (id) {
                    window.location.href = "/products/delete/" + id;
                }
                else {
                    alert($.jgrid.nav.alerttext);
                }
            }
        });
});