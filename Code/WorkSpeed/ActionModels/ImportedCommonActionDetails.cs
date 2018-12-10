namespace WorkSpeed.ActionModels
{
    public abstract class ImportedCommonActionDetails : ImportedActionDetailsBase
    {
        public readonly ushort ProductId;

        protected ImportedCommonActionDetails(ushort productId)
        {
            ProductId = productId;
        }
    }
}
