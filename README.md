# Software Testing Project

## 📚 Môn học: Software Testing

Đây là dự án cá nhân trong môn học *Software Testing*. Dự án được tổ chức theo các thư mục để học và thực hành các công cụ, kỹ thuật kiểm thử phần mềm hiện đại như **CodeScene**, **NUnit**, và xây dựng bài lab kiểm thử với source thực tế.

---

## 📁 Cấu trúc thư mục chính

- `.git/`, `.github/`: Cấu hình Git và GitHub.
- `Sample Document/`: Chứa các tài liệu mẫu, bài tham khảo kỹ thuật kiểm thử phần mềm
- `Slides/`: Tài liệu bài giảng môn học.
- `Tester/`: Thư mục chính để thực hiện và lưu trữ các công cụ kiểm thử như CodeScene và NUnit.
  - `CodeScene/`: Phân tích chất lượng mã nguồn qua công cụ CodeScene.
  - `NUnit/`: Kiểm thử đơn vị sử dụng NUnit.

---

## 🛠️ Công cụ sử dụng

### ✅ NUnit
- NUnit được sử dụng để viết và chạy các bài test đơn vị cho các class trong project.
- Dự án mẫu là `UniteTestingTutorial`, với class kiểm thử `BankAccount.cs`.

### 📊 CodeScene
- Đã được sử dụng để phân tích mã nguồn của dự án **Dental_Clinic_System**.
- CodeScene giúp xác định các điểm nóng (hotspots) trong code, độ phức tạp của code, và đề xuất cải tiến.

---

## 🚀 Hướng dẫn chạy dự án

### Chạy NUnit tests
1. Mở `UniteTestingTutorial.sln` bằng Visual Studio.
2. Build solution.
3. Vào menu **Test > Run All Tests** hoặc sử dụng cửa sổ Test Explorer.

### Phân tích với CodeScene
1. Đưa source code của dự án `Dental_Clinic_System` lên GitHub (hoặc Git repo).
2. Truy cập [CodeScene](https://codescene.io) và cấu hình repository.
3. Chạy phân tích và xem báo cáo đánh giá code.

---

## 📌 Ghi chú

- Mọi phần kiểm thử và phân tích đều được thực hiện thủ công trong khuôn khổ môn học.
- Các thư mục, file mẫu là phục vụ học tập và không triển khai đầy đủ tính năng như trong sản phẩm thực tế.