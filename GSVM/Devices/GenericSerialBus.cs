﻿namespace GSVM.Devices
{
    public class GenericSerialBus : GenericDeviceBus<GenericSerialRequest>
    {
        public GenericSerialBus()
        {
            GenerateID();
        }

        public override void ClockTick()
        {
            base.ClockTick();

            ReadyToRead = true;
            ReadyToWrite = false;
        }

        protected override bool InterruptChannelOk(uint channel)
        {
            return true;
        }
    }
}
