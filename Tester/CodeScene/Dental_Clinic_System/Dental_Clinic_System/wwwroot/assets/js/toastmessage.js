// toasts function
function toasts({ title = "", message = "", type = "info", duration = 3000 }) {
    const main = document.getElementById("toasts");
    if (main) {
        const toasts = document.createElement("div");

        // Auto remove toasts
        const autoRemoveId = setTimeout(function () {
            main.removeChild(toasts);
        }, duration + 1000);

        // Remove toasts when clicked
        toasts.onclick = function (e) {
            if (e.target.closest(".toasts__close")) {
                main.removeChild(toasts);
                clearTimeout(autoRemoveId);
            }
        };

        const icons = {
            success: "fas fa-check-circle",
            info: "fas fa-info-circle",
            warning: "fas fa-exclamation-circle",
            error: "fas fa-exclamation-circle"
        };
        const icon = icons[type];
        const delay = (duration / 1000).toFixed(2);

        toasts.classList.add("toasts", `toasts--${type}`);
        toasts.style.animation = `slideInLeft ease .3s, fadeOut linear 1s ${delay}s forwards`;

        toasts.innerHTML = `
                    <div class="toasts__icon">
                        <i class="${icon}"></i>
                    </div>
                    <div class="toasts__body">
                        <h3 class="toasts__title">${title}</h3>
                        <p class="toasts__msg">${message}</p>
                    </div>
                    <div class="toasts__close">
                        <i class="fas fa-times"></i>
                    </div>
                `;
        main.appendChild(toasts);
    }
}

function showSuccessToasts(message) {
    toasts({
        title: "Thành công!",
        message: message,
        type: "success",
        duration: 5000
    });
}

function showErrorToasts(message) {
    toasts({
        title: "Thất bại!",
        message: message,
        type: "error",
        duration: 5000
    });
}
