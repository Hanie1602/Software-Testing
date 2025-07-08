$(document).ready(function () {
	//Lấy tỉnh thành
	$.getJSON('https://esgoo.net/api-tinhthanh/1/0.htm', function (data_tinh) {
		if (data_tinh.error == 0) {
			$.each(data_tinh.data, function (key_tinh, val_tinh) {
				$("#tinh").append('<option value="' + val_tinh.id + '" data-fullname="' + val_tinh.full_name + '">' + val_tinh.full_name + '</option>');
			});
			$("#tinh").change(function (e) {
				var selectedOption = $(this).find('option:selected');
				var idtinh = selectedOption.val();
				var fullnameTinh = selectedOption.data('fullname');
				$('#Province').val(fullnameTinh);
				console.log(fullnameTinh);

				//Lấy quận huyện
				$.getJSON('https://esgoo.net/api-tinhthanh/2/' + idtinh + '.htm', function (data_quan) {
					if (data_quan.error == 0) {
						$("#quan").html('<option value="0">Quận Huyện</option>');
						$("#phuong").html('<option value="0">Phường Xã</option>');
						$.each(data_quan.data, function (key_quan, val_quan) {
							$("#quan").append('<option value="' + val_quan.id + '" data-fullname="' + val_quan.full_name + '">' + val_quan.full_name + '</option>');
						});
						//Lấy phường xã
						$("#quan").change(function (e) {
							var selectedOption = $(this).find('option:selected');
							var idquan = selectedOption.val();
							var fullnameQuan = selectedOption.data('fullname');
							$('#District').val(fullnameQuan);

							$.getJSON('https://esgoo.net/api-tinhthanh/3/' + idquan + '.htm', function (data_phuong) {
								if (data_phuong.error == 0) {
									$("#phuong").html('<option value="0">Phường Xã</option>');
									$.each(data_phuong.data, function (key_phuong, val_phuong) {
										$("#phuong").append('<option value="' + val_phuong.id + '" data-fullname="' + val_phuong.full_name + '">' + val_phuong.full_name + '</option>');
									});

									// Set hidden input for Ward on change
									$("#phuong").change(function (e) {
										var selectedOption = $(this).find('option:selected');
										var fullnamePhuong = selectedOption.data('fullname');
										$('#Ward').val(fullnamePhuong);
									});
								}
							});
						});

					}
				});
			});

		}
	});
});