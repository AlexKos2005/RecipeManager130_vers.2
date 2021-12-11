using RecipeManager.DAL.DTO;
using RecipeManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.DAL.Extentions
{
   public static class DoseDtoExtention
    {
        public static Dose ToModel(this DoseDto dose)
        {
            return new Dose()
            {
                Date = dose.Date,
                Time = dose.Time,
                RecipeName = dose.RecipeName,
                ComponentCode_1 = dose.ComponentCode_1,
                ComponentWeight_1 = dose.ComponentWeight_1,
                ComponentCode_2 = dose.ComponentCode_2,
                ComponentWeight_2 = dose.ComponentWeight_2,
                ComponentCode_3 = dose.ComponentCode_3,
                ComponentWeight_3 = dose.ComponentWeight_3,
                ComponentCode_4 = dose.ComponentCode_4,
                ComponentWeight_4 = dose.ComponentWeight_4,
                ComponentCode_5 = dose.ComponentCode_5,
                ComponentWeight_5 = dose.ComponentWeight_5,
                ComponentCode_6 = dose.ComponentCode_6,
                ComponentWeight_6 = dose.ComponentWeight_6,
                ComponentCode_7 = dose.ComponentCode_7,
                ComponentWeight_7 = dose.ComponentWeight_7,
                ComponentCode_8 = dose.ComponentCode_8,
                ComponentWeight_8 = dose.ComponentWeight_8,
                ComponentCode_9 = dose.ComponentCode_9,
                ComponentWeight_9 = dose.ComponentWeight_9,
                ComponentCode_10 = dose.ComponentCode_10,
                ComponentWeight_10 = dose.ComponentWeight_10,
                ComponentCode_11 = dose.ComponentCode_11,
                ComponentWeight_11 = dose.ComponentWeight_11,
                ComponentCode_12 = dose.ComponentCode_12,
                ComponentWeight_12 = dose.ComponentWeight_12,
                ComponentCode_13 = dose.ComponentCode_13,
                ComponentWeight_13 = dose.ComponentWeight_13,
                ComponentCode_14 = dose.ComponentCode_14,
                ComponentWeight_14 = dose.ComponentWeight_14,
                ComponentCode_15 = dose.ComponentCode_15,
                ComponentWeight_15 = dose.ComponentWeight_15,
                ComponentCode_16 = dose.ComponentCode_16,
                ComponentWeight_16 = dose.ComponentWeight_16,
                WaterCode = dose.WaterCode,
                WaterWeight = dose.WaterWeight,
                WaterTemper = dose.WaterTemper,
            };
        }
        public static DoseDto FromModel(this Dose dose)
        {
            return new DoseDto()
            {
                Date = dose.Date,
                Time = dose.Time,
                RecipeName = dose.RecipeName,
                ComponentCode_1 = dose.ComponentCode_1,
                ComponentWeight_1 = dose.ComponentWeight_1,
                ComponentCode_2 = dose.ComponentCode_2,
                ComponentWeight_2 = dose.ComponentWeight_2,
                ComponentCode_3 = dose.ComponentCode_3,
                ComponentWeight_3 = dose.ComponentWeight_3,
                ComponentCode_4 = dose.ComponentCode_4,
                ComponentWeight_4 = dose.ComponentWeight_4,
                ComponentCode_5 = dose.ComponentCode_5,
                ComponentWeight_5 = dose.ComponentWeight_5,
                ComponentCode_6 = dose.ComponentCode_6,
                ComponentWeight_6 = dose.ComponentWeight_6,
                ComponentCode_7 = dose.ComponentCode_7,
                ComponentWeight_7 = dose.ComponentWeight_7,
                ComponentCode_8 = dose.ComponentCode_8,
                ComponentWeight_8 = dose.ComponentWeight_8,
                ComponentCode_9 = dose.ComponentCode_9,
                ComponentWeight_9 = dose.ComponentWeight_9,
                ComponentCode_10 = dose.ComponentCode_10,
                ComponentWeight_10 = dose.ComponentWeight_10,
                ComponentCode_11 = dose.ComponentCode_11,
                ComponentWeight_11 = dose.ComponentWeight_11,
                ComponentCode_12 = dose.ComponentCode_12,
                ComponentWeight_12 = dose.ComponentWeight_12,
                ComponentCode_13 = dose.ComponentCode_13,
                ComponentWeight_13 = dose.ComponentWeight_13,
                ComponentCode_14 = dose.ComponentCode_14,
                ComponentWeight_14 = dose.ComponentWeight_14,
                ComponentCode_15 = dose.ComponentCode_15,
                ComponentWeight_15 = dose.ComponentWeight_15,
                ComponentCode_16 = dose.ComponentCode_16,
                ComponentWeight_16 = dose.ComponentWeight_16,
                WaterCode = dose.WaterCode,
                WaterWeight = dose.WaterWeight,
                WaterTemper = dose.WaterTemper,
            };
        }
    }
}
