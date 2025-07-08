var tbody = document.getElementById("myTableBody");
var pagination = document.getElementById("pagination");
var numRowsPerPage = 10; // số dòng trên mỗi trang
var currentPage = 1;

function updateTable() {
	var start = (currentPage - 1) * numRowsPerPage;
	var end = start + numRowsPerPage;

	for (var i = 0; i < tbody.rows.length; i++) {
		tbody.rows[i].style.display = i >= start && i < end ? "" : "none";
	}
}

function updatePagination() {
	var numPages = Math.ceil(tbody.rows.length / numRowsPerPage);

	pagination.innerHTML = ""; // xóa nội dung hiện tại

	// Thêm nút "Previous"
	var prevLi = document.createElement("li");
	prevLi.className = "page-item" + (currentPage === 1 ? " disabled" : "");
	var prevA = document.createElement("a");
	prevA.className = "page-link";
	prevA.href = "#";
	prevA.textContent = "Trước";
	prevA.addEventListener("click", function (e) {
		e.preventDefault();
		if (currentPage > 1) {
			currentPage--;
			updateTable();
			updatePagination();
		}
	});
	prevLi.appendChild(prevA);
	pagination.appendChild(prevLi);

	// Thêm các nút số trang
	var lastNumber = 0;
	for (var i = 1; i <= numPages; i++) {
		if (
			i == 1 ||
			i == numPages ||
			(i >= currentPage - 1 && i <= currentPage + 1)
		) {
			if (lastNumber != i - 1) {
				// Thêm nút "..."
				var li = document.createElement("li");
				li.className = "page-item disabled";
				var a = document.createElement("a");
				a.className = "page-link";
				a.href = "#";
				a.textContent = "...";
				li.appendChild(a);
				pagination.appendChild(li);
			}
			lastNumber = i;
			(function (i) {
				var li = document.createElement("li");
				li.className = "page-item" + (i === currentPage ? " active" : "");
				var a = document.createElement("a");
				a.className = "page-link";
				a.href = "#";
				a.textContent = i;
				a.addEventListener("click", function (e) {
					e.preventDefault();
					currentPage = i;
					updateTable();
					updatePagination();
				});
				li.appendChild(a);
				pagination.appendChild(li);
			})(i);
		}
	}

	// Thêm nút "Next"
	var nextLi = document.createElement("li");
	nextLi.className =
		"page-item" + (currentPage === numPages ? " disabled" : "");
	var nextA = document.createElement("a");
	nextA.className = "page-link";
	nextA.href = "#";
	nextA.textContent = "Sau";
	nextA.addEventListener("click", function (e) {
		e.preventDefault();
		if (currentPage < numPages) {
			currentPage++;
			updateTable();
			updatePagination();
		}
	});
	nextLi.appendChild(nextA);
	pagination.appendChild(nextLi);
}

updateTable();
updatePagination();
