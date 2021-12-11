using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.BLL.Entities.ReciveEntities;
using RecipeManager.BLL.Interfaces;
using RecipeManager.DAL.DTO;

namespace RecipeManager.BLL.Services
{
    public class CodesConverter : IArrayToCodesConverter
    {
        #region Const
        const int COUNT_CODES_IN_ONE_RECIPE = 190;

        const int DATE_START_CELL = 0;
        const int TIME_START_CELL = 3;
        const int RECIPE_NAME_START_CELL = 10;

        const int COMP_CODE_START_CELL = 20;
        const int COMP_WEIGHT_START_CELL = 22;
        const int COMP_STEP = 10;

        //const int COMP_2_CODE_START_CELL = 30;
        //const int COMP_2_WEIGHT_START_CELL = 32;

        //const int COMP_3_CODE_START_CELL = 40;
        //const int COMP_3_WEIGHT_START_CELL = 42;

        //const int COMP_4_CODE_START_CELL = 50;
        //const int COMP_4_WEIGHT_START_CELL = 52;

        //const int COMP_5_CODE_START_CELL = 60;
        //const int COMP_5_WEIGHT_START_CELL = 62;

        //const int COMP_6_CODE_START_CELL = 70;
        //const int COMP_6_WEIGHT_START_CELL = 72;

        //const int COMP_7_CODE_START_CELL = 80;
        //const int COMP_7_WEIGHT_START_CELL = 82;

        //const int COMP_8_CODE_START_CELL = 90;
        //const int COMP_8_WEIGHT_START_CELL = 92;

        //const int COMP_9_CODE_START_CELL = 100;
        //const int COMP_9_WEIGHT_START_CELL = 102;

        //const int COMP_10_CODE_START_CELL = 110;
        //const int COMP_10_WEIGHT_START_CELL = 112;

        //const int COMP_11_CODE_START_CELL = 120;
        //const int COMP_11_WEIGHT_START_CELL = 122;

        //const int COMP_12_CODE_START_CELL = 130;
        //const int COMP_12_WEIGHT_START_CELL = 132;

        //const int COMP_13_CODE_START_CELL = 140;
        //const int COMP_13_WEIGHT_START_CELL = 142;

        //const int COMP_14_CODE_START_CELL = 150;
        //const int COMP_14_WEIGHT_START_CELL = 152;

        //const int COMP_15_CODE_START_CELL = 160;
        //const int COMP_15_WEIGHT_START_CELL = 162;

        const int WATER_CODE_START_CELL = 170;
        const int WATER_WEIGHT_START_CELL = 172;
        const int WATER_TEMPER_START_CELL = 174;

        const int COMP_COUNT = 15;
        const int WATER_NUMBER = 16;
        #endregion
        public (bool IsError, string Message, List<DoseDto> Doses) ConvertArraysCodes(List<short[]> Codes)
        {
            var listDose = new List<DoseDto>();
            var convertCodesResult = GetSortedCodesLists(Codes);
            if(!convertCodesResult.IsError)
            {
                var doseResult = GetDosesFromCodes(convertCodesResult.recipeCodesList);
                if(!doseResult.IsError)
                {
                    return (false, "", doseResult.Doses);
                }
                else
                {
                    return (true, doseResult.Message, null);
                }
            }
            else
            {
                return (true, convertCodesResult.Message, null);
            }
        }

        #region CodesSortingMethods
        private (bool IsError, string Message, List<ReciveRecipeCodes> recipeCodesList) GetSortedCodesLists(List<short[]> Codes)
        {
            var listCodesRecipes = new List<ReciveRecipeCodes>();
            var currentArrayCodes = 0;

            if (Codes.Count % COUNT_CODES_IN_ONE_RECIPE != 0)
            {
                return (true, "Неверное количество элементов массива", null);
            }

            try
            {
                foreach (var recipe in Codes)
                {
                    var listCodes = new ReciveRecipeCodes();
                    var compList = new List<ReciveComponentCodes>();
                    var waterCodes = new ReciveWaterCodes();
                    listCodes.Date = GetDateByteCodes(recipe);
                    listCodes.Time = GetTimeByteCodes(recipe);
                    listCodes.RecipeName = GetRecipeNameByteCodes(recipe);

                    for (int i = 0; i < COMP_COUNT; i++)
                    {
                        if (i < COMP_COUNT - 1)
                        {
                            var component = new ReciveComponentCodes();
                            component.CompCode = GetComponentCodeByteCodes(recipe, i * COMP_STEP);
                            component.CompWeight = GetComponentWeightByteCodes(recipe, i * COMP_STEP);
                            compList.Add(component);
                        }
                        else
                        {
                            waterCodes.WaterCode = GetWaterCodeByteCodes(recipe, i * COMP_STEP);
                            waterCodes.WaterWeight = GetWaterWeightByteCodes(recipe, i * COMP_STEP);
                            waterCodes.WaterTemper = GetWaterTemperByteCodes(recipe, i * COMP_STEP);
                            listCodes.WaterCodes = waterCodes;
                        }

                    }

                    listCodesRecipes.Add(listCodes);
                    currentArrayCodes++;
                }
                return (false, "", listCodesRecipes);
            }
            catch (Exception e)
            {
                string message = $"Ошибка в полученом массиве кодов: Номер рецепта {currentArrayCodes}" + DateTime.Now.ToShortDateString()+ "//" + e.Message + "//" + e.Source;
                return (false, e.Message, null);
            }
            
        }

