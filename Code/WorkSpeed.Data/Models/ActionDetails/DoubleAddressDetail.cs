﻿

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class DoubleAddressDetail : ActionDetailBase
    {
        public Address SenderCellAdress { get; set; }
        public Address ReceiverCellAdress { get; set; }
    }
}