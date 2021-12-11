using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.BLL.Interfaces;
using RecipeManager.Core.Entities;
using RecipeManager.BLL.Entities;
using RecipeManager.BLL.Entities.RecipesCodes;
using RecipeManager.BLL.Entities.ReciveEntities;
using RecipeManager.DAL.DTO;

namespace RecipeManager.BLL.Services
{
    public class PLCCodeTransformator : ICodesTransformator
    {
        const int YEARMONTH_STEP = 1;
        const int DAYHOUR_STEP = 1;
        const int MINUTESECOND_STEP = 1;
        const int NAMES_STEP = 8;
        const int CODES_STEP = 16;
        const int WEIGHTS_STEP = 32;
        const int WATER_STEP = 2;
        const int TEMPER_STEP = 1;
        public (bool IsError, string Message, List<RecipeCodes> codesRecipes, List<Codes> codes) GetCodesWithResult(List<Recipe> recipes)
        {
            var listCodesRecipe = new List<RecipeCodes>();
            var listCodes = new List<Codes>();

            foreach(var recipe in recipes)
            {
                var codes = new Codes();
                var recipeCodes = new RecipeCodes();
                var recipeNameCodes = new RecipeNameCodes();
                var recipeWeightCodes = new RecipeWeightCodes();

                recipeCodes.RecipeNameCodes = GetCodesOfRecipeNameText(recipe.RecipeName);
                recipeNameCodes.Recipe_NameCodes.AddRange(GetCodesOfRecipeNameText(recipe.RecipeName));

                recipeCodes.ProductCount_LowWord = GetTwoShortValues(recipe.ProductCount).LowValue;
                recipeCodes.ProductCount_HighWord = GetTwoShortValues(recipe.ProductCount).HighValue;
                recipeWeightCodes.Recipe_WeightCodes_Low = GetTwoShortValues(recipe.ProductCount).LowValue;
                recipeWeightCodes.Recipe_WeightCodes_High = GetTwoShortValues(recipe.ProductCount).HighValue;

                codes.RecipesNameCodes.Add(recipeNameCodes);
                codes.RecipesWeightCodes.Add(recipeWeightCodes);

                foreach (var component in recipe.Components)
                {
                    var componentCodes = new ComponentCodes();
                    var compCodeCodes = new ComponentCodeCodes();
                    var compNameCodes = new ComponentNameCodes();
                    var compWeightCodes = new ComponentWeightCodes();

                    componentCodes.Comp_NameCodes = GetCodesOfComponentNameText(component.Comp_Name);
                    compNameCodes.Comp_NameCodes.AddRange(GetCodesOfComponentNameText(component.Comp_Name));

                    componentCodes.Comp_Code = (short)component.Comp_Code;
                    compCodeCodes.Comp_CodeCodes = (short)component.Comp_Code;

                    componentCodes.Comp_Weight_LowWord = GetTwoShortValues(component.Comp_Weight).LowValue;
                    componentCodes.Comp_Weight_HighWord = GetTwoShortValues(component.Comp_Weight).HighValue;
                    compWeightCodes.Comp_WeightCodes_Low = GetTwoShortValues(component.Comp_Weight).LowValue;
                    compWeightCodes.Comp_WeightCodes_High = GetTwoShortValues(component.Comp_Weight).HighValue;

                    recipeCodes.ComponentsCodes.Add(componentCodes);
                    codes.ComponentsCodeCodes.Add(compCodeCodes);
                    codes.ComponentsNameCodes.Add(compNameCodes);
                    codes.ComponentsWeightCodes.Add(compWeightCodes);
                }

                var waterWeightCodes = new WaterWeightCodes();
                var waterTempCodes = new WaterTempCodes();

                recipeCodes.WaterCodes.Comp_NameCodes = GetCodesOfComponentNameText(recipe.WaterComp.Comp_Name);
                recipeCodes.WaterCodes.Comp_Code = (short)recipe.WaterComp.Comp_Code;

                recipeCodes.WaterCodes.Comp_Weight_LowWord = GetTwoShortValues(recipe.WaterComp.Comp_Weight).LowValue;
                recipeCodes.WaterCodes.Comp_Weight_HighWord = GetTwoShortValues(recipe.WaterComp.Comp_Weight).HighValue;
                waterWeightCodes.Water_WeightCodes_Low = GetTwoShortValues(recipe.WaterComp.Comp_Weight).LowValue;
                waterWeightCodes.Water_WeightCodes_High = GetTwoShortValues(recipe.WaterComp.Comp_Weight).HighValue;

                recipeCodes.WaterCodes.Comp_Temper = (short) recipe.WaterComp.Comp_Temper;
                waterTempCodes.Water_TempCodes = (short)recipe.WaterComp.Comp_Temper;

                codes.WaterWeightCodes.Add(waterWeightCodes);
                codes.WaterTempCodes.Add(waterTempCodes);

                listCodes.Add(codes);
                listCodesRecipe.Add(recipeCodes);
            }
            return (false,"",listCodesRecipe,listCodes);
        }

