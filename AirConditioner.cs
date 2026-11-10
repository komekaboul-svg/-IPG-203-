using System;

namespace HomeApplianceRepairCenter.Models
{
    /// <summary>
    /// مكيف: التكلفة تتأثر بنوع المكيف (سبليت أو شباك) وبإعادة تعبئة الغاز.
    /// </summary>
    public class AirConditioner : Appliance
    {
        private const decimal BaseFee = 40m;
        private const decimal HourlyRate = 25m;
        private const decimal SplitUnitFee = 50m;
        private const decimal GasRefillFee = 45m;

        private readonly bool _isSplit;
        private readonly bool _needsGasRefill;

        public AirConditioner(string serialNumber, string ownerName, string brand,
            DateTime receivedDate, DateTime expectedDeliveryDate,
            bool isSplit, bool needsGasRefill)
            : base(serialNumber, ownerName, brand, receivedDate, expectedDeliveryDate)
        {
            _isSplit = isSplit;
            _needsGasRefill = needsGasRefill;
        }

        public bool IsSplit { get { return _isSplit; } }
        public bool NeedsGasRefill { get { return _needsGasRefill; } }

        public override string ApplianceType { get { return _isSplit ? "Split AC" : "Window AC"; } }

        public override decimal CalculateRepairCost()
        {
            decimal cost = BaseFee + (decimal)LaborHours * HourlyRate + PartsCost;
            if (_isSplit) cost += SplitUnitFee;
            if (_needsGasRefill) cost += GasRefillFee;
            return cost;
        }
    }
}
