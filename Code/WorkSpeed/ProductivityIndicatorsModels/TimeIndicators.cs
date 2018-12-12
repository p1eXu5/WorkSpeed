using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class TimeIndicators : ProductivityIndicators
    {
        public ProductivityTimer GatheringTime = new ProductivityTimer();
        public ProductivityTimer ClientGatheringTime = new ProductivityTimer();
        public ProductivityTimer ScanningTime = new ProductivityTimer();
        public ProductivityTimer ClientScanningTime = new ProductivityTimer();
        public ProductivityTimer DefragmentationTime = new ProductivityTimer();
        public ProductivityTimer PlacingTime = new ProductivityTimer();
        public ProductivityTimer InventorizationTime = new ProductivityTimer();
        public ProductivityTimer ShipmentTime = new ProductivityTimer();
        public ProductivityTimer NonProductiveTime = new ProductivityTimer();

        public override string GetName()
        {
            throw new NotImplementedException();
        }
    }
}