        private List<short> GetDateByteCodes(short[] recipe)
        {
            var dateArray = new List<short>();
            for(int i=DATE_START_CELL;i<=DATE_START_CELL+3;i++)
            {
                dateArray.Add(recipe[i]);
            }
            return dateArray;
        }
        private List<short> GetTimeByteCodes(short[] recipe)
        {
            var timeArray = new List<short>();
            for (int i = TIME_START_CELL; i <= TIME_START_CELL + 2; i++)
            {
                timeArray.Add(recipe[i]);
            }
            return timeArray;
        }

        private List<short> GetRecipeNameByteCodes(short[] recipe)
        {
            var recipeNameArray = new List<short>();
            for (int i = RECIPE_NAME_START_CELL; i <= RECIPE_NAME_START_CELL + 8; i++)
            {
                recipeNameArray.Add(recipe[i]);
            }
            return recipeNameArray;
        }

        private List<short> GetComponentCodeByteCodes(short[] recipe,int comp_Count)
        {
            var componentCodeArray = new List<short>();
            for (int i = COMP_CODE_START_CELL+comp_Count; i <= COMP_CODE_START_CELL + comp_Count + 2; i++)
            {
                componentCodeArray.Add(recipe[i]);
            }
            return componentCodeArray;
        }

        private List<short> GetComponentWeightByteCodes(short[] recipe, int comp_Count)
        {
            var componentWeightArray = new List<short>();
            for (int i = COMP_WEIGHT_START_CELL + comp_Count; i <= COMP_WEIGHT_START_CELL + comp_Count + 2; i++)
            {
                componentWeightArray.Add(recipe[i]);
            }
            return componentWeightArray;
        }

        private List<short> GetWaterCodeByteCodes(short[] recipe, int comp_Count)
        {
            var waterCodeArray = new List<short>();
            for (int i = WATER_CODE_START_CELL + comp_Count; i <= WATER_CODE_START_CELL + comp_Count + 2; i++)
            {
                waterCodeArray.Add(recipe[i]);
            }
            return waterCodeArray;
        }

        private List<short> GetWaterWeightByteCodes(short[] recipe, int comp_Count)
        {
            var waterWeightArray = new List<short>();
            for (int i = WATER_WEIGHT_START_CELL + comp_Count; i <= WATER_WEIGHT_START_CELL + comp_Count + 2; i++)
            {
                waterWeightArray.Add(recipe[i]);
            }
            return waterWeightArray;
        }

        private List<short> GetWaterTemperByteCodes(short[] recipe, int comp_Count)
        {
            var waterTemperArray = new List<short>();
            for (int i = WATER_TEMPER_START_CELL + comp_Count; i <= WATER_TEMPER_START_CELL + comp_Count + 2; i++)
            {
                waterTemperArray.Add(recipe[i]);
            }
            return waterTemperArray;
        }
        #endregion

