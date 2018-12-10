namespace WorkSpeed.ActionModels
{
    public class ImportedShipmentActionDetails : ImportedActionDetailsBase
    {
        public readonly float WeightPerEmployee;
        public readonly float ClientCargoQuantity;
        public readonly float CommonCargoQuantity;

        public ImportedShipmentActionDetails (float weight, float clientCargoQuantity, float commonCargoQuantity)
        {
            WeightPerEmployee = weight;
            ClientCargoQuantity = clientCargoQuantity;
            CommonCargoQuantity = commonCargoQuantity;
        }
    }
}
