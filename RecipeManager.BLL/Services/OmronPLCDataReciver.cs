using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.BLL.Interfaces;
using RecipeManager.BLL.Entities.ReciveEntities;
using RecipeManager.DAL.Interfaces;
using OmronFinsTCP.Net;
using System.Net.NetworkInformation;

namespace RecipeManager.BLL.Services
{
    public class OmronPLCDataReciver : IPLCDataReciver,IDisposable
    {
        const int START_CELL = 25000;
        const short COUNT_CELL = 24649;
        const short CLEAR_CELL = 24650;


        const short YEAR_MONTH_START_CELL = 25000;
        const short YEAR_MONTH_END_CELL = 25079;
        const short YEAR_MONTH_MAX_CELL_IN_RECIPE = 1;

        const short DAY_HOUR_START_CELL = 25080;
        const short DAY_HOUR_END_CELL = 25159;
        const short DAY_HOUR_MAX_CELL_IN_RECIPE = 1;

        const short MINUTE_SECOND_START_CELL = 25160;
        const short MINUTE_SECOND_END_CELL = 25239;
        const short MINUTE_SECOND_MAX_CELL_IN_RECIPE = 1;

        const short NAMES_START_CELL = 25240;
        const short NAMES_END_CELL = 25879;
        const short NAMES_MAX_CELL_IN_RECIPE = 8;

        const short CODES_START_CELL = 25880;
        const short CODES_END_CELL = 27239;
        const short CODES_MAX_CELL_IN_RECIPE = 17;

        const short WEIGHTS_START_CELL = 27240;
        const short WEIGHTS_END_CELL = 29799;
        const short WEIGHTS_MAX_CELL_IN_RECIPE =32;

        const short WATER_START_CELL = 29800;
        const short WATER_END_CELL = 29959;
        const short WATER_MAX_CELL_IN_RECIPE = 2;

        const short TEMPER_START_CELL = 29960;
        const short TEMPER_END_CELL = 30039;
        const short TEMPER_MAX_CELL_IN_RECIPE = 1;

        const short STEP = 190;
        const short MAX_CELL = 500;
        
        private static short COUNT_DATAROWS = 190;//количество ячеек для считывания в одном рецепте.
        private short PACKEGES = (short)(500 / COUNT_DATAROWS);//количество пакетов, которое можем считать за один раз (до 500 ячеек в одной передаче данных).

        private readonly string _ipAddress = null;
        private short _cellPointer = 0;
        private EtherNetPLC _etherNetPLC = null;
        private PlcMemory _plcMemory = PlcMemory.DM;