        private List<short> GetCodesOfRecipeNameText(string str)
        {
            var codes = new List<byte>();
            var codesResult = new List<short>();
            var symbols = str.ToCharArray();
            codes.AddRange(Encoding.Default.GetBytes(symbols));
            int count = codes.Count <= 16 ? codes.Count : 16;
            for (int i = 0; i < count; i = i + 2)
            {
                if (count % 2 == 0)//проверка на четное количество кодов
                {
                    codesResult.Add((short)(codes[i] * 256 + codes[i + 1]));
                }
                else
                {
                    if(i==(count-1))//конец, последний символ
                    {
                        codesResult.Add((short)(codes[i] * 256));
                    }
                    else // по паре символов
                    {
                        codesResult.Add((short)(codes[i] * 256 + codes[i + 1]));
                    }
                    
                }

            }

            if (count < 16)//если кодов было меньше 32
            {
                for (int i = codesResult.Count(); i < 8; i++)//смотрим сколько пар уже записано, остальное делаем нулями
                {
                    codesResult.Add((short)(0));
                }
            }
            return codesResult;
        }
        private List<short> GetCodesOfComponentNameText(string str)
        {
            var codes = new List<byte>();
            var codesResult = new List<short>();
            var symbols = str.ToCharArray();
            codes.AddRange(Encoding.Default.GetBytes(symbols));
            int count = codes.Count <= 16 ? codes.Count : 16;
            for (int i=0;i<count;i=i+2)
            {
                if (count % 2 == 0)//проверка на четное количество кодов
                {
                    codesResult.Add((short)(codes[i] * 256 + codes[i + 1]));
                }
                else
                {
                    if (i == (count - 1))//конец, последний символ
                    {
                        codesResult.Add((short)(codes[i] * 256));
                    }
                    else // по паре символов
                    {
                        codesResult.Add((short)(codes[i] * 256 + codes[i + 1]));
                    }

                }

            }

            if(count<16)//если кодов было меньше 16
            {
                for(int i=codesResult.Count();i<8;i++)//смотрим сколько пар уже записано, остальное делаем нулями
                {
                    codesResult.Add((short)(0));
                }
            }
            return codesResult;   
        }

        private (short LowValue,short HighValue) GetTwoShortValues(int value)
        {
            short low, high = 0;
            int div, mod = 0;

            if (value <65536)
            {
                low = (short)value;
                high = 0;
                return(low, high);
            }
            else
            {
                 div = value / 65536;
                 mod = value % 65536;

                low =(short) mod;
                high =(short) div;
                return (low, high);
            }
        }

