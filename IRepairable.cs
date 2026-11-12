using HomeApplianceRepairCenter.Enums;
using HomeApplianceRepairCenter.Events;

namespace HomeApplianceRepairCenter.Interfaces
{
    /// <summary>
    /// العمليات الأساسية المشتركة بين جميع الأجهزة داخل مركز الصيانة.
    /// </summary>
    public interface IRepairable
    {
        string SerialNumber { get; }
        RepairStatus Status { get; }

        /// <summary>يُطلق عند تغير حالة الصيانة أو تأخر التسليم.</summary>
        event RepairNotificationHandler RepairNotification;

        void StartDiagnosis();
        void StartRepair();
        void CompleteRepair();
        void Deliver();

        decimal CalculateRepairCost();
        string GetStatusSummary();
    }
}
