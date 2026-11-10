using System;
using System.Globalization;
using HomeApplianceRepairCenter.Enums;
using HomeApplianceRepairCenter.Events;
using HomeApplianceRepairCenter.Helpers;
using HomeApplianceRepairCenter.Interfaces;

namespace HomeApplianceRepairCenter.Models
{
    /// <summary>
    /// الفئة المجردة لجميع الأجهزة المنزلية. تطبق الواجهة IRepairable
    /// وتحتوي الخصائص المشتركة ودالة حساب التكلفة المجردة.
    /// </summary>
    public abstract class Appliance : IRepairable
    {
        // ===== الحقول الخاصة (Encapsulation) =====
        private readonly string _serialNumber;      // للقراءة فقط بعد الإنشاء
        private readonly DateTime _receivedDate;    // للقراءة فقط بعد الإنشاء
        private string _ownerName;
        private string _brand;
        private DateTime _expectedDeliveryDate;
        private DateTime? _completedDate;
        private RepairStatus _status;
        private double _laborHours;
        private decimal _partsCost;
        private bool _overdueNotified;

        /// <summary>الحدث الذي يُطلق عند تغير الحالة أو تأخر التسليم.</summary>
        public event RepairNotificationHandler RepairNotification;

        protected Appliance(string serialNumber, string ownerName, string brand,
            DateTime receivedDate, DateTime expectedDeliveryDate)
        {
            if (!ValidationHelper.IsValidSerialNumber(serialNumber))
            {
                throw new ArgumentException("Invalid serial number. Expected format like RF-1001.", nameof(serialNumber));
            }
            if (!ValidationHelper.IsValidName(ownerName))
            {
                throw new ArgumentException("Invalid owner name.", nameof(ownerName));
            }
            if (string.IsNullOrWhiteSpace(brand))
            {
                throw new ArgumentException("Brand is required.", nameof(brand));
            }
            if (!ValidationHelper.IsValidDeliveryDate(receivedDate, expectedDeliveryDate))
            {
                throw new ArgumentException("Delivery date cannot be earlier than the received date.", nameof(expectedDeliveryDate));
            }

            _serialNumber = serialNumber;
            _ownerName = ownerName.Trim();
            _brand = brand.Trim();
            _receivedDate = receivedDate.Date;
            _expectedDeliveryDate = expectedDeliveryDate.Date;
            _status = RepairStatus.Received;
        }

        // ===== الخصائص (Properties) =====
        public string SerialNumber { get { return _serialNumber; } }
        public DateTime ReceivedDate { get { return _receivedDate; } }
        public RepairStatus Status { get { return _status; } }
        public DateTime? CompletedDate { get { return _completedDate; } }
        public string Brand { get { return _brand; } }

        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                if (!ValidationHelper.IsValidName(value))
                {
                    throw new ArgumentException("Invalid owner name.");
                }
                _ownerName = value.Trim();
            }
        }

        public DateTime ExpectedDeliveryDate
        {
            get { return _expectedDeliveryDate; }
            set
            {
                EnsureEditable();
                if (!ValidationHelper.IsValidDeliveryDate(_receivedDate, value))
                {
                    throw new ArgumentException("Delivery date cannot be earlier than the received date.");
                }
                _expectedDeliveryDate = value.Date;
                _overdueNotified = false; // تغيير الموعد يسمح بإشعار جديد عند التأخر
            }
        }

        public double LaborHours
        {
            get { return _laborHours; }
            set
            {
                EnsureEditable();
                if (!ValidationHelper.IsNonNegative(value))
                {
                    throw new ArgumentException("Labor hours cannot be negative.");
                }
                _laborHours = value;
            }
        }

        public decimal PartsCost
        {
            get { return _partsCost; }
            set
            {
                EnsureEditable();
                if (!ValidationHelper.IsNonNegative(value))
                {
                    throw new ArgumentException("Parts cost cannot be negative.");
                }
                _partsCost = value;
            }
        }

        /// <summary>اسم نوع الجهاز (ثلاجة، غسالة، مكيف...).</summary>
        public abstract string ApplianceType { get; }

        /// <summary>تكلفة الإصلاح: لكل نوع جهاز طريقة حساب خاصة به.</summary>
        public abstract decimal CalculateRepairCost();

        // ===== عمليات الصيانة =====
        public void StartDiagnosis()
        {
            ChangeStatus(RepairStatus.Diagnosing, "Diagnosis started.");
        }

        public void StartRepair()
        {
            ChangeStatus(RepairStatus.InProgress, "Repair started.");
        }

        public void CompleteRepair()
        {
            ChangeStatus(RepairStatus.Completed, "Repair completed, ready for delivery.");
        }

        public void Deliver()
        {
            ChangeStatus(RepairStatus.Delivered, "Delivered to the customer.");
        }

        /// <summary>
        /// يفحص تأخر موعد التسليم، ويُطلق حدثا (مرة واحدة) إذا كان الجهاز متأخرا.
        /// </summary>
        public bool CheckOverdue(DateTime today)
        {
            bool isOverdue = _status < RepairStatus.Completed && today.Date > _expectedDeliveryDate;

            if (isOverdue && !_overdueNotified)
            {
                _overdueNotified = true;
                int days = (today.Date - _expectedDeliveryDate).Days;
                OnRepairNotification(new RepairNotificationEventArgs(this, _status, _status,
                    NotificationType.DeliveryOverdue, $"Delivery is {days} day(s) overdue."));
            }
            return isOverdue;
        }

        public virtual string GetStatusSummary()
        {
            // InvariantCulture لتثبيت التقويم الميلادي بغض النظر عن لغة الجهاز
            string due = _expectedDeliveryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string cost = CalculateRepairCost().ToString("F2", CultureInfo.InvariantCulture);

            return $"[{ApplianceType}] {_brand} | {_serialNumber} | Owner: {_ownerName} | " +
                   $"Status: {_status.ToDisplayText()} | Due: {due} | Cost: ${cost}";
        }

        // ===== دوال داخلية =====
        private void ChangeStatus(RepairStatus newStatus, string message)
        {
            // الانتقال مسموح فقط للمرحلة التالية مباشرة
            if (newStatus != _status + 1)
            {
                throw new InvalidOperationException(
                    $"Invalid transition from '{_status.ToDisplayText()}' to '{newStatus.ToDisplayText()}'.");
            }

            RepairStatus oldStatus = _status;
            _status = newStatus;

            NotificationType type = NotificationType.StatusChanged;
            if (newStatus == RepairStatus.Completed)
            {
                _completedDate = DateTime.Today;
                type = NotificationType.RepairCompleted;
            }

            OnRepairNotification(new RepairNotificationEventArgs(this, oldStatus, newStatus, type, message));
        }

        // لا يمكن تعديل التكلفة والموعد بعد اكتمال الإصلاح
        private void EnsureEditable()
        {
            if (_status >= RepairStatus.Completed)
            {
                throw new InvalidOperationException("Cost and delivery date cannot be modified after the repair is completed.");
            }
        }

        protected virtual void OnRepairNotification(RepairNotificationEventArgs e)
        {
            RepairNotification?.Invoke(this, e);
        }
    }
}
