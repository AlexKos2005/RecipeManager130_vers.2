using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.BLL.Entities;
using RecipeManager.BLL.Interfaces;
using OmronFinsTCP.Net;
using RecipeManager.BLL.Entities.RecipesCodes;
using System.Threading;

namespace RecipeManager.BLL.Services
{
    public class OmronPLCDataTransceiver : IPLCDataTransceiver,IDisposable
    {
        //10 слов-название+общий вес(8+2),15*12-15 компонентов(8-назвение,2-код,2-вес),10 слов вода(4-название,2-код,2-вес,2-темпер). Итого 200 слов+10 запас=210 на рецепт
        const int START_CELL = 50;
        const short STEP = 210;
        const short STATUS_CELL = 24600;
        const short NULLABLE_CELL = 24601;

        const short RECIPES_NAMES_START_CELL = 0;
        const short RECIPES_NAMES_END_CELL = 1039;

        const short RECIPES_WEIGHTS_START_CELL = 1040;
        const short RECIPES_WEIGHTS_END_CELL = 1299;

        const short COMPONENTS_CODES_START_CELL = 1300;
        const short COMPONENTS_CODES_END_CELL = 3379;

        const short COMPONENTS_NAMES_START_CELL = 3380;
        const short COMPONENTS_NAMES_END_CELL = 20019;

        const short COMPONENTS_WEIGHTS_START_CELL = 20020;
        const short COMPONENTS_WEIGHTS_END_CELL = 24179;

        const short WATERS_WEIGHTS_START_CELL = 24180;
        const short WATERS_WEIGHTS_END_CELL = 24439;

        const short WATERS_TEMPS_START_CELL = 24440;
        const short WATERS_TEMPS_END_CELL = 24569;

        const int MAX_CELL = 480;
        short valueForNullable = 1;


        private readonly string  _ipAddress = null;
        private short _cellPointer = 0;
        private EtherNetPLC _etherNetPLC = null;
        private PlcMemory _plcMemory = PlcMemory.DM;
        
        
        public OmronPLCDataTransceiver()
        {

        }

        public OmronPLCDataTransceiver(string ipAddress)
        {
            _ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        }
     
        public (bool IsError, string Message) SetDataWithResult(List<List<short>> listCodesRecipe, List<Codes> codes, List<CellPLC> cellsPLC, short statusValue)
        {
            var listCodesRec = listCodesRecipe ?? throw new ArgumentNullException(nameof(listCodesRecipe));
            var listCodesDownload = codes ?? throw new ArgumentNullException(nameof(codes));
            var listCellsPLC = cellsPLC ?? throw new ArgumentNullException(nameof(cellsPLC));

            var connectResult = ConnectInit();
            if(connectResult.IsError)
            {
                return (true, "Нет связи с ПЛК! Повторите попытку позже!");
            }
            short status = 0;
            if (!connectResult.IsError)
            {
                SendCode(valueForNullable, NULLABLE_CELL);
                Thread.Sleep(2000);
                UploadRecipesNames(GetAllRecipesNames(listCodesDownload));
                UploadRecipesCodes(GetAllRecipesCodes(listCodesDownload));
                UploadComponentsCodes(GetAllComponentsCodes(listCodesDownload));
                UploadComponentsNames(GetAllComponentsNames(listCodesDownload));
                UploadComponentsWeights(GetAllComponentsWeights(listCodesDownload));
                UploadWatersWeights(GetAllWatersWeights(listCodesDownload));
                UploadWaterTemps(GetAllWaterTemps(listCodesDownload));

                #region vers1
                //foreach (var listCodes in listCodesRec)
                //{
                //    status = SendCodes(listCodes,cellsPLC[count]);
                //    if(status == 0)
                //    {
                //        count++;
                //        if (count >= cellsPLC.Count)
                //        {
                //            SendStatusValue(statusValue);
                //            _etherNetPLC.Close();
                //            return (false, "");
                //        }

                //    }
                //    else
                //    {
                //        _etherNetPLC.Close();
                //        return (true, $"Метод Write вернул -1, {cellsPLC[count].CellAddress}");

                //    }

                //}
                #endregion
            }
            SendStatusValue(statusValue);
            _etherNetPLC.Close();
            return (false, "");

        }
        #region GetAllCodesMethods
        private List<short> GetAllRecipesNames(List<Codes> codesList)
        {
            var namesCodes = new List<short>();
            int counter = 0;
            foreach(var codes in codesList)
            {
                foreach(var nameCodes in codes.RecipesNameCodes)
                {
                    namesCodes.AddRange(nameCodes.Recipe_NameCodes);
                    counter++;
                }
            }
            return namesCodes;
        }