        public (bool IsError, string Message, List<DoseDto> doses) GetRecipesWithResult(ReciveCodes codes)
        {
            int doseCounter = 0;
            var doses = new List<DoseDto>();

            var listRecipesCodesResult = MakeListCodes(codes);
            if(!listRecipesCodesResult.IsError)
            {
                foreach(var recipe in listRecipesCodesResult.reciveCodes)
                {
                    var dose = new DoseDto();

                    dose.Date = GetDoseDate(recipe);
                    dose.Time = GetDoseTime(recipe);
                    dose.RecipeName = GetDoseRecipeName(recipe);
                    var listCodes = GetDoseCompCodes(recipe);
                    var listWeights = GetDoseCompWeights(recipe);
                    WriteCodesAndWeights(ref dose, GetDoseCompCodes(recipe), GetDoseCompWeights(recipe));
                    dose.WaterWeight = GetDoseWaterWeight(recipe);
                    dose.WaterTemper = GetDoseWaterTemper(recipe);

                    doses.Add(dose);

                }
                return (false, "", doses);
            }
            else
            {
                return (true, "Ошибка преобразования в коды рецептов", null);
            }
            
        }

        private (bool IsError, string Message,List<ReciveCodes> reciveCodes) MakeListCodes(ReciveCodes reciveCodes)
        {
            var listCodes = new List<ReciveCodes>();

            try
            {
                for (int i = 0; i < reciveCodes.YearMonth.Count; i++)
                {
                    var codes = new ReciveCodes();
                    codes.YearMonth = GetYearMonthCodes(i, reciveCodes);
                    codes.DayHour = GetDayHourCodes(i, reciveCodes);
                    codes.MinuteSecond = GetMinuteSecondCodes(i, reciveCodes);
                    codes.RecipesNames = GetRecipeNameCodes(i, reciveCodes);
                    codes.ComponentsCodes = GetComponentsCodesCodes(i, reciveCodes);
                    codes.ComponentsWeights = GetComponentsWeightsCodes(i, reciveCodes);
                    codes.WaterWeights = GetWaterWeightCodes(i, reciveCodes);
                    codes.WaterTempers = GetWaterTemperCodes(i, reciveCodes);

                    listCodes.Add(codes);
                }
                return (false, "", listCodes);
            }
            catch(Exception e)
            {
                return (true, e.Message, null);
            }
            
        }

        private List<short> GetYearMonthCodes(int recipeCount,ReciveCodes reciveCodes)
        {
            var yearmonth = new List<short>();

            yearmonth.Add(reciveCodes.YearMonth[recipeCount*YEARMONTH_STEP]);

            return yearmonth;


        }

        private List<short> GetDayHourCodes(int recipeCount, ReciveCodes reciveCodes)
        {
            var dayhour = new List<short>();

            dayhour.Add(reciveCodes.DayHour[recipeCount * DAYHOUR_STEP]);

            return dayhour;
        }

        private List<short> GetMinuteSecondCodes(int recipeCount, ReciveCodes reciveCodes)
        {
            var minutesecond = new List<short>();

            minutesecond.Add(reciveCodes.MinuteSecond[recipeCount * MINUTESECOND_STEP]);

            return minutesecond;
        }

        private List<short> GetRecipeNameCodes(int recipeCount, ReciveCodes reciveCodes)
        {
            var recipeName = new List<short>();

            for(int i=0;i<NAMES_STEP;i++)
            {
                recipeName.Add(reciveCodes.RecipesNames[(recipeCount*NAMES_STEP)+ i]);
            }
            
            return recipeName;
        }

        private List<short> GetComponentsCodesCodes(int recipeCount, ReciveCodes reciveCodes)
        {
            var componentCodes = new List<short>();

            for (int i = 0; i < CODES_STEP; i++)
            {
                componentCodes.Add(reciveCodes.ComponentsCodes[(recipeCount * CODES_STEP) + i]);
            }

            return componentCodes;
        }

        private List<short> GetComponentsWeightsCodes(int recipeCount, ReciveCodes reciveCodes)
        {
            var componentWeights = new List<short>();

            for (int i = 0; i < WEIGHTS_STEP; i++)
            {
                componentWeights.Add(reciveCodes.ComponentsWeights[(recipeCount * WEIGHTS_STEP) + i]);
            }

            return componentWeights;
        }

