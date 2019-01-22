

namespace WorkSpeed.Data.Models
{
    public enum BoxTypes : byte
    {
        GatheringCell,
        StorageCell,
        DynamicGathering,
        DynamicPlacing,
        ClientGatheringCell
    }

    public static class BoxTypeExtension
    {
        public static string ToString ( this BoxTypes boxType )
        {
            switch ( boxType ) {
                case BoxTypes.GatheringCell: return "Быстрый набор";
                case BoxTypes.StorageCell: return "Хранение";
                case BoxTypes.DynamicGathering: return "Динамическая ячейка (подбор)";
                case BoxTypes.DynamicPlacing: return "Динамическая ячейка";
                case BoxTypes.ClientGatheringCell: return "Динамическая ячейка предварительный подбор";
            }

            return "";
        }
    }
}