        private List<short> GetAllRecipesCodes(List<Codes> codesList)
        {
            var codesCodes = new List<short>();

            foreach (var codes in codesList)
            {
                foreach (var nameCodes in codes.RecipesWeightCodes)
                {
                    codesCodes.Add(nameCodes.Recipe_WeightCodes_Low);
                    codesCodes.Add(nameCodes.Recipe_WeightCodes_High);
                }
            }
            return codesCodes;
        }

        private List<short> GetAllComponentsCodes(List<Codes> codesList)
        {
            var componentsCodeCodes = new List<short>();

            foreach (var codes in codesList)
            {
                foreach (var codeCodes in codes.ComponentsCodeCodes)
                {
                    componentsCodeCodes.Add(codeCodes.Comp_CodeCodes);
                   
                }
            }
            return componentsCodeCodes;
        }

        private List<short> GetAllComponentsNames(List<Codes> codesList)
        {
            var componentsNamesCodes = new List<short>();

            foreach (var codes in codesList)
            {
                foreach (var nameCodes in codes.ComponentsNameCodes)
                {
                    componentsNamesCodes.AddRange(nameCodes.Comp_NameCodes);

                }
            }
            return componentsNamesCodes;
        }

        private List<short> GetAllComponentsWeights(List<Codes> codesList)
        {
            var componentsWeightCodes = new List<short>();

            foreach (var codes in codesList)
            {
                foreach (var weightCodes in codes.ComponentsWeightCodes)
                {
                    componentsWeightCodes.Add(weightCodes.Comp_WeightCodes_Low);
                    componentsWeightCodes.Add(weightCodes.Comp_WeightCodes_High);

                }
            }
            return componentsWeightCodes;
        }

        private List<short> GetAllWatersWeights(List<Codes> codesList)
        {
            var waterWeightCodes = new List<short>();

            foreach (var codes in codesList)
            {
                foreach (var weightCodes in codes.WaterWeightCodes)
                {
                    waterWeightCodes.Add(weightCodes.Water_WeightCodes_Low);
                    waterWeightCodes.Add(weightCodes.Water_WeightCodes_High);

                }
            }
            return waterWeightCodes;
        }

        private List<short> GetAllWaterTemps(List<Codes> codesList)
        {
            var waterTempCodes = new List<short>();

            foreach (var codes in codesList)
            {
                foreach (var tempCodes in codes.WaterTempCodes)
                {
                    waterTempCodes.Add(tempCodes.Water_TempCodes);
                }
            }
            return waterTempCodes;
        }
        #endregion

        #region UploadMethods
        private void UploadRecipesNames(List<short> nameCodes)
        {
            
            int div = nameCodes.Count / MAX_CELL;//целая часть
            int mod = nameCodes.Count % MAX_CELL;//остаток

            if(div>0)
            {
                for (int i = 0; i < div; i++)
                {
                    var list = GetArrayRange(nameCodes, i* MAX_CELL, (i* MAX_CELL) + MAX_CELL);
                    SendCodes(list, (short)(RECIPES_NAMES_START_CELL + i * MAX_CELL));
                }
            }
           
            if(mod>0)
            {
                var list = GetArrayRange(nameCodes, div* MAX_CELL, nameCodes.Count);
                SendCodes(list, (short)(RECIPES_NAMES_START_CELL + (div* MAX_CELL)));
            }
  
        }
        private void UploadRecipesCodes(List<short> weightCodes)
        {
            //SendCodes(weightCodes, (short)(RECIPES_WEIGHTS_START_CELL));

            int div = weightCodes.Count / MAX_CELL;//целая часть
            int mod = weightCodes.Count % MAX_CELL;//остаток

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    var list = GetArrayRange(weightCodes, i* MAX_CELL, (i * MAX_CELL) + MAX_CELL);
                    SendCodes(list, (short)(RECIPES_WEIGHTS_START_CELL + i * MAX_CELL));
                }
            }

