namespace HomeApplianceRepairCenter.Enums
{
    /// <summary>
    /// مراحل صيانة الجهاز بالترتيب. الانتقال مسموح فقط للمرحلة التالية مباشرة.
    /// </summary>
    public enum RepairStatus
    {
        Received = 0,    // تم الاستلام
        Diagnosing = 1,  // قيد الفحص
        InProgress = 2,  // قيد الإصلاح
        Completed = 3,   // تم الإصلاح
        Delivered = 4    // تم التسليم للزبون
    }

    /// <summary>
    /// تحويل حالة الصيانة إلى نص للعرض في الشاشة.
    /// </summary>
    public static class RepairStatusExtensions
    {
        public static string ToDisplayText(this RepairStatus status)
        {
            switch (status)
            {
                case RepairStatus.Received: return "Received";
                case RepairStatus.Diagnosing: return "Diagnosing";
                case RepairStatus.InProgress: return "In Progress";
                case RepairStatus.Completed: return "Completed";
                case RepairStatus.Delivered: return "Delivered";
                default: return status.ToString();
            }
        }
    }
}