        private List<short> GetWaterWeightCodes(int recipeCount, ReciveCodes reciveCodes)
        {
            var waterWeights = new List<short>();

            for (int i = 0; i < WATER_STEP; i++)
            {
                waterWeights.Add(reciveCodes.WaterWeights[(recipeCount * WATER_STEP) + i]);
            }

            return waterWeights;
        }

        private List<short> GetWaterTemperCodes(int recipeCount, ReciveCodes reciveCodes)
        {
            var waterTemper = new List<short>();

            for (int i = 0; i < TEMPER_STEP; i++)
            {
                waterTemper.Add(reciveCodes.WaterTempers[(recipeCount * TEMPER_STEP) + i]);
            }

            return waterTemper;
        }

        
        private DateTime GetDoseDate(ReciveCodes codes)
        {
            DateTime dt;
            string b = codes.YearMonth[0].ToString("X");
            string c = codes.DayHour[0].ToString("X");
            char[] yearmonth = b.ToCharArray();
            char[] dayhour = c.ToCharArray();
            if (yearmonth.Count() == 3) //если первым символом был незначащий ноль, то его сотрет. Нужно чтобы был (кол-во символов = 4).
            {
                 yearmonth = new char[4] {'0',yearmonth[0],yearmonth[1],yearmonth[2]}; 
            }
            if (yearmonth.Count() == 2) //если первым символом был незначащий ноль, то его сотрет. Нужно чтобы был (кол-во символов = 4).
            {
                yearmonth = new char[4] { '0', '0', yearmonth[0], yearmonth[1] };
            }
            if (dayhour.Count() == 3) //если первым символом был незначащий ноль, то его сотрет. Нужно чтобы был (кол-во символов = 4).
            {
                dayhour = new char[4] { '0', dayhour[0], dayhour[1], dayhour[2] };
            }
            if (dayhour.Count() == 2) //если первым символом был незначащий ноль, то его сотрет. Нужно чтобы был (кол-во символов = 4).
            {
                dayhour = new char[4] { '0', '0', dayhour[0], dayhour[1] };
            }


            dt = Convert.ToDateTime($"{dayhour[0]}{dayhour[1]}/{yearmonth[2]}{yearmonth[3]}/{yearmonth[0]}{yearmonth[1]}");
            return dt;
            
        }

        private TimeSpan GetDoseTime(ReciveCodes codes)
        {
            TimeSpan time;
            string b = codes.DayHour[0].ToString("X");
            string c = codes.MinuteSecond[0].ToString("X");
            char[] dayhour = b.ToCharArray();
            char[] minutesecond = c.ToCharArray();
            if (dayhour.Count() == 3) //если первым символом был незначащий ноль, то его сотрет. Нужно чтобы был (кол-во символов = 4).
            {
                dayhour = new char[4] { '0', dayhour[0], dayhour[1], dayhour[2] };
            }
            if (dayhour.Count() == 2) //если первым символом был незначащий ноль, то его сотрет. Нужно чтобы был (кол-во символов = 4).
            {
                dayhour = new char[4] { '0', '0', dayhour[0], dayhour[1]};
            }
            if (minutesecond.Count() == 3) //если первым символом был незначащий ноль, то его сотрет. Нужно чтобы был (кол-во символов = 4).
            {
                minutesecond = new char[4] { '0', minutesecond[0], minutesecond[1], minutesecond[2] };
            }
            if (minutesecond.Count() == 2) //если первым символом был незначащий ноль, то его сотрет. Нужно чтобы был (кол-во символов = 4).
            {
                minutesecond = new char[4] { '0', '0', minutesecond[0], minutesecond[1]};
            }

            time = Convert.ToDateTime($"{dayhour[2]}{dayhour[3]}:{minutesecond[0]}{minutesecond[1]}:{minutesecond[2]}{minutesecond[3]}").TimeOfDay;
            return time;

        }

