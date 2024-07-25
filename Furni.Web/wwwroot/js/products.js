$(document).ready(function () {

    $('[data-kt-filter="search"]').on('keyup', function () {
        var input = $(this);
        datatable.search(input.val()).draw();
    });

    datatable = $('#Products').DataTable({
        serverSide: true,
        processing: true,
        stateSave: false,
        lengthMenu: [10, 20, 40],
        ajax: {
            url: '/Admin/Products/GetProducts', 
            type: 'POST'
        },
        order: [[1, 'asc']],
        'drawCallback': function () {
            KTMenu.createInstances();
        },
        columnDefs: [{ // Option that allows you to define specific configuration options for individual columns.
            targets: [0], // Column 0 is Id, we will unvisible and close search on it
            visible: false,
            searchable: false
        }],
        columns: [ // Array of columns that will rendered
            { "data": "id", "name": "Id", className: "d-none" },
            {
                "name": "Title",
                "render": function (data, type, row) {
                    return `<div class="d-flex align-items-center">
								<!--begin::Thumbnail-->
								<a href="/Admin/Products/Details/${row.id}" class="symbol symbol-50px">
									<span class="symbol-label" style="background-image:url(${row.mainImageThumbnailUrl});"></span>
								</a>
								<!--end::Thumbnail-->
								<div class="ms-5 d-flex flex-column">
									<!--begin::Title-->
									<a href="/Admin/Products/Details/${row.id}" class="text-gray-800 text-hover-primary fs-5 fw-bold mb-1">${row.title}</a>
                                    <span>${row.category}</span>
									<!--end::Title-->
								</div>
							</div>`;
                }
            },
            { "data": "quantity", "name": "Quantity" },
            {
                "name": "CostPrice",
                "render": function (data, type, row) {
                    return `<span>
                                    $${(row.costPrice)}
                            </span>`;
                }
            },
            {
                "name": "Price",
                "render": function (data, type, row) {
                    return `<span>
                                    $${(row.price)}
                            </span>`;
                }
            },
            { "data": "category", "name": "Category" },
            {
                "name": "createdOn", "render": function (data, type, row) {
                    return moment(row.publishingDate).format('YYYY MMMM, DD');
                }
            },
            {
                "name": "IsDeleted",
                "render": function (data, type, row) {
                    return `<span class="badge badge-light-${(row.isDeleted ? 'danger' : 'success')} js-status">
                                    ${(row.isDeleted ? 'InActicve' : 'Active')}
                            </span>`;
                }
            },
            {
                "className": 'text-end',
                "orderable": false,
                "render": function (data, type, row) {
                    return `<a href="#" class="btn btn-light btn-active-light-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                Actions
                                <span class="svg-icon fs-5 m-0">
                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                            <polygon points="0 0 24 0 24 24 0 24"></polygon>
                                            <path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
                                        </g>
                                    </svg>
                                </span>
                            </a>
                            <!--begin::Menu-->
                            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true">
                                <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="/Admin/Products/Edit/${row.id}" class="menu-link px-3">
                                        Edit
                                    </a>
                                </div>
                                <!--end::Menu item-->

                                <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="javascript:;" class="menu-link flex-stack px-3 js-toggle-status" data-url="/Admin/Products/ToggleStatus/${row.id}">
                                        Toggle Status
                                    </a>
                                </div>
                                <!--end::Menu item-->
                            </div>
                            <!--end::Menu-->`;
                }
            }

        ]
    });



    function store() {
        var text = document.getElementById("searchItem").value;
        localStorage.setItem("searchItem", text);
    }
    // Local storage to keep values after refresh
    function getValue() {
        var storedText = localStorage.getItem("searchItem");

        if (storedText != null) {
            document.getElementById("searchItem").value = storedText;
        }
        else
            document.getElementById("searchItem").value = "";
    }

    document.getElementById("searchItem").onkeyup = store;
    document.getElementById("searchItem").onload = getValue();

});