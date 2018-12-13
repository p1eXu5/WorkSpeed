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
        private readonly IBreakConstraints _breakConstraints;

        private ProductivityTimer _gatheringTime = new ProductivityTimer();
        private ProductivityTimer _clientGatheringTime = new ProductivityTimer();
        private ProductivityTimer _scanningTime = new ProductivityTimer();
        private ProductivityTimer _clientScanningTime = new ProductivityTimer();
        private ProductivityTimer _defragmentationTime = new ProductivityTimer();
        private ProductivityTimer _placingTime = new ProductivityTimer();
        private ProductivityTimer _inventoryTime = new ProductivityTimer();
        private ProductivityTimer _shipmentTime = new ProductivityTimer();
        private ProductivityTimer _nonProductivTime = new ProductivityTimer();

        public TimeIndicators ( string name )
            : base( name )
        {
            _breakConstraints = BreakConstraints.DefaultBreakConstraints;
        }

        public TimeIndicators ( string name, IBreakConstraints breakConstraints )
            : this( name )
        {
            _breakConstraints = breakConstraints ?? throw new ArgumentNullException();
        }

        public ProductivityTimer GatheringTime
        {
            get => _gatheringTime;
            set => _gatheringTime = _breakConstraints.TryModify( value );
        }

        public ProductivityTimer ClientGatheringTime
        {
            get => _clientGatheringTime;
            set => _clientGatheringTime = _breakConstraints.TryModify( value );
        }

        public ProductivityTimer ScanningTime
        {
            get => _scanningTime;
            set => _scanningTime = _breakConstraints.TryModify( value );
        }

        public ProductivityTimer ClientScanningTime
        {
            get => _clientScanningTime;
            set => _clientScanningTime = _breakConstraints.TryModify( value );
        }

        public ProductivityTimer DefragmentationTime
        {
            get => _defragmentationTime;
            set => _defragmentationTime = _breakConstraints.TryModify( value );
        }

        public ProductivityTimer PlacingTime
        {
            get => _placingTime;
            set => _placingTime = _breakConstraints.TryModify( value );
        }

        public ProductivityTimer InventoryTime
        {
            get => _inventoryTime;
            set => _inventoryTime = _breakConstraints.TryModify( value );
        }

        public ProductivityTimer ShipmentTime
        {
            get => _shipmentTime;
            set => _shipmentTime = _breakConstraints.TryModify( value );
        }

        public ProductivityTimer NonProductivTime
        {
            get => _nonProductivTime;
            set => _nonProductivTime = _breakConstraints.TryModify( value );
        }

    }
}
