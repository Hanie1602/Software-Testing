document.addEventListener('DOMContentLoaded', function () {
    const clinicModal = document.getElementById('clinicModal');

    clinicModal.addEventListener('show.bs.modal', function (event) {
        const button = event.relatedTarget;     //Nút kích hoạt Modal
        const clinicName = button.getAttribute('data-clinic-name');
        const clinicAddress = button.getAttribute('data-clinic-address');
        const clinicManager = button.getAttribute('data-clinic-manager');
        const clinicPhone = button.getAttribute('data-clinic-phone');
        const clinicEmail = button.getAttribute('data-clinic-email');
        const clinicProvince = button.getAttribute('data-clinic-province');
        const clinicDistrict = button.getAttribute('data-clinic-district');
        const clinicWard = button.getAttribute('data-clinic-ward');
        const clinicBasis = button.getAttribute('data-clinic-basis');
        const clinicDescription = button.getAttribute('data-clinic-description');
        const clinicImage = button.getAttribute('data-clinic-image');

        //Điền vào các fields
        document.getElementById('clinicName').textContent = clinicName;
        document.getElementById('clinicDescription').textContent = clinicDescription;
        /*document.getElementById('name').textContent = clinicName;*/
        document.getElementById('basis').textContent = clinicBasis;
        document.getElementById('managerName').textContent = clinicManager;
        document.getElementById('phone').textContent = clinicPhone;
        document.getElementById('email').textContent = clinicEmail;
        document.getElementById('address').textContent = clinicAddress;
        document.getElementById('province').textContent = clinicProvince;
        document.getElementById('district').textContent = clinicDistrict;
        document.getElementById('ward').textContent = clinicWard;
        document.getElementById('description').textContent = clinicDescription;

        if (clinicImage) {
            document.getElementById('clinicImage').src = clinicImage;
        } else {
            document.getElementById('clinicImage').src = 'https://eastrosedental.com/Content/Home/images/content/banner-left.jpg';
        }
    });
});
