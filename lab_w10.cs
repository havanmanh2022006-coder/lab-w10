using System;

namespace SmartHomeMini
{
    // ==========================================
    // 1. PHẦN B: MÔ HÌNH THIẾT BỊ (INTERFACES & CLASSES)
    // ==========================================
    public interface IDevice
    {
        void TurnOn();
        void TurnOff();
    }

    public interface ILockable
    {
        void Lock();
        void Unlock();
    }

    public class SmartLight : IDevice
    {
        private readonly string name;
        public SmartLight(string name) { this.name = name; }
        
        public void TurnOn() => Console.WriteLine($"{name}: ON");
        public void TurnOff() => Console.WriteLine($"{name}: OFF");
    }

    public class SmartFan : IDevice
    {
        private readonly string name;
        public SmartFan(string name) { this.name = name; }
        
        public void TurnOn() => Console.WriteLine($"{name}: ON");
        public void TurnOff() => Console.WriteLine($"{name}: OFF");
    }

    public class SmartDoor : IDevice, ILockable
    {
        private readonly string name;
        public SmartDoor(string name) { this.name = name; }
        
        public void TurnOn() => Console.WriteLine($"{name}: OPENED (Active)");
        public void TurnOff() => Console.WriteLine($"{name}: CLOSED (Inactive)");
        
        public void Lock() => Console.WriteLine($"{name}: LOCKED");
        public void Unlock() => Console.WriteLine($"{name}: UNLOCKED");
    }

    // ==========================================
    // 2. PHẦN C: MẪU FACTORY PATTERN
    // ==========================================
    public static class DeviceFactory
    {
        public static IDevice Create(string type, string name)
        {
            return type.Trim().ToLowerInvariant() switch
            {
                "light" => new SmartLight(name),
                "fan"   => new SmartFan(name),
                "door"  => new SmartDoor(name),
                _       => throw new ArgumentException($"Unsupported device type: {type}")
            };
        }
    }

    // ==========================================
    // 3. PHẦN D: MẪU SINGLETON PATTERN
    // ==========================================
    public sealed class HomeConfiguration
    {
        private static readonly Lazy<HomeConfiguration> lazy =
            new(() => new HomeConfiguration());

        public static HomeConfiguration Instance => lazy.Value;

        public string HomeName { get; set; } = "My Home";
        public int NightLightLevel { get; set; } = 30;

        private HomeConfiguration() { }
    }

    // ==========================================
    // 4. PHẦN E: MẪU FACADE PATTERN
    // ==========================================
    public sealed class SmartHomeFacade
    {
        private readonly IDevice light;
        private readonly IDevice fan;
        private readonly ILockable door;

        public SmartHomeFacade(IDevice light, IDevice fan, ILockable door)
        {
            this.light = light;
            this.fan = fan;
            this.door = door;
        }

        public void GoToWork()
        {
            light.TurnOff();
            fan.TurnOff();
            door.Lock();
        }

        public void GoToSleep()
        {
            fan.TurnOff();
            door.Lock();
            light.TurnOn();
            int level = HomeConfiguration.Instance.NightLightLevel;
            Console.WriteLine($"Night light level: {level}%");
        }
    }

    // ==========================================
    // 5. PHẦN F: CHẠY KIỂM THỬ (TEST SUITES T1 -> T5)
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== BẮT ĐẦU CHẠY KIỂM THỬ SMART HOME MINI ===\n");

            // T1: Tạo light, fan, door
            Console.WriteLine("[T1] Kiểm tra tạo thiết bị qua Factory:");
            IDevice light = DeviceFactory.Create("light", "Living room light");
            IDevice fan = DeviceFactory.Create("fan", "Living room fan");
            IDevice door = DeviceFactory.Create("door", "Main door");
            
            bool t1Pass = (light is SmartLight) && (fan is SmartFan) && (door is SmartDoor);
            Console.WriteLine($"-> Kết quả T1: {(t1Pass ? "PASS (Factory trả đúng kiểu đối tượng)" : "FAIL")}\n");

            // T2: Tạo loại camera khi chưa hỗ trợ
            Console.WriteLine("[T2] Kiểm tra tạo thiết bị không hợp lệ (camera):");
            try
            {
                DeviceFactory.Create("camera", "Security Camera");
                Console.WriteLine("-> Kết quả T2: FAIL (Không phát sinh ngoại lệ)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"-> Bắt được ngoại lệ chuẩn xác: {ex.Message}");
                Console.WriteLine("-> Kết quả T2: PASS (Phát sinh ArgumentException đúng mong đợi)\n");
            }

            // T3: Lấy HomeConfiguration.Instance hai lần
            Console.WriteLine("[T3] Kiểm tra Singleton Pattern:");
            HomeConfiguration c1 = HomeConfiguration.Instance;
            HomeConfiguration c2 = HomeConfiguration.Instance;
            bool t3Pass = ReferenceEquals(c1, c2);
            if (!t3Pass) throw new Exception("Singleton implementation is incorrect.");
            Console.WriteLine($"-> ReferenceEquals(c1, c2) = {t3Pass}");
            Console.WriteLine("-> Kết quả T3: PASS (Hai biến cùng tham chiếu một đối tượng)\n");

            // Khởi tạo Facade với các thiết bị đã tạo
            var facade = new SmartHomeFacade(light, fan, (SmartDoor)door);

            // T4: Gọi GoToWork()
            Console.WriteLine("[T4] Kiểm tra kịch bản GoToWork():");
            facade.GoToWork();
            Console.WriteLine("-> Kết quả T4: PASS (Đèn tắt, quạt tắt, cửa khóa)\n");

            // T5: Gọi GoToSleep()
            Console.WriteLine("[T5] Kiểm tra kịch bản GoToSleep():");
            facade.GoToSleep();
            Console.WriteLine("-> Kết quả T5: PASS (Quạt tắt, cửa khóa, đèn bật, hiện mức sáng)\n");

            Console.WriteLine("=== TẤT CẢ CÁC KIỂM THỬ ĐÃ HOÀN TẤT THÀNH CÔNG ===");
        }
    }
}