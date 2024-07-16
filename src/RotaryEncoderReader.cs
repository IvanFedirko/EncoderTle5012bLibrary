
using System;
using System.Data;
using System.Device.Spi;
using System.IO;

namespace FIV.Tle5012b
{
    public class RotaryEncoder
    {
        private SpiDevice _spi;

        public RotaryEncoder(int busId =0, int cs =0)
        {
      


            var spiConf = new SpiConnectionSettings(busId, cs); // SPI = 0, CS = 0
            spiConf.ChipSelectLineActiveState = 0; //Активное состояние чип селекта 0
            spiConf.ClockFrequency = 80000; // 80Mhz частота
            spiConf.Mode = SpiMode.Mode1; //Полярность устанавливается на низком, и выборка данных осуществляется по заднему фронту тактового сигнала.


            _spi = SpiDevice.Create(spiConf);
        }

        public bool Cmd_8021(out double angle, int round = 2)
        {

            byte[] writeBuffer = new byte[6] { 0x80, 0x21, 0x00, 0x00, 0x00, 0x00 }; //пишем 6ть байт
            byte[] readBuffer = new byte[6]; //читаем 6ть байт
            _spi.TransferFullDuplex(writeBuffer, readBuffer); //Пишем и читаем

            ushort angleData = BitConverter.ToUInt16(new byte[2] { readBuffer[3], readBuffer[2] }); //данные угла
            ushort maskAngleData = 0x7FFF; //выделяем нужные биты
            double del = 360 / 32768.0; // 360 град 15 бит(32768 точек)
            angle = Math.Round((angleData & maskAngleData) * del, round); //округляем до 2х знаков после запятой

            ushort safetyWord = BitConverter.ToUInt16(new byte[2] { readBuffer[5], readBuffer[4] }); //получаем  проверочное слово

            safetyWord = (ushort)(safetyWord & 0x0fff); //выделяем нужную информацию по маске

            //заполняем массив для проверки полученных значений методом контрольной суммы
            byte[] crcCheckArray = new byte[6];
            crcCheckArray[0] = 0x80;
            crcCheckArray[1] = 0x21;
            crcCheckArray[2] = (byte)(angleData >> 8);
            crcCheckArray[3] = (byte)(angleData & 0xff);


            var crc8 = Crc8.Calculate(crcCheckArray, 4);

            return crc8 == (safetyWord & 0xff);

        }

    }
}



