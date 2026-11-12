using System;
using HomeApplianceRepairCenter.Enums;
using HomeApplianceRepairCenter.Models;

namespace HomeApplianceRepairCenter.Events
{
    /// <summary>
    /// نوع الإشعار الذي يطلقه الجهاز.
    /// </summary>
    public enum NotificationType
    {
        StatusChanged,    // تغير عادي في الحالة
        RepairCompleted,  // اكتمال الإصلاح
        DeliveryOverdue   // تأخر موعد التسليم
    }

    /// <summary>
    /// بيانات الحدث: الجهاز المعني والحالة القديمة والجديدة ونوع الإشعار.
    /// </summary>
    public class RepairNotificationEventArgs : EventArgs
    {
        public Appliance Appliance { get; }
        public RepairStatus OldStatus { get; }
        public RepairStatus NewStatus { get; }
        public NotificationType Type { get; }
        public string Message { get; }

        public RepairNotificationEventArgs(Appliance appliance, RepairStatus oldStatus,
            RepairStatus newStatus, NotificationType type, string message)
        {
            Appliance = appliance;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Type = type;
            Message = message;
        }
    }
}
