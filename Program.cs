using System;
using System.Globalization;
using System.Text;
using HomeApplianceRepairCenter.Enums;
using HomeApplianceRepairCenter.Helpers;
using HomeApplianceRepairCenter.Models;
using HomeApplianceRepairCenter.Services;

namespace HomeApplianceRepairCenter
{
    /// <summary>
    /// محاكاة لعمل مركز صيانة الأجهزة المنزلية.
    /// </summary>
    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            DateTime today = DateTime.Today;

            RepairCenter center = new RepairCenter("Al-Noor Home Appliance Repair Center");
            Console.WriteLine($"Welcome to {center.Name}");

            // 1) استلام الأجهزة (ثلاث فئات مشتقة مختلفة)
            Console.WriteLine("\n--- Receiving appliances ---");
            Refrigerator fridge = new Refrigerator("RF-1001", "Ahmad Khaled", "Samsung",
                today.AddDays(-3), today.AddDays(4), hasFreezer: true, needsGasRefill: true);
            WashingMachine washer = new WashingMachine("WM-2002", "Sara Ali", "LG",
                today.AddDays(-10), today.AddDays(-2), capacityKg: 8, drumReplaced: true);
            AirConditioner ac = new AirConditioner("AC-3003", "Mohammad Hassan", "Gree",
                today.AddDays(-1), today.AddDays(3), isSplit: true, needsGasRefill: false);
            AirConditioner windowAc = new AirConditioner("AC-3004", "Layla Youssef", "Carrier",
                today, today.AddDays(5), isSplit: false, needsGasRefill: true);

            center.AddAppliance(fridge);
            center.AddAppliance(washer);
            center.AddAppliance(ac);
            center.AddAppliance(windowAc);

            // 2) إدخال بيانات الإصلاح
            fridge.LaborHours = 3;     fridge.PartsCost = 35m;
            washer.LaborHours = 4;     washer.PartsCost = 80m;
            ac.LaborHours = 2.5;       ac.PartsCost = 20m;
            windowAc.LaborHours = 1.5; windowAc.PartsCost = 10m;

            center.DisplayAll("Appliances after receiving");

            // 3) التحقق من البيانات (فئة ثابتة)
            Console.WriteLine("\n--- Data validation ---");
            TryAddInvalid(center, "123", "Rana Saeed");        // رقم تسلسلي خاطئ
            TryAddInvalid(center, "RF-9009", "R2D2");          // اسم غير صالح

            // 4) فحص التأخر (يُطلق حدث DeliveryOverdue للغسالة)
            Console.WriteLine("\n--- Overdue check ---");
            int overdue = center.CheckOverdueAppliances(today);
            Console.WriteLine($"Overdue appliances: {overdue}");

            // 5) سير عملية الصيانة (تُطلق أحداث)
            Console.WriteLine("\n--- Repair workflow ---");
            fridge.StartDiagnosis();
            fridge.StartRepair();
            fridge.CompleteRepair();      // حدث: اكتمال الإصلاح
            fridge.Deliver();

            washer.StartDiagnosis();
            washer.StartRepair();
            washer.CompleteRepair();

            ac.StartDiagnosis();
            ac.StartRepair();             // ما زال قيد الإصلاح

            // محاولة انتقال غير مسموح (تخطي مرحلة)
            try
            {
                windowAc.CompleteRepair();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Expected error: {ex.Message}");
            }

            // محاولة تعديل بيانات بعد اكتمال الإصلاح
            try
            {
                fridge.PartsCost = 999m;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Expected error: {ex.Message}");
            }

            // 6) التقارير (تعدد الأشكال + خصائص ثابتة)
            center.DisplayAll("All appliances");
            center.Display(center.GetUnderRepair(), "Appliances still under repair");
            center.Display(center.GetRepaired(), "Repaired appliances");

            Console.WriteLine("\n===== Statistics =====");
            Console.WriteLine($"Total appliances: {RepairStatistics.TotalAppliances}");
            Console.WriteLine($"Still under repair: {RepairStatistics.StillUnderRepairCount}");
            Console.WriteLine($"Repaired: {RepairStatistics.RepairedCount}");
            Console.WriteLine($"Currently in progress: {RepairStatistics.CountByStatus(RepairStatus.InProgress)}");
            Console.WriteLine($"Total cost of all appliances: ${center.CalculateTotalCost():F2}");
            Console.WriteLine($"Center revenue (finished repairs): ${RepairStatistics.TotalRevenue:F2}");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // يجرب إنشاء جهاز ببيانات غير صالحة ويعرض رسالة الخطأ
        private static void TryAddInvalid(RepairCenter center, string serial, string owner)
        {
            DateTime today = DateTime.Today;
            try
            {
                center.AddAppliance(new Refrigerator(serial, owner, "Test", today, today.AddDays(2), false, false));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input rejected: {ex.Message}");
            }
        }
    }
}
