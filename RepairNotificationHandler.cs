namespace HomeApplianceRepairCenter.Events
{
    /// <summary>
    /// التفويض (Delegate) المسؤول عن الإشعار بتغير حالة الجهاز.
    /// </summary>
    /// <param name="sender">الجهاز الذي أطلق الحدث</param>
    /// <param name="e">تفاصيل التغير</param>
    public delegate void RepairNotificationHandler(object sender, RepairNotificationEventArgs e);
}