            if (mod > 0)
            {
                var list = GetArrayRange(weightCodes, div * MAX_CELL, weightCodes.Count);
                SendCodes(list, (short)(RECIPES_WEIGHTS_START_CELL + (div * MAX_CELL)));
            }
        }

        private void UploadComponentsCodes(List<short> codeCodes)
        {
            // SendCodes(codeCodes, (short)(COMPONENTS_CODES_START_CELL));

            int div = codeCodes.Count / MAX_CELL;//целая часть
            int mod = codeCodes.Count % MAX_CELL;//остаток

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    var list = GetArrayRange(codeCodes, i* MAX_CELL, (i * MAX_CELL) + MAX_CELL);
                    SendCodes(list, (short)(COMPONENTS_CODES_START_CELL + i * MAX_CELL));
                }
            }

            if (mod > 0)
            {
                var list = GetArrayRange(codeCodes, div * MAX_CELL, codeCodes.Count);
                SendCodes(list, (short)(COMPONENTS_CODES_START_CELL + (div * MAX_CELL)));
            }
        }
        private void UploadComponentsNames(List<short> nameCodes)
        {
            //SendCodes(nameCodes, (short)(COMPONENTS_NAMES_START_CELL));

            int div = nameCodes.Count / MAX_CELL;//целая часть
            int mod = nameCodes.Count % MAX_CELL;//остаток

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    var list = GetArrayRange(nameCodes, i* MAX_CELL, (i * MAX_CELL) + MAX_CELL);
                    SendCodes(list, (short)(COMPONENTS_NAMES_START_CELL + i * MAX_CELL));
                }
            }

            if (mod > 0)
            {
                var list = GetArrayRange(nameCodes, div * MAX_CELL, nameCodes.Count);
                SendCodes(list, (short)(COMPONENTS_NAMES_START_CELL + (div * MAX_CELL)));
            }
        }
        private void UploadComponentsWeights(List<short> weightCodes)
        {
            //SendCodes(weightCodes, (short)(COMPONENTS_WEIGHTS_START_CELL));

            int div = weightCodes.Count / MAX_CELL;//целая часть
            int mod = weightCodes.Count % MAX_CELL;//остаток

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    var list = GetArrayRange(weightCodes, i* MAX_CELL, (i * MAX_CELL) + MAX_CELL);
                    SendCodes(list, (short)(COMPONENTS_WEIGHTS_START_CELL + i * MAX_CELL));
                }
            }

            if (mod > 0)
            {
                var list = GetArrayRange(weightCodes, div * MAX_CELL, weightCodes.Count);
                SendCodes(list, (short)(COMPONENTS_WEIGHTS_START_CELL + (div * MAX_CELL)));
            }
        }

        private void UploadWatersWeights(List<short> weightCodes)
        {
            //SendCodes(weightCodes, (short)(WATERS_WEIGHTS_START_CELL));

            int div = weightCodes.Count / MAX_CELL;//целая часть
            int mod = weightCodes.Count % MAX_CELL;//остаток

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    var list = GetArrayRange(weightCodes, i* MAX_CELL, (i * MAX_CELL) + MAX_CELL);
                    SendCodes(list, (short)(WATERS_WEIGHTS_START_CELL + i * MAX_CELL));
                }
            }

            if (mod > 0)
            {
                var list = GetArrayRange(weightCodes, div * MAX_CELL, weightCodes.Count);
                SendCodes(list, (short)(WATERS_WEIGHTS_START_CELL + (div * MAX_CELL)));
            }
        }

        private void UploadWaterTemps(List<short> tempCodes)
        {
            //SendCodes(tempCodes, (short)(WATERS_TEMPS_START_CELL));

            int div = tempCodes.Count / MAX_CELL;//целая часть
            int mod = tempCodes.Count % MAX_CELL;//остаток

            if (div > 0)
            {
                for (int i = 0; i < div; i++)
                {
                    var list = GetArrayRange(tempCodes, i* MAX_CELL, (i * MAX_CELL) + MAX_CELL);
                    SendCodes(list, (short)(WATERS_TEMPS_START_CELL + i * MAX_CELL));
                }
            }

            if (mod > 0)
            {
                var list = GetArrayRange(tempCodes, div * MAX_CELL, tempCodes.Count);
                SendCodes(list, (short)(WATERS_TEMPS_START_CELL + (div * MAX_CELL)));
            }
        }
        #endregion
        private short SendCodes(List<short> codes,CellPLC cell)
        {
            var array = codes.ToArray();
            //_etherNetPLC.WriteWords(_plcMemory, cell.CellAddress,(short)(cell.CellAddress+STEP),array);  
          return _etherNetPLC.WriteWords(_plcMemory, cell.CellAddress,(short)array.Count(), array);

        }

        private short SendCodes(List<short> codes, short cell)
        {
            var array = codes.ToArray();
            //_etherNetPLC.WriteWords(_plcMemory, cell.CellAddress,(short)(cell.CellAddress+STEP),array);  
            return _etherNetPLC.WriteWords(_plcMemory, cell, (short)array.Count(), array);

        }

        private short SendCode(short code, short cell)
        {
            //_etherNetPLC.WriteWords(_plcMemory, cell.CellAddress,(short)(cell.CellAddress+STEP),array);  
            return _etherNetPLC.WriteWord(_plcMemory, cell, code);

        }
        private short SendStatusValue(short statusValue)
        {
            return _etherNetPLC.WriteWord(_plcMemory, STATUS_CELL, statusValue);
        }

        private List<short> GetArrayRange(List<short> list,int startIndex, int endIndex)
        {
            var resultValues = new List<short>();
            for(int i = startIndex;i<endIndex;i++)
            {
                resultValues.Add(list[i]);
            }
            return resultValues;
        }


        private (bool IsError,string Message) ConnectInit()
        {
            if(!PingPLC())
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