        #region CodesToDosesTransformMethods
        private (bool IsError, string Message, List<DoseDto> Doses) GetDosesFromCodes(List<ReciveRecipeCodes> reciveRecipesCodes)
        {
            var listDoses = new List<DoseDto>();
            int currentDose = 0;
            int currentComponent = 0;
            string currentDateTime = $"{DateTime.Now.ToShortDateString()}/{DateTime.Now.ToShortTimeString()}";

            foreach(var recRecipeCodes in reciveRecipesCodes)
            {
                var dose = new DoseDto();
                //Date
                var dateResult = GetDate(recRecipeCodes.Date);
                if(!dateResult.IsError)
                {
                    dose.Date =(DateTime) dateResult.Date;
                }
                else
                {
                    return (true, $"{currentDateTime}/ Массив кодов:{currentDose}/ Ошибка:{dateResult.Message}", null);
                }
                //Time
                var timeResult = GetTime(recRecipeCodes.Time);
                if (!timeResult.IsError)
                {
                    dose.Time = (TimeSpan) timeResult.Time;
                }
                else
                {
                    return (true, $"{currentDateTime}/ Массив кодов:{currentDose}/ Ошибка:{timeResult.Message}", null);
                }
                //RecipeName
                var recipeNameResult = GetRecipeName(recRecipeCodes.RecipeName);
                if (!recipeNameResult.IsError)
                {
                    dose.RecipeName = recipeNameResult.recipeName;
                }
                else
                {
                    return (true, $"{currentDateTime}/ Массив кодов:{currentDose}/ Ошибка:{recipeNameResult.Message}", null);
                }
                //Components
                MapComponents(recRecipeCodes.ComponentCodes,ref dose);
                dose.WaterCode =(int) GetWaterCode(recRecipeCodes.WaterCodes.WaterCode).waterCode;
                dose.WaterWeight = (int)GetWaterWeight(recRecipeCodes.WaterCodes.WaterWeight).waterWeight;
                dose.WaterTemper = (int)GetWaterTemper(recRecipeCodes.WaterCodes.WaterTemper).waterTemper;

                listDoses.Add(dose);
                currentDose++;

            }

            return (false, "", listDoses);
        }

        private (bool IsError, string Message) MapComponents(List<ReciveComponentCodes> componentCodes, ref DoseDto doseDto)
        {
            doseDto.ComponentCode_1 =(int) GetCompCode(componentCodes[0].CompCode).compCode;
            doseDto.ComponentWeight_1 = (int)GetCompWeight(componentCodes[0].CompWeight).compWeight;
            doseDto.ComponentCode_2 = (int)GetCompCode(componentCodes[1].CompCode).compCode;
            doseDto.ComponentWeight_2 = (int)GetCompWeight(componentCodes[1].CompWeight).compWeight;
            doseDto.ComponentCode_3 = (int)GetCompCode(componentCodes[2].CompCode).compCode;
            doseDto.ComponentWeight_3 = (int)GetCompWeight(componentCodes[2].CompWeight).compWeight;
            doseDto.ComponentCode_4 = (int)GetCompCode(componentCodes[3].CompCode).compCode;
            doseDto.ComponentWeight_4 = (int)GetCompWeight(componentCodes[3].CompWeight).compWeight;
            doseDto.ComponentCode_5 = (int)GetCompCode(componentCodes[4].CompCode).compCode;
            doseDto.ComponentWeight_5 = (int)GetCompWeight(componentCodes[4].CompWeight).compWeight;
            doseDto.ComponentCode_6 = (int)GetCompCode(componentCodes[5].CompCode).compCode;
            doseDto.ComponentWeight_6 = (int)GetCompWeight(componentCodes[5].CompWeight).compWeight;
            doseDto.ComponentCode_7 = (int)GetCompCode(componentCodes[6].CompCode).compCode;
            doseDto.ComponentWeight_7 = (int)GetCompWeight(componentCodes[6].CompWeight).compWeight;
            doseDto.ComponentCode_8 = (int)GetCompCode(componentCodes[7].CompCode).compCode;
            doseDto.ComponentWeight_8 = (int)GetCompWeight(componentCodes[7].CompWeight).compWeight;
            doseDto.ComponentCode_9 = (int)GetCompCode(componentCodes[8].CompCode).compCode;
            doseDto.ComponentWeight_9 = (int)GetCompWeight(componentCodes[8].CompWeight).compWeight;
            doseDto.ComponentCode_10 = (int)GetCompCode(componentCodes[9].CompCode).compCode;
            doseDto.ComponentWeight_10 = (int)GetCompWeight(componentCodes[9].CompWeight).compWeight;
            doseDto.ComponentCode_11 = (int)GetCompCode(componentCodes[10].CompCode).compCode;
            doseDto.ComponentWeight_11 = (int)GetCompWeight(componentCodes[10].CompWeight).compWeight;
            doseDto.ComponentCode_12 = (int)GetCompCode(componentCodes[11].CompCode).compCode;
            doseDto.ComponentWeight_12 = (int)GetCompWeight(componentCodes[11].CompWeight).compWeight;
            doseDto.ComponentCode_13 = (int)GetCompCode(componentCodes[12].CompCode).compCode;
            doseDto.ComponentWeight_13 = (int)GetCompWeight(componentCodes[12].CompWeight).compWeight;
            doseDto.ComponentCode_14 = (int)GetCompCode(componentCodes[13].CompCode).compCode;
            doseDto.ComponentWeight_14 = (int)GetCompWeight(componentCodes[13].CompWeight).compWeight;
            doseDto.ComponentCode_15 = (int)GetCompCode(componentCodes[14].CompCode).compCode;
            doseDto.ComponentWeight_15 = (int)GetCompWeight(componentCodes[14].CompWeight).compWeight;

            return (false, "");
        }
        private (bool IsError,string Message, DateTime? Date) GetDate(List<short> dateCodes)
        {
            string strDate;
            DateTime resultDate;
            if(dateCodes.Count!=3)
            {
                return (true, "Несоответствие кол-ва кодов для св-ва Date", null);
            }
            strDate = $"{dateCodes[2]}/{dateCodes[1]}/{dateCodes[0]}";
            if(DateTime.TryParse(strDate, out resultDate))
            {
                return (false, "", resultDate);
            }
            else
            {
                return (true, "Неудалось преобразование для св-ва Date", null);
            }
        }

