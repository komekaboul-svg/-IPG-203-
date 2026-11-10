using System;

namespace HomeApplianceRepairCenter.Models
{
    /// <summary>
    /// ثلاجة: التكلفة تتأثر بوجود فريزر وبحاجة الجهاز لإعادة تعبئة الغاز.
    /// </summary>
    public class Refrigerator : Appliance
    {
        private const decimal BaseFee = 50m;
        private const decimal HourlyRate = 20m;
        private const decimal FreezerFee = 30m;
        private const decimal GasRefillFee = 40m;

        private readonly bool _hasFreezer;
        private readonly bool _needsGasRefill;

        public Refrigerator(string serialNumber, string ownerName, string brand,
            DateTime receivedDate, DateTime expectedDeliveryDate,
            bool hasFreezer, bool needsGasRefill)
            : base(serialNumber, ownerName, brand, receivedDate, expectedDeliveryDate)
        {
            _hasFreezer = hasFreezer;
            _needsGasRefill = needsGasRefill;
        }

        public bool HasFreezer { get { return _hasFreezer; } }
        public bool NeedsGasRefill { get { return _needsGasRefill; } }

        public override string ApplianceType { get { return "Refrigerator"; } }

        public override decimal CalculateRepairCost()
        {
            decimal cost = BaseFee + (decimal)LaborHours * HourlyRate + PartsCost;
            if (_hasFreezer) cost += FreezerFee;
            if (_needsGasRefill) cost += GasRefillFee;
            return cost;
        }
    }
}
