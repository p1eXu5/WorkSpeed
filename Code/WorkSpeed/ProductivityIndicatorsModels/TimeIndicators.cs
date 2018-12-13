using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Constraints;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityCalculator;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class TimeIndicators : ProductivityIndicators
    {
        private readonly ITimeConstraint _timeConstraint;

        private ProductivityTime _gatheringTime = new ProductivityTime();
        private ProductivityTime _clientGatheringTime = new ProductivityTime();
        private ProductivityTime _shopperGatheringTime = new ProductivityTime();
        private ProductivityTime _scanningTime = new ProductivityTime();
        private ProductivityTime _clientScanningTime = new ProductivityTime();
        private ProductivityTime _defragmentationTime = new ProductivityTime();
        private ProductivityTime _placingTime = new ProductivityTime();
        private ProductivityTime _inventoryTime = new ProductivityTime();
        private ProductivityTime _shipmentTime = new ProductivityTime();
        private ProductivityTime _nonProductivTime = new ProductivityTime();

        public TimeIndicators ( string name, ITimeConstraint timeConstraint )
            : base( name )
        {
            _timeConstraint = timeConstraint ?? throw new ArgumentNullException();
        }

        public ProductivityTime GatheringTime
        {
            get => _gatheringTime;
            set => _gatheringTime += _timeConstraint.GetProductivityTime ( value, _gatheringTime );
        }

        public ProductivityTime ClientGatheringTime
        {
            get => _clientGatheringTime;
            set => _clientGatheringTime = _timeConstraint.GetProductivityTime( value, _clientGatheringTime );
        }

        public ProductivityTime ShopperGatheringTime
        {
            get => _shopperGatheringTime;
            set => _shopperGatheringTime = _timeConstraint.GetProductivityTime( value, _shopperGatheringTime );
        }

        public ProductivityTime ScanningTime
        {
            get => _scanningTime;
            set => _scanningTime = _timeConstraint.GetProductivityTime( value, _scanningTime );
        }

        public ProductivityTime ClientScanningTime
        {
            get => _clientScanningTime;
            set => _clientScanningTime = _timeConstraint.GetProductivityTime( value, _clientScanningTime );
        }

        public ProductivityTime DefragmentationTime
        {
            get => _defragmentationTime;
            set => _defragmentationTime = _timeConstraint.GetProductivityTime( value, _defragmentationTime );
        }

        public ProductivityTime PlacingTime
        {
            get => _placingTime;
            set => _placingTime = _timeConstraint.GetProductivityTime( value, _placingTime );
        }

        public ProductivityTime InventoryTime
        {
            get => _inventoryTime;
            set => _inventoryTime = _timeConstraint.GetProductivityTime( value, _inventoryTime );
        }

        public ProductivityTime ShipmentTime
        {
            get => _shipmentTime;
            set => _shipmentTime = _timeConstraint.GetProductivityTime( value,_shipmentTime );
        }

        public ProductivityTime NonProductivTime
        {
            get => _nonProductivTime;
            set => _nonProductivTime = _timeConstraint.GetProductivityTime( value, _nonProductivTime );
        }

    }
}