        private (bool IsError, string Message, TimeSpan? Time) GetTime(List<short> timeCodes)
        {
            string strTime;
            TimeSpan resultTime;
            if (timeCodes.Count != 2)
            {
                return (true, "Несоответствие кол-ва кодов для св-ва Time", null);
            }
            strTime = $"{timeCodes[2]}/{timeCodes[1]}/{timeCodes[0]}";
            if (TimeSpan.TryParse(strTime, out resultTime))
            {
                return (false, "", resultTime);
            }
            else
            {
                return (true, "Неудалось преобразование для св-ва Time", null);
            }
        }

        private (bool IsError, string Message, string recipeName) GetRecipeName(List<short> recipeNameCodes)
        {
            byte[] array;
            string recipeName;
            if (recipeNameCodes.Count<=0)
            {
                return (true, "Несоответствие кол-ва кодов для поля RecipeName", null);
            }
            try
            {
                array = new byte[recipeNameCodes.Count];
                recipeName = Encoding.Default.GetString(array);
                return (false, "", recipeName);
            }
            catch(Exception e)
            {
                return (true, "Ошибка Encoding свойства RecipeName", null);
            }
            

            
        }

        private (bool IsError, string Message, int? compCode) GetCompCode(List<short> compCodeCodes)
        {
            int compCode;
            if (compCodeCodes.Count <= 0 || compCodeCodes.Count > 2)
            {
                return (true, "Несоответствие кол-ва кодов для поля CompCodes", null);
            }

            compCode = (compCodeCodes[0] * 65536) + compCodeCodes[1];

            return (false,"", compCode);
        }

        private (bool IsError, string Message, int? compWeight) GetCompWeight(List<short> compCodeWeight)
        {
            int compWeight;
            if (compCodeWeight.Count <= 0 || compCodeWeight.Count > 2)
            {
                return (true, "Несоответствие кол-ва кодов для поля CompWeight", null);
            }

            compWeight = (compCodeWeight[0] * 65536) + compCodeWeight[1];

            return (false, "", compWeight);
        }

        private (bool IsError, string Message, int? waterCode) GetWaterCode(List<short> waterCodeCodes)
        {
            int waterCode;
            if (waterCodeCodes.Count <= 0 || waterCodeCodes.Count > 2)
            {
                return (true, "Несоответствие кол-ва кодов для поля WaterCodes", null);
            }

            waterCode = (waterCodeCodes[0] * 65536) + waterCodeCodes[1];

            return (false, "", waterCode);
        }

        private (bool IsError, string Message, int? waterWeight) GetWaterWeight(List<short> waterCodeWeight)
        {
            int waterWeight;
            if (waterCodeWeight.Count <= 0 || waterCodeWeight.Count > 2)
            {
                return (true, "Несоответствие кол-ва кодов для поля WaterWeight", null);
            }

            waterWeight = (waterCodeWeight[0] * 65536) + waterCodeWeight[1];

            return (false, "", waterWeight);
        }

        private (bool IsError, string Message, int? waterTemper) GetWaterTemper(List<short> waterCodeTemper)
        {
            int waterTemper;
            if (waterCodeTemper.Count <= 0 || waterCodeTemper.Count > 2)
            {
                return (true, "Несоответствие кол-ва кодов для поля WaterWeight", null);
            }

            waterTemper = (waterCodeTemper[0] * 65536) + waterCodeTemper[1];

            return (false, "", waterTemper);
        }
        #endregion

    }
}
