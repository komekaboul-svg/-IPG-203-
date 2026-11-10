using System;

namespace HomeApplianceRepairCenter.Models
{
    /// <summary>
    /// غسالة: التكلفة تتأثر بسعة الغسالة وباستبدال الأسطوانة (الدرام).
    /// </summary>
    public class WashingMachine : Appliance
    {
        private const decimal BaseFee = 30m;
        private const decimal HourlyRate = 15m;
        private const decimal DrumReplacementFee = 60m;
        private const decimal PerKgFee = 2m;

        private readonly double _capacityKg;
        private readonly bool _drumReplaced;

        public WashingMachine(string serialNumber, string ownerName, string brand,
            DateTime receivedDate, DateTime expectedDeliveryDate,
            double capacityKg, bool drumReplaced)
            : base(serialNumber, ownerName, brand, receivedDate, expectedDeliveryDate)
        {
            if (capacityKg <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.", nameof(capacityKg));
            }
            _capacityKg = capacityKg;
            _drumReplaced = drumReplaced;
        }

        public double CapacityKg { get { return _capacityKg; } }
        public bool DrumReplaced { get { return _drumReplaced; } }

        public override string ApplianceType { get { return "Washing Machine"; } }

        public override decimal CalculateRepairCost()
        {
            decimal cost = BaseFee + (decimal)LaborHours * HourlyRate + PartsCost
                           + (decimal)_capacityKg * PerKgFee;
            if (_drumReplaced) cost += DrumReplacementFee;
            return cost;
        }
    }
}
