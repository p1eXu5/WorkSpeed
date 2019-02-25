using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.Productivity
{
    public class ShipmentProductivityViewModel : ProductivityViewModel
    {
        public ShipmentProductivityViewModel ( IProductivity productivity, Operation operation )
            : base( operation )
        {
            SpeedLabeling = SPEED_IN_VOLUMES;
            Speed = productivity.GetTotalVolume();

            (double client, double nonClient) = productivity.GetCargoQuantity();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( new [] {
                    (client, $"Клиентские: {client}"),
                    (nonClient, $"Не клиентские: {nonClient}"),
                }),
                Annotation = "сканн"
            } );

            Next();
        }
    }
}
