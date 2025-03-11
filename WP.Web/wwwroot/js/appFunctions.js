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
        bulkactions: (elm, url) => {
            var $action = $('#bulk-action-selector-top').val();
            var ids = [...document.querySelectorAll('.item_checkbox:checked')].map(s => parseInt(s.value)); // Get selected checkboxes
            console.log(ids);
            if ($action === 'trash') {
                if (confirm("Are you sure you want to move selected items to trash?")) {
                    $.post(url, { selectedIds: ids }, function (response) {
                        if (response) {
                            alert("Selected posts moved to trash successfully!");
                            window.location.href = "/posts/index";
                        }
                        else {
                            console.error(response.message);
                        }
                    }).fail(function (xhr, status, error) {
                        alert("Error: " + xhr.responseText);
                    });

                }
            }
            else if ($action === 'edit') {
                if (ids.length == 1) {
                    window.location.href = "/posts/addpost?post=" + ids;
                } else {
                    alert("Please select only one post to edit.");
                }
            }


        }
    },

    // Initialize the DataTable with filters and other features
    posts: {
        $dtTable:null,
        dt_post_list_datatable: (url) => {
            $dtTable = $('#post_list_datatable').DataTable({
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
                        d.categoryId = $('#categoryFilter').val() || '';
                        d.rankMathFilter = $('#rankFilter').val() || '';
                        d.date = $('#dateFilter').val() || '';
                        return JSON.stringify(d);
                    }
                },
                columnDefs: [
                    { targets: 4, width: "150px" } // Adjust width for the "publishedDate" column (assuming it's the first column)
                ],
                "columns": [
                    {
                        "data": "id", "render": function (data) {
                            return `<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />`;
                        }
                    },
                    {
                        "data": "post_Title", "render": function (val, column, data) {
                            return `<a href="#">${val}</a>
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
                            return data.split(',').map(category => `<a href="javascript:void(0);" class="cat-bdge">${category}</a>`).join(", ");
                        }
                    },
                    { "data": "user_Login" },
                    {
                        "data": "tags", "render": function (data) {
                            if (!data) return "";
                            return data.split(',').map(tag => `<a href="javascript:void(0);" class="tag-bdge">${tag}</a>`).join(", ");
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
            $('#search-submit').on('click', function () {
                $dtTable.ajax.reload(null, false);
            });
            $('#filterButton').on('click', function () {
                $dtTable.ajax.reload(null, false);
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


           
        }
    },
    users: {
        $dtTable:null,
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
                            return `  <input id="${val}" type="checkbox" value="${val}" /> `;
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

        }
    }
};