        private string GetDoseRecipeName(ReciveCodes codes)
        {
            string name;
            int counter = 0;
            byte[] symbols = new byte[16];
            
            for(int i=0;i<codes.RecipesNames.Count;i++)
            {
                symbols[counter] = (byte)(((ushort)codes.RecipesNames[i]) / 256);
                symbols[counter + 1] = (byte)(((ushort)codes.RecipesNames[i]) % 256);
                counter += 2;
            }

            name = Encoding.Default.GetString(symbols).Trim('\0');

            return name;

        }

        private List<int> GetDoseCompCodes(ReciveCodes codes)
        {
            var list = new List<int>();
            
            for (int i = 0; i < codes.ComponentsCodes.Count; i++)
            {
                list.Add(codes.ComponentsCodes[i]);
            }

            return list;

        }

        private List<int> GetDoseCompWeights(ReciveCodes codes)
        {
            var list = new List<int>();

            for (int i = 0; i < codes.ComponentsWeights.Count; i+=2)
            {
                int vLow = codes.ComponentsWeights[i] < 0 ? codes.ComponentsWeights[i] + 65536 : codes.ComponentsWeights[i];
                int vHigh = codes.ComponentsWeights[i+1] < 0 ? codes.ComponentsWeights[i+1] + 65536 : codes.ComponentsWeights[i+1];
                int value = vLow + (vHigh * 65536);
                list.Add(value);
            }

            return list;

        }

        private int GetDoseWaterWeight(ReciveCodes codes)
        {
            int value = 0;
            int vLow = codes.WaterWeights[0] < 0 ? codes.WaterWeights[0] + 65536 : codes.WaterWeights[0];
            int vHigh = codes.WaterWeights[1] < 0 ? codes.WaterWeights[1] + 65536 : codes.WaterWeights[1];
            value = vLow + (vHigh * 65536);

            return value;
        }

        private int GetDoseWaterTemper(ReciveCodes codes)
        {
            int value = 0;

            value = codes.WaterTempers[0];

            return value;
        }

        private bool WriteCodesAndWeights(ref DoseDto dose,List<int> codes,List<int> weights)
        {
            try
            {
                dose.ComponentCode_1 = codes[0];
                dose.ComponentWeight_1 = weights[0];

                dose.ComponentCode_2 = codes[1];
                dose.ComponentWeight_2 = weights[1];

                dose.ComponentCode_3 = codes[2];
                dose.ComponentWeight_3 = weights[2];

                dose.ComponentCode_4 = codes[3];
                dose.ComponentWeight_4 = weights[3];

                dose.ComponentCode_5 = codes[4];
                dose.ComponentWeight_5 = weights[4];

                dose.ComponentCode_6 = codes[5];
                dose.ComponentWeight_6 = weights[5];

                dose.ComponentCode_7 = codes[6];
                dose.ComponentWeight_7 = weights[6];

                dose.ComponentCode_8 = codes[7];
                dose.ComponentWeight_8 = weights[7];

                dose.ComponentCode_9 = codes[8];
                dose.ComponentWeight_9 = weights[8];

                dose.ComponentCode_10 = codes[9];
                dose.ComponentWeight_10 = weights[9];

                dose.ComponentCode_11 = codes[10];
                dose.ComponentWeight_11 = weights[10];

                dose.ComponentCode_12 = codes[11];
                dose.ComponentWeight_12 = weights[11];

                dose.ComponentCode_13 = codes[12];
                dose.ComponentWeight_13 = weights[12];

                dose.ComponentCode_14 = codes[13];
                dose.ComponentWeight_14 = weights[13];

                dose.ComponentCode_15 = codes[14];
                dose.ComponentWeight_15 = weights[14];

                dose.ComponentCode_16 = codes[15];
                dose.ComponentWeight_16 = weights[15];

                //dose.WaterCode = codes[16];

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            
        }


    }
}
