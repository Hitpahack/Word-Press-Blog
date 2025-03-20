// postManager.js
const appFun = {
    core: {
        generateDateFilter: function (startYear, elementid) {
            const currentDate = new Date();
            const currentYear = currentDate.getFullYear();
            const currentMonth = currentDate.getMonth() + 1; // Months are 0-based in JS

            let select = document.getElementById(elementid);

            // Add "All dates" option
            let allOption = document.createElement("option");
            allOption.value = "";
            allOption.selected = true;
            allOption.textContent = "All dates";
            select.appendChild(allOption);

            // Generate options from startYear to currentYear
            for (let year = currentYear; year >= startYear; year--) {
                let startMonth = year === currentYear ? currentMonth : 12; // Ensure only past months are included for the current year
                for (let month = startMonth; month >= 1; month--) {
                    let option = document.createElement("option");
                    let monthValue = `${year}${month.toString().padStart(2, "0")}`;
                    option.value = monthValue;
                    option.textContent = new Date(year, month - 1).toLocaleString("en-US", {
                        month: "long",
                        year: "numeric",
                    });
                    select.appendChild(option);
                }
            }
        },
        postReq: (url, reqdata, callbackOnSuccess) => {
            $.post(url, reqdata, (response) => {
                if (response.data) {
                    if (typeof callbackOnSuccess !== "undefined" && callbackOnSuccess) {
                        callbackOnSuccess(response);
                    }
                }
            }).fail(function (xhr, status, error) {
                alert("Error: " + xhr.responseText);
            });
        },
        delete: (url, reqdata, callbackOnSuccess) => {
            Swal.fire({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    appFun.core.postReq(url, reqdata, callbackOnSuccess);
                }
            });
        }
    },

    // Initialize the DataTable with filters and other features
    posts: {
        $dtTable: null,
        $dtFilters: ['#dateFilter, #categoryFilter, #rankFilter'],
        dt_post_list_datatable: (url) => {
            $dtTable = $('#post_list_datatable').DataTable({
                "processing": true,
                "serverSide": true,
                searching: false,
                drawCallback: function (settings) {

                },
                "ajax": {
                    "url": url,
                    "type": "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", // Expect JSON response
                    "data": function (d) {
                        d.search.value = $('#post-search-input').val();
                        d.categoryId = $('#categoryFilter').val() || '';
                        d.rankMathFilter = $('#rankFilter').val() || '';
                        d.date = $('#dateFilter').val() || '';
                        return JSON.stringify(d);
                    }
                },
                columnDefs: [
                    { targets: 1, width: "320px" }, // Adjust width for the "publishedDate" column (assuming it's the first column)
                    { targets: 3, width: "100px" }, // Adjust width for the "publishedDate" column (assuming it's the first column)
                    { targets: 4, width: "280px" }, // Adjust width for the "publishedDate" column (assuming it's the first column)
                    { targets: 5, width: "80px" }, // Adjust width for the "publishedDate" column (assuming it's the first column)
                    { targets: 6, width: "120px" } // Adjust width for the "publishedDate" column (assuming it's the first column)
                ],
                "columns": [
                    {
                        "data": "id", "render": function (data) {
                            return `<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />`;
                        }
                    },
                    {
                        "data": "post_Title", "render": function (val, column, data) {
                            return `<a href="/Posts/AddPost?post=${data.id}">${val}</a>
                                <div class="row-actions">
                                    <span class="edit">
                                        <a href="/Posts/AddPost?post=${data.id}" aria-label="">Edit</a>
                                    |</span>
                                    <span class="inline hide-if-no-js">
                                        <button type="button" class="button-link editinline" aria-label="Quick Edit">Quick Edit</button> |
                                    </span>
                                    <span class="trash">
                                        <a href="javascript:void(0);" class="submitdelete" data-id="${data.id}" aria-label="">Trash</a> |
                                    </span>
                                    <span class="view">
                                        <a href="" rel="" aria-label="">Preview</a>
                                    </span>
                                </div>`;
                        }
                    },
                    {
                        "data": "categories", "render": function (data) {
                            if (!data) return "";
                            return data.split(',').map(category => `<a href="javascript:void(0);" onclick="appFun.posts.dtTableSearch('${category}')" class="cat-bdge">${category}</a>`).join(", ");
                        }
                    },
                    { "data": "user_Login", "render": (data) => `<a href="javascript:void(0);" onclick="appFun.posts.dtTableSearch('${data}')" >${data}</a>` },
                    {
                        "data": "tags", "render": function (data) {
                            if (!data) return "";
                            return data.split(',').map(tag => `<a href="javascript:void(0);" onclick="appFun.posts.dtTableSearch('${tag}')" class="tag-bdge">${tag}</a>`).join(", ");
                        }
                    },
                    {
                        "data": "post_Status", "render": function (data) {
                            switch (data.toLowerCase()) {
                                case "publish":
                                    return '<span class="badge bg-success">Published</span>';
                                case "draft":
                                    return '<span class="badge bg-warning text-dark">Draft</span>';
                                case "inherit":
                                    return '<span class="badge bg-info">Inherited</span>';
                                case "trash":
                                    return '<span class="badge bg-danger">Trashed</span>';
                                default:
                                    return '<span class="badge bg-secondary">Unknown</span>';
                            }
                        }
                    },
                    {
                        "data": "post_Date",
                        "render": function (data) {
                            if (!data) return ''; // Handle empty/null values
                            let date = new Date(data);

                            // Extract components
                            let year = date.getFullYear();
                            let month = (date.getMonth() + 1).toString().padStart(2, '0'); // Ensure two digits
                            let day = date.getDate().toString().padStart(2, '0');
                            let hours = date.getHours();
                            let minutes = date.getMinutes().toString().padStart(2, '0');
                            let ampm = hours >= 12 ? 'pm' : 'am';
                            hours = hours % 12 || 12; // Convert 24-hour format to 12-hour

                            // Format final output
                            return `Last Modified ${year}/${month}/${day} at ${hours}:${minutes} ${ampm}`;
                        }
                    },
                    {
                        "data": "id", "render": function (data) {
                            return ``;
                        }
                    }
                ]
            });
            $(appFun.posts.$dtFilters.join(', ')).on('change', function () {
                $dtTable.ajax.reload(null, false);
            });

            $('#search-submit').on('click', function () {
                $dtTable.ajax.reload(null, false);
            });
            $('#resetFilter').on('click', function () {
                $(appFun.posts.$dtFilters.concat('#post-search-input').join(', ')).each(function () {
                    if ($(this).is('select')) {
                        $(this).prop('selectedIndex', 0); // Reset dropdown to first option
                    } else {
                        $(this).val(''); // Clear input fields
                    }
                });
                $dtTable.ajax.reload(null, false); // Reload DataTable without resetting pagination
            });
        },
        trash_post: (url, reqdata) => {

            Swal.fire({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    $.post(url, reqdata, function (response) {
                        if (response.data) {
                            Swal.fire({
                                title: "Deleted!",
                                text: "Your Post has been deleted.",
                                icon: "success"
                            });
                            $dtTable.ajax.reload(null, false);
                        }
                        else {
                            console.error(response.message);
                        }
                    }).fail(function (xhr, status, error) {
                        alert("Error: " + xhr.responseText);
                    });

                }
            });
        },
        bulkactions: (elm, url) => {
            var $action = $('#bulk-action-selector-top').val();
            var ids = [...document.querySelectorAll('.item_checkbox:checked')].map(s => parseInt(s.value)); // Get selected checkboxes

            if ($action === 'trash') {
                appFun.core.delete(url, { selectedIds: ids }, (response) => { $dtTable.ajax.reload(null, false); });
            }
            else if ($action === 'edit') {
                if (ids.length == 1) {
                    window.location.href = "/posts/addpost?post=" + ids;
                } else {
                    alert("Please select only one post to edit.");
                }
            }
        },
        trash_post: (url, reqData) => {
            appFun.core.delete(url, reqData, (response) => { $dtTable.ajax.reload(null, false); })
        },
        dtTableSearch: (search_val) => {
            $('#post-search-input').val(search_val);
            //$dtTable.search(search_val).draw();
            $dtTable.ajax.reload();
        },
        dtReload: () => $dtTable.ajax.reload(null, false)
    },
    users: {
        $dtTable: null,
        dt_user_list_datatable: (url) => {
            $dtTable = $('#user_list_datatable').DataTable({
                "processing": true,
                "serverSide": true,
                searching: false,
                "ajax": {
                    "url": url,
                    "type": "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", // Expect JSON response
                    "data": function (d) {
                        d.search.value = $('#post-search-input').val();
                        d.date = $('#dateFilter').val() || '';
                        return JSON.stringify(d);
                    }
                },
                columnDefs: [
                    { targets: 4, width: "150px" } // Adjust width for the "publishedDate" column (assuming it's the first column)
                ],
                "columns": [
                    {
                        "data": "id", "render": function (val, colum, data) {
                            return `  <input  class="item_checkbox" id="${val}" type="checkbox" value="${val}" /> `;
                        }
                    },
                    {
                        "data": "user_Login", "render": function (val, colum, data) {

                            return `<img width="50" src="${data.avatar}">
                            <a href="#">${val}</a>
                                                    <div class="row-actions">
                                                        <span class="edit">
                                                                                    <a href="/Users/EditUser?user=${data.id}" aria-label="">Edit</a>
                                                            |
                                                        </span>
                                                       <span class="delete">
																					<a href="/Users/DeleteUser?user=${data.id}" aria-label="">Delete</a>
															|
														</span>
                                                       
                                                        <span class="view">
                                                            <a href="" rel="" aria-label="">Preview</a>
                                                        </span>
                                                                 <span class="2fa">
                                                                    <a href="" class="submitdelete" aria-label="">
                                                                        2FA
                                                                    </a> |
                                                                </span>
                                                    </div>`;
                        }
                    },
                    { "data": "display_Name" },
                    { "data": "user_Email" },
                    { "data": "role" },
                    { "data": "total_Posts" },
                    { "data": null, "defaultContent": "" }, // Blank Column
                    { "data": null, "defaultContent": "" } // Blank Column

                ]
            });
            $('#search-submit').on('click', function () {
                $dtTable.ajax.reload(null, false);
            });
            $('#post-query-submit').on('click', function () {
                $dtTable.ajax.reload(null, false);
            });
        },
        trash_user: (url, reqData) => {
            appFun.core.delete(url, reqData, (response) => { $dtTable.ajax.reload(null, false); })
        },
        bulkactions: (elm, url) => {
            var $action = $('#bulk-action-selector-top').val();
            var ids = [...document.querySelectorAll('.item_checkbox:checked')].map(s => parseInt(s.value)); // Get selected checkboxes

            if ($action === 'trash') {
                if (ids.length == 0) {
                    alert("Please select user to delete.");
                    return;
                }
                else {
                    window.location.href = "/users/deleteuser?user=" + ids.join(",");
                }
                //appFun.core.delete(url, { selectedIds: ids }, (response) => { $dtTable.ajax.reload(null, false); });
            }
            else if ($action === 'edit') {
                if (ids.length == 1) {
                    window.location.href = "/users/edituser?user=" + ids;
                } else {
                    alert("Please select only one user to edit.");
                }
            }
        },
    },
    comments: {
        $dtTable: null,
        dt_comment_list_datatable: (url) => {
            $dtTable = $('#comment_list_datatable').DataTable({
                "processing": true,
                "serverSide": true,
                searching: false,
                "ajax": {
                    "url": url,
                    "type": "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", // Expect JSON response
                    "data": function (d) {
                        d.search.value = $('#post-search-input').val();
                        return JSON.stringify(d);
                    }
                },
                columnDefs: [
                    { targets: 4, width: "150px" } // Adjust width for the "publishedDate" column (assuming it's the first column)
                ],
                "columns": [
                    {   
                        "data": "Comment_Id", "render": function (val, colum, data) {
                            return `  <input class="item_checkbox" id="${data.comment_Id}" type="checkbox" value="${data.comment_Id}" /> `;
                        }
                    },
                    {
                        "data": "Comment_Author", "render": function (val, colum, data) {
                            return `<img src=${data.avatar} width="50" class="rounded-circle">
                                        <br>
                                        <strong>${data.comment_Author}</strong>
                                        <br>
                                        <a href="mailto:${data.comment_Author_Email}">${data.comment_Author_Email}</a>`
                        }
                    },
                    {
                        "data": "Comment_Content", "render": function (val, colum, data) {
                            let statusText = data.comment_Approved === "approved" ? "Unapprove" : "Approve";
                            let statusValue = data.comment_Approved === "approved" ? "unapproved" : "approved";
                            return ` 
                                        <div class="comment-box">
                                            ${data.comment_Content.length > 200
                                                                        ? data.comment_Content.substring(0, 250) + ' <b>.... more</b>'
                                                                        : data.comment_Content}
                                        </div>
                                        <div class="comment-actions mt-1">
                                        <br/>
                                            <a href="javascript:void(0);" class="updatestatus" data-id="${data.comment_Id}" data-status="${statusValue}">
                                                ${statusText}
                                            </a> |
                                            <a href="#">Reply</a> | 
                                            <a href="/edit?comment=${data.comment_Id}">Quick Edit</a> | 
                                            <a href="/edit?comment=${data.comment_Id}">Edit</a> | 
                                            <a href="javascript:void(0);" class="updatestatus" data-id="${data.comment_Id}" data-status="spammed">Spam</a> |
                                            <a href="javascript:void(0);" class="updatestatus" data-id="${data.comment_Id}" data-status="trashed">Trash</a>
                                        </div>`

                        }
                    },
                    {
                        "data": "display_Name", "render": function (val, colum, data) {
                            return `<a href="${data.post_Title}">${data.post_Title}</a>
                                <br>
                                <a href="${data.post_Title}">View Post</a>`
                        }
                    },
                    { "data": "comment_Date_Gmt" },
                ]
            });
            $('#search-submit').on('click', function () {
                $dtTable.ajax.reload(null, false);
            });
            $('#post-query-submit').on('click', function () {
                $dtTable.ajax.reload(null, false);
            });

        },
        update_status: (url, reqdata) => {
            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(reqdata),
                success: function (response) {
                    if (response.data) {
                        if ($.fn.DataTable.isDataTable("#comment_list_datatable")) {
                            let dt = $("#comment_list_datatable").DataTable();
                            console.log("Reloading DataTable..."); // Debugging
                            dt.ajax.reload(null, false);
                        } else {
                            console.warn("DataTable not initialized, reloading page...");
                            location.reload();
                        }
                    }
                },
                error: function (xhr) {
                    console.error("Error:", xhr.responseText);
                }
            });
        },
        bulkactions: (elm, url) => {
            var $action = $('#bulk-action-selector-top').val(); // Get selected action
            var ids = [...document.querySelectorAll('.item_checkbox:checked')].map(s => parseInt(s.value)); // Get selected checkboxes

            if (!$action) { // Corrected condition
                alert("Please select an action!");
                return;
            }
            if (ids.length === 0) {
                alert("Please select at least one comment!");
                return;
            }
            appFun.comments.update_status(url, { CommentIds: ids, Status: $action }); // Ensure function call is correct
        }
    }
};