        public OmronPLCDataReciver(string ipAddress)
        {
            _ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));

        }

       
        public (bool IsError, string Message, ReciveCodes CodesArray) GetDataWithResult()
        {
            var arrList = new List<short[]>();
            var bufferCodes = new ReciveCodes();
            short count = 0;
            short total = 0, mod = 0;
            short[] data=new short[190];
            
            var connectResult = ConnectInit();
            if (!connectResult.IsError)
            {
                _etherNetPLC.ReadWord(_plcMemory, COUNT_CELL, out count);

                if (count > 0)
                {
                    bufferCodes.YearMonth.AddRange(DownloadYearMonth(count));
                    bufferCodes.DayHour.AddRange(DownloadDayHour(count));
                    bufferCodes.MinuteSecond.AddRange(DownloadMinuteSecond(count));
                    bufferCodes.RecipesNames.AddRange(DownloadNames(count));
                    bufferCodes.ComponentsCodes.AddRange(DownloadCodes(count));
                    bufferCodes.ComponentsWeights.AddRange(DownloadWeights(count));
                    bufferCodes.WaterWeights.AddRange(DownloadWater(count));
                    bufferCodes.WaterTempers.AddRange(DownloadTemper(count));
                }
                _etherNetPLC.Close();
                return (false, "", bufferCodes);
            }

            else
            {
                _etherNetPLC.Close();
                return (true, connectResult.Message, null);
            }

        }

        #region DownloadStatistic

        private List<short> DownloadYearMonth(int recipeCount)
        {

            int div = recipeCount * YEAR_MONTH_MAX_CELL_IN_RECIPE / MAX_CELL;//целая часть
            int mod = recipeCount * YEAR_MONTH_MAX_CELL_IN_RECIPE % MAX_CELL;//остаток

            var list = new List<short>();

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    short from = (short)(YEAR_MONTH_START_CELL + ((i * MAX_CELL)));
                    short count = (short)(MAX_CELL)-1;

                    list.AddRange(GetCodes(from, count));
                }
            }

            if (mod > 0)
            {
                short from = (short)(YEAR_MONTH_START_CELL + (div * MAX_CELL));
                short count =(short)(mod);

                list.AddRange(GetCodes(from,count));
            }

            return list;

        }

        private List<short> DownloadDayHour(int recipeCount)
        {

            int div = recipeCount * DAY_HOUR_MAX_CELL_IN_RECIPE / MAX_CELL;//целая часть
            int mod = recipeCount * DAY_HOUR_MAX_CELL_IN_RECIPE % MAX_CELL;//остаток

            var list = new List<short>();

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    short from = (short)(DAY_HOUR_START_CELL + ((i * MAX_CELL)));
                    short count = (short)(MAX_CELL) - 1;

                    list.AddRange(GetCodes(from, count));
                }
            }

            if (mod > 0)
            {
                short from = (short)(DAY_HOUR_START_CELL + (div * MAX_CELL));
                short count = (short)(mod);

                list.AddRange(GetCodes(from, count));
            }

            return list;

        }

        private List<short> DownloadMinuteSecond(int recipeCount)
        {

            int div = recipeCount * MINUTE_SECOND_MAX_CELL_IN_RECIPE / MAX_CELL;//целая часть
            int mod = recipeCount * MINUTE_SECOND_MAX_CELL_IN_RECIPE % MAX_CELL;//остаток

            var list = new List<short>();

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    short from = (short)(MINUTE_SECOND_START_CELL + ((i * MAX_CELL)));
                    short count = (short)(MAX_CELL) - 1;

                    list.AddRange(GetCodes(from, count));
                }
            }

            if (mod > 0)
            {
                short from = (short)(MINUTE_SECOND_START_CELL + (div * MAX_CELL));
                short count = (short)(mod);

                list.AddRange(GetCodes(from, count));
            }

            return list;

        }

        private List<short> DownloadNames(int recipeCount)
        {

            int div = recipeCount * NAMES_MAX_CELL_IN_RECIPE / MAX_CELL;//целая часть
            int mod = recipeCount * NAMES_MAX_CELL_IN_RECIPE % MAX_CELL;//остаток

            var list = new List<short>();

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    short from = (short)(NAMES_START_CELL + ((i * MAX_CELL)));
                    short count = (short)(MAX_CELL) - 1;

                    list.AddRange(GetCodes(from, count));
                }
            }

            if (mod > 0)
            {
                short from = (short)(NAMES_START_CELL + (div * MAX_CELL));
                short count = (short)(mod);

                list.AddRange(GetCodes(from, count));
            }

            return list;

        }

        private List<short> DownloadCodes(int recipeCount)
        {

            int div = recipeCount * CODES_MAX_CELL_IN_RECIPE / MAX_CELL;//целая часть
            int mod = recipeCount * CODES_MAX_CELL_IN_RECIPE % MAX_CELL;//остаток

            var list = new List<short>();

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    short from = (short)(CODES_START_CELL + ((i * MAX_CELL)));
                    short count = (short)(MAX_CELL) - 1;

                    list.AddRange(GetCodes(from, count));
                }
            }

            if (mod > 0)
            {
                short from = (short)(CODES_START_CELL + (div * MAX_CELL));
                short count = (short)(mod);

                list.AddRange(GetCodes(from, count));
            }

            return list;

        }

        private List<short> DownloadWeights(int recipeCount)
        {

            int div = recipeCount * WEIGHTS_MAX_CELL_IN_RECIPE / MAX_CELL;//целая часть
            int mod = recipeCount * WEIGHTS_MAX_CELL_IN_RECIPE % MAX_CELL;//остаток

            var list = new List<short>();

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    short from = (short)(WEIGHTS_START_CELL + ((i * MAX_CELL)));
                    short count = (short)(MAX_CELL);

                    list.AddRange(GetCodes(from, count));
                }
            }

            if (mod > 0)
            {
                short from = (short)(WEIGHTS_START_CELL + (div * MAX_CELL));
                short count = (short)(mod);

                list.AddRange(GetCodes(from, count));
            }

            return list;

        }

        private List<short> DownloadWater(int recipeCount)
        {

            int div = recipeCount * WATER_MAX_CELL_IN_RECIPE / MAX_CELL;//целая часть
            int mod = recipeCount * WATER_MAX_CELL_IN_RECIPE % MAX_CELL;//остаток

            var list = new List<short>();

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    short from = (short)(WATER_START_CELL + ((i * MAX_CELL)));
                    short count = (short)(MAX_CELL) - 1;

                    list.AddRange(GetCodes(from, count));
                }
            }

            if (mod > 0)
            {
                short from = (short)(WATER_START_CELL + (div * MAX_CELL));
                short count = (short)(mod);

                list.AddRange(GetCodes(from, count));
            }

            return list;

        }

        private List<short> DownloadTemper(int recipeCount)
        {

            int div = recipeCount * TEMPER_MAX_CELL_IN_RECIPE / MAX_CELL;//целая часть
            int mod = recipeCount * TEMPER_MAX_CELL_IN_RECIPE % MAX_CELL;//остаток

            var list = new List<short>();

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    short from = (short)(TEMPER_START_CELL + ((i * MAX_CELL)));
                    short count = (short)(MAX_CELL) - 1;

                    list.AddRange(GetCodes(from, count));
                }
            }

            if (mod > 0)
            {
                short from = (short)(TEMPER_START_CELL + (div * MAX_CELL));
                short count = (short)(mod);

                list.AddRange(GetCodes(from, count));
            }

            return list;

        }

        private List<short> GetCodes(short from,short count)
        {
            short[] array; 
             _etherNetPLC.ReadWords(_plcMemory, from, count, out array);
            return array.ToList();
        }

        #endregion

        public (bool IsError, string Message) ClearMemory()
        {
            var connectResult = ConnectInit();
            if (!connectResult.IsError)
            {
                _etherNetPLC.WriteWord(_plcMemory, CLEAR_CELL, 3);
                _etherNetPLC.Close();
                return (false, "");
            }
            else
            {
                return (true, connectResult.Message);
            }
        }
        private (bool IsError, string Message) ConnectInit()
        {
            if (!PingPLC())
            {
                _etherNetPLC = new EtherNetPLC();
                _etherNetPLC.Link(_ipAddress, 9600, 1000);

                return (false, "");
            }
            else
            {
                return (true, "Не удалось подключиться к ПЛК! Отсутствует PING!");
            }

        }

        private bool PingPLC()
        {
            Ping ping = new Ping();
            PingReply rep;
            rep = ping.Send(_ipAddress, 1000);
            if (rep.Status == IPStatus.Success)//проверка статуса пинга
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Dispose()
        {
            _etherNetPLC?.Close();
        }


    }
}
