using System;
using System.Text.RegularExpressions;

namespace HomeApplianceRepairCenter.Helpers
{
    /// <summary>
    /// فئة ثابتة للتحقق من صحة البيانات المدخلة قبل إنشاء الأجهزة.
    /// </summary>
    public static class ValidationHelper
    {
        // الرقم التسلسلي: حرفان إنجليزيان كبيران + شرطة + 4 إلى 8 أرقام (مثال: RF-1001)
        private static readonly Regex SerialPattern = new Regex(@"^[A-Z]{2}-\d{4,8}$");

        public static bool IsValidSerialNumber(string serial)
        {
            return !string.IsNullOrWhiteSpace(serial) && SerialPattern.IsMatch(serial);
        }

        /// <summary>اسم صالح: حرفان على الأقل، أحرف ومسافات فقط (يدعم العربية).</summary>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 2)
            {
                return false;
            }

            foreach (char c in name)
            {
                if (!char.IsLetter(c) && c != ' ')
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>موعد التسليم يجب ألا يسبق تاريخ الاستلام.</summary>
        public static bool IsValidDeliveryDate(DateTime receivedDate, DateTime expectedDate)
        {
            return expectedDate.Date >= receivedDate.Date;
        }

        public static bool IsNonNegative(decimal value)
        {
            return value >= 0;
        }

        public static bool IsNonNegative(double value)
        {
            return value >= 0;
        }
    }
}
