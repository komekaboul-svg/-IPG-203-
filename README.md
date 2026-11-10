# نظام إدارة مركز صيانة وإصلاح الأجهزة المنزلية

وظيفة تطبيقية - مقرر البرمجة والتصميم غرضية التوجه (TIC_IPG203_F25) - الجامعة الافتراضية السورية.

نظام C# (.NET 8) يدير أجهزة منزلية (ثلاجة، غسالة، مكيف) وعمليات صيانتها وتكلفة إصلاحها.

## التشغيل
افتح `HomeApplianceRepairCenter.sln` في Visual Studio 2022 (مع .NET 8 SDK) ثم اضغط F5،
أو من الطرفية: `dotnet run --project HomeApplianceRepairCenter`

## هيكل المشروع
| المجلد | المحتوى |
|---|---|
| Interfaces | `IRepairable` |
| Models | `Appliance` (مجردة)، `Refrigerator`، `WashingMachine`، `AirConditioner` |
| Services | `RepairCenter` (الفئة الرئيسية) |
| Events | `RepairNotificationHandler` (Delegate)، `RepairNotificationEventArgs` |
| Helpers | `ValidationHelper` و `RepairStatistics` (فئات ثابتة) |
| Enums | `RepairStatus` |

## المبادئ المطبقة
- **التجريد:** `IRepairable` + `Appliance` مع الدالة المجردة `CalculateRepairCost()`.
- **الوراثة وتعدد الأشكال:** ثلاث فئات فرعية تعيد تعريف الدالة، و`RepairCenter` يعالج `List<Appliance>`.
- **التغليف:** حقول خاصة + خصائص، و`SerialNumber` و`ReceivedDate` للقراءة فقط، وقفل التعديل بعد اكتمال الإصلاح.
- **التفويض والأحداث:** `RepairNotificationHandler` والحدث `RepairNotification` (تغير الحالة، اكتمال الإصلاح، تأخر التسليم).
- **الأعضاء الثابتة:** `ValidationHelper` (تحقق) و`RepairStatistics` (خصائص ثابتة للعدد والإيراد).
