# lab-w10 Smart Home Mini Project

## 1. Vị trí sử dụng các Pattern & Lợi ích / Hạn chế

- **Factory Pattern (`DeviceFactory`)**
  - **Vị trí sử dụng:** Dùng để khởi tạo các thiết bị (`SmartLight`, `SmartFan`, `SmartDoor`) dựa vào chuỗi định danh truyền vào.
  - **Lợi ích:** Giúp client tách rời khỏi logic khởi tạo đối tượng cụ thể, dễ dàng mở rộng thêm thiết bị mới mà không ảnh hưởng tới code client.
  - **Hạn chế:** Làm tăng số lượng lớp trong hệ thống; khi thêm loại thiết bị mới bắt buộc phải sửa đổi code bên trong Factory.

- **Singleton Pattern (`HomeConfiguration`)**
  - **Vị trí sử dụng:** Quản lý thông tin cấu hình chung của ngôi nhà (tên nhà, mức sáng ban đêm).
  - **Lợi ích:** Đảm bảo toàn bộ ứng dụng chỉ tồn tại duy nhất một instance cấu hình, tiết kiệm bộ nguyên nguyên liệu và đồng bộ trạng thái.
  - **Hạn chế:** Gây khó khăn khi viết Unit Test do phụ thuộc trạng thái toàn cục (global state).

- **Facade Pattern (`SmartHomeFacade`)**
  - **Vị trí sử dụng:** Cung cấp các thao tác mức cao như `GoToWork()` và `GoToSleep()`.
  - **Lợi ích:** Giúp gom nhóm các tác vụ phức tạp liên quan đến nhiều thiết bị thành một lời gọi đơn giản cho client, giảm sự phụ thuộc trực tiếp.
  - **Hạn chế:** Nếu lạm dụng, lớp Facade có thể trở thành "God Object", ôm đồm quá nhiều logic nghiệp vụ của hệ thống.

## G, câu hỏi trả lời
## 1. Factory đang che giấu quyết định thiết kế nào?
- Che giấu logic khởi tạo đối tượng cụ thể (new SmartLight, SmartFan, SmartDoor),
  giúp client không cần biết lớp cụ thể nào được tạo ra mà chỉ thao tác qua IDevice.
## 2. Singleton bảo đảm điều gì, nhưng tạo ra rủi ro nào? 
- Bảo đảm: chỉ có duy nhất 1 instance
- Rủi ro: gây khó khăn khi viết unitest
## 3. Facade khác Factory ở mục đích chính như thế nào? 
- factory: dùng để tạo ra các đối tượng
- facade: dùng để cung cấp giao diện gom nhóm các thao tác chứa đối tượng có sẵn
## 4. Nếu bỏ SmartHomeFacade, client sẽ phải biết thêm những chi tiết nào?
- Client sẽ phải tự gọi và quản lý từng thiết bị riêng lẻ (tắt đèn, tắt quạt, khóa cửa, đọc cấu hình mức sáng)
 thay vì chỉ gọi một hàm gom nhóm duy nhất.
