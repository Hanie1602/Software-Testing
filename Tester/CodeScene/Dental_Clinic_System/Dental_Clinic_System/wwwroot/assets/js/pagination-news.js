document.addEventListener("DOMContentLoaded", function () {
	var newsItems = document.querySelectorAll(
		".newPost__wrapper .newsItem__wrapper"
	);
	var pagination = document.getElementById("pagination");
	var numRowsPerPage = 2; // Số phần tử trên mỗi trang
	var currentPage = 1;

	function updateNewsItemsDisplay() {
		var start = (currentPage - 1) * numRowsPerPage;
		var end = start + numRowsPerPage;

		newsItems.forEach((item, index) => {
			item.style.display = index >= start && index < end ? "" : "none";
		});
	}

	function updatePagination() {
		var numPages = Math.ceil(newsItems.length / numRowsPerPage);

		pagination.innerHTML = ""; // Xóa nội dung hiện tại của phân trang

		// Thêm nút "Previous"
		var prevLi = document.createElement("li");
		prevLi.className = "page-item" + (currentPage === 1 ? " disabled" : "");
		var prevA = document.createElement("a");
		prevA.className = "page-link";
		prevA.href = "#";

		// Create the icon element
		var icon = document.createElement("i");
		icon.className = "fa-solid fa-arrow-left";

		// Append the icon to the anchor element
		prevA.appendChild(icon);

		prevA.addEventListener("click", function (e) {
			e.preventDefault();
			if (currentPage > 1) {
				currentPage--;
				updateNewsItemsDisplay();
				updatePagination();

				// Apply animation to newsItem__wrapper elements
				document
					.querySelectorAll(".newsItem__wrapper")
					.forEach(function (element) {
						element.style.animation = "prevPage 0.5s ease-out forwards";
					});
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
						// Determine the animation direction based on the clicked page number
						var animationName = i > currentPage ? "nextPage" : "prevPage";
						currentPage = i;
						updateNewsItemsDisplay();
						updatePagination();

						// Apply the determined animation to all newsItem__wrapper elements
						document
							.querySelectorAll(".newsItem__wrapper")
							.forEach(function (element) {
								element.style.animation = `${animationName} 0.5s ease-out forwards`;
							});
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

		// Create the icon element for the "Next" button
		var nextIcon = document.createElement("i");
		nextIcon.className = "fa-solid fa-arrow-right";

		// Append the icon to the anchor element
		nextA.appendChild(nextIcon);

		nextA.addEventListener("click", function (e) {
			e.preventDefault();
			if (currentPage < numPages) {
				currentPage++;
				updateNewsItemsDisplay();
				updatePagination();

				// Apply animation to newsItem__wrapper elements
				document
					.querySelectorAll(".newsItem__wrapper")
					.forEach(function (element) {
						element.style.animation = "nextPage 0.5s ease-out forwards";
					});
			}
		});
		nextLi.appendChild(nextA);
		pagination.appendChild(nextLi);
	}

	updateNewsItemsDisplay();
	updatePagination();
});
