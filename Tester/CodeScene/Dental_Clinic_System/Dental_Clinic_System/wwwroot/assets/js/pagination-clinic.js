var itemsContainer = document.getElementById("myItem");
var pagination = document.getElementById("pagination");
var itemsPerPage = 4; // number of items per page
var currentPage = 1;

function updateItems() {
	var start = (currentPage - 1) * itemsPerPage;
	var end = start + itemsPerPage;

	for (var i = 0; i < itemsContainer.children.length; i++) {
		itemsContainer.children[i].style.display =
			i >= start && i < end ? "" : "none";
	}
}

function updatePagination() {
	var numPages = Math.ceil(itemsContainer.children.length / itemsPerPage);

	pagination.innerHTML = ""; // clear current content

	// Add "Previous" button
	var prevLi = document.createElement("li");
	prevLi.className = "page-item" + (currentPage === 1 ? " disabled" : "");
	var prevA = document.createElement("a");
	prevA.className = "page-link";
	prevA.href = "#";
	prevA.textContent = "Previous";
	prevA.addEventListener("click", function (e) {
		e.preventDefault();
		if (currentPage > 1) {
			currentPage--;
			updateItems();
			updatePagination();
		}
	});
	prevLi.appendChild(prevA);
	pagination.appendChild(prevLi);

	// Add page number buttons
	var lastNumber = 0;
	for (var i = 1; i <= numPages; i++) {
		if (
			i == 1 ||
			i == numPages ||
			(i >= currentPage - 1 && i <= currentPage + 1)
		) {
			if (lastNumber != i - 1) {
				// Add "..." button
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
					updateItems();
					updatePagination();
				});
				li.appendChild(a);
				pagination.appendChild(li);
			})(i);
		}
	}

	// Add "Next" button
	var nextLi = document.createElement("li");
	nextLi.className =
		"page-item" + (currentPage === numPages ? " disabled" : "");
	var nextA = document.createElement("a");
	nextA.className = "page-link";
	nextA.href = "#";
	nextA.textContent = "Next";
	nextA.addEventListener("click", function (e) {
		e.preventDefault();
		if (currentPage < numPages) {
			currentPage++;
			updateItems();
			updatePagination();
		}
	});
	nextLi.appendChild(nextA);
	pagination.appendChild(nextLi);
}

updateItems();
updatePagination();
